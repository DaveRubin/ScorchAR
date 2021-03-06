using System.Collections.Generic;
using DG.Tweening;
using ScorchEngine;
using ScorchEngine.Config;
using ScorchEngine.Models;
using ScorchEngine.Server;
using Server;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using System;

public class MainGame : MonoBehaviour
{

    public const float TANK_SCALE = 1f;
    public const int MAP_SIZE = 64;

    public static Action<EGameStatus> statusChanged;
    public static EGameStatus currentStatus = EGameStatus.PLAYING;
    public DestructibleObject treeObstacle;
    public DestructibleObject crateObstacle;
    public DestructibleObject boulderObstacle;

    private bool OFFLINE_MODE = false;
    private bool VUFORIA = true;

    private TankControl MyTank {
        get {
            return tanks[PlayerIndex];
        }
    }

    private TankControl OpponentTank {
        get {
            return tanks[(PlayerIndex+1)%2];
        }
    }

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    public const int MAX_SCORE = 3;

    public CameraGUI Gui;
    public static ScorchEngine.Game GameCore;
    public static string gameID;
    private List<TankControl> tanks;
    private List<float> tanksHeight;
    private List<DestructibleObject> obstacles;
    public static int PlayerIndex;
    private Transform rootTransform;

    private Transform obstaclesRoot;
    public static GameObject terrain;
    public static Terrain terrainComp;
    private VuforiaWrapper vuforiaWrapper;
    private Tween terrainTween;

    private int currentRound = 0;

    public const float POLL_FREQUENCY = 0.2f;

    public static GameObject GetTerrain()
    {
        return terrain;
    }

    void Awake() {
        if (SceneManager.GetActiveScene().name == "DLLTest") {
            OFFLINE_MODE = true;
        }
        Application.targetFrameRate = 60;
        PrefabManager.Init();
        rootTransform = new GameObject().transform;
        rootTransform.gameObject.name = "Root";
        //rootTransform.position = new Vector3(-25,0,-25);
        rootTransform.position = Vector3.zero;
        vuforiaWrapper = new VuforiaWrapper();
        vuforiaWrapper.onStatusChange += OnTrackerDetection;

        if (OFFLINE_MODE) {
            MainUser.Instance.Name = "Test";
            MainUser.Instance.Index = 1;
            MainUser.Instance.CurrentGame = new GameInfo{
                Id = "20",
                Name = "GAME"
            };
            Game.OFFLINE = true;
            gameID = MainUser.Instance.CurrentGame.Id;
            PostLogin(MainUser.Instance.Index);
        }
        else {
            gameID = MainUser.Instance.CurrentGame.Id;
            PostLogin(MainUser.Instance.Index);
        }

    }

    void OnDestroy() {
        UnityServerWrapper.Instance.RemovePlayerFromGame(gameID,PlayerIndex,()=> {
            Debug.LogError("Exiting");
        });
    }

    /// <summary>
    /// After login is called start game...
    /// Needs error handling exc...
    /// </summary>
    /// <param name="index"></param>
    public void PostLogin(int index) {
        PlayerIndex = index;
        InitGame();
        CreateTerrain();
        InitializePlayers();
        InitializeObstacles();
        //CreateMockTerrain();
        InitializeGUI();

        //after all is set, start polling from server for changes
        GameCore.MyID = PlayerIndex;
        InvokeRepeating("Poll", 1, POLL_FREQUENCY);
        //OverlayControl.Instance.ToggleLoading(false);
        if (VUFORIA)
        {
            vuforiaWrapper.Init();
        }
        else {
            Gui.DisableErrors();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            //ResetGame();
            onTankKilled(tanks[1]);
        }

    }

    private void Poll() {
        PlayerState pState = new PlayerState();
        pState.Id = PlayerIndex;
        pState.AngleHorizontal = MyTank.PlayerStats.ControlledTank.AngleHorizontal;
        pState.Force = MyTank.PlayerStats.ControlledTank.Force;
        pState.AngleVertical= MyTank.PlayerStats.ControlledTank.AngleVertical;
        pState.IsReady = MyTank.PlayerStats.ControlledTank.IsReady;
        MyTank.PlayerStats.ControlledTank.IsReady = false;
        pState.PositionX = MyTank.PlayerStats.ControlledTank.PositionX;
        pState.PositionY = MyTank.PlayerStats.ControlledTank.PositionY;
        pState.PositionZ = MyTank.PlayerStats.ControlledTank.PositionZ;

        if (!OFFLINE_MODE) {
            UnityServerWrapper.Instance.UpdatePlayerState(MainUser.Instance.CurrentGame.Id, pState, OnPollResult);
        }
    }

    /// <summary>
    /// According to players given, create for each one a tank, position it and link add it to game
    /// </summary>
    public void InitializePlayers() {

        Transform tanksRoot= new GameObject().transform;
        tanksRoot.gameObject.name = "Tanks";
        tanksRoot.SetParent(rootTransform);
        tanksRoot.localPosition = Vector3.zero;

        tanks = new List<TankControl>();
        tanksHeight  = new List<float>();

        //Add players in game,
        //for each player create a tank, and initialize
        List<Player> players = new List<Player>() {
            Player.CreateMockPlayer(),
            Player.CreateMockPlayer()
        };

        foreach (Player player in players) {
            GameCore.AddPlayer(player);
        }

        terrainComp = terrain.GetComponentInChildren<Terrain>();
        terrainComp.terrainData.size = new Vector3(64, 60, 64);
        initTanks(players, tanksRoot);
		for (int j = 0; j < tanks.Count; j++) {
            TankColorer colorer = tanks[j].GetComponent<TankColorer>();
            if (j == 0) {
                colorer.Color(new Color(0.7294118f, 0.0f, 0.0f),
                        new Color(0.39215687f, 0.0f, 0.039215688f),
                        Color.black);
            }
            else {
                colorer.Color(new Color(0.0f, 0.44705883f, 0.7294118f),
                        new Color(0.0f, 0.18431373f, 0.39215687f),
                        Color.black);
            }
        }
    }

    public void InitializeObstacles()
    {
        obstaclesRoot = new GameObject().transform;
        obstaclesRoot.gameObject.name = "Obstacles";
        obstaclesRoot.SetParent(rootTransform);
        obstaclesRoot.localPosition = Vector3.zero;
        initObstacles();
    }

    private void initTanks(List<Player> players, Transform tanksRoot)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            TankControl tankGO = PrefabManager.InstantiatePrefab("Tank").GetComponent<TankControl>();
            tankGO.transform.SetParent(tanksRoot);
   
            float x = MainUser.Instance.CurrentGame.PlayerPositions[currentRound][i].X;
            float z = MainUser.Instance.CurrentGame.PlayerPositions[currentRound][i].Y;
            tankGO.onKill += onTankKilled;
            tankGO.onHit += onTankHit;
            float height = terrainComp.SampleHeight(new Vector3(x, 0, z));
            tankGO.transform.localPosition = new Vector3(x, height, z);
            tankGO.transform.localScale = Vector3.one * TANK_SCALE;

            tankGO.SetPlayer(players[i]);
            tankGO.PlayerStats.ControlledTank.PositionX = x;
            tankGO.PlayerStats.ControlledTank.PositionY = height;
            tankGO.PlayerStats.ControlledTank.PositionZ = z;
            tanks.Add(tankGO);
            tanksHeight.Add(height);
        }
    }

    /// <summary>
    /// Create terrain
    /// TODO - need to randomize obstacle location and verify that it is not next to the tanks...
    /// </summary>
    private void initObstacles()
    {

        obstacles = new List<DestructibleObject>();
        List<Point> destructableObjectsPositions = MainUser.Instance.CurrentGame.DestructableObjectPositions[currentRound];
        DestructibleObject destructableObject;
        for (int i = 0; i < destructableObjectsPositions.Count; ++i)
        {
            float x = destructableObjectsPositions[i].X;
            float z = destructableObjectsPositions[i].Y;
            float height = terrainComp.SampleHeight(new Vector3(x, 0, z));
            if (i % 3 == 0)
            {
                destructableObject = Instantiate(treeObstacle, new Vector3(x, height, z), Quaternion.identity);
            }
            else if (i % 3 == 1)
            {
                destructableObject  = Instantiate(crateObstacle, new Vector3(x, height, z), Quaternion.identity);
            }
            else
            {
                destructableObject = Instantiate(boulderObstacle, new Vector3(x, height, z), Quaternion.identity);
            }
            destructableObject.transform.SetParent(obstaclesRoot);
            obstacles.Add(destructableObject);
        }
    }

    /// <summary>
    /// Create terrain
    /// TODO - should find a way to have tanks on the terrain...
    /// </summary>
    public void CreateTerrain() {

        Transform terrainRoot= new GameObject().transform;

        terrain = PrefabManager.InstantiatePrefab("Terrain");
        terrain.transform.SetParent(terrainRoot);
        terrain.GetComponent<TerrainDeform>().onTerrainDeformed.AddListener(OnTerrainDeform);
        terrainRoot.gameObject.name = "Terrain";
        terrainRoot.SetParent(rootTransform);
        terrainRoot.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Create game object ,initialize config and other global vars
    /// TODO - config should be fetched from server!
    /// </summary>
    public void InitGame() {
        GameConfig conf = new GameConfig();
        conf.Size = new int[]{ 50, 50 };
        conf.MaxPlayers = 2;
        GameCore = new Game(conf);
        Game.debugLog += (string obj) => Debug.Log(obj);
        GameCore.TurnStarted += OnTurnStarted;

    }

    /// <summary>
    /// Setup gui, link tanks and game to it
    /// </summary>
    public void InitializeGUI() {
        Gui = GameObject.Find("GUI").GetComponent<CameraGUI>();
        Debug.LogFormat("Player index {0}",PlayerIndex);
        TankControl tc = tanks[PlayerIndex];
        tc.LinkToGUI(Gui);
    }

    public void OnTurnStarted(int playerIndex) {
        Debug.LogFormat("Player #{0} turn ", playerIndex);
    }

    public void OnTrackerDetection(bool detected) {
        Debug.Log(detected);
        ToggleMapHeight(detected);
        Gui.ToggleTrackerDetection(detected);
    }

    private void OnRoundEnded(int loserIndex) {
        if (loserIndex == 1) {
            Debug.LogError("p1 ++");
            scoreP1++;
        }
        else {
            Debug.LogError("p2 ++");
            scoreP2++;
        }

        if (scoreP1 != MAX_SCORE && scoreP2 != MAX_SCORE) {
            currentRound++;
            Gui.ShowEndRound(tanks[loserIndex] != MyTank,scoreP1,scoreP2,tanks,()=> {
                ResetGame();
            });
        }
        else {
            Gui.ShowEndGame(tanks[loserIndex] != MyTank).AddListener(()=> {
                UnityServerWrapper.Instance.RemovePlayerFromGame(gameID,PlayerIndex, () => {
                    SceneManager.LoadScene("Menus");
                });
            });
        }
    }

    public void onTankKilled(TankControl tank) {
        //set score
        int winnerIndex = tank == tanks[0]? 0:1;
        UnityServerWrapper.Instance.NotifiyRoundWinner(MainUser.Instance.CurrentGame.Id,winnerIndex);
    }

    public void onTankHit(TankControl tank)
    {
        Gui.DoOnHitAnimation(tank == MyTank);
    }

    public void ToggleMapHeight(bool show) {

        if (terrainTween != null)
            terrainTween.Kill();

        float duration = 0.6f;
        float from = show?0:60;
        float to = show?60:0;

        if (show) terrainComp.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0,DOVirtual.Float(from,to,duration,val=> {
            terrainComp.terrainData.size = new Vector3(64, val, 64);
        }));
        sequence.InsertCallback(duration,()=> {
            if (!show)  terrainComp.gameObject.SetActive(false);
        });

        for (int i = 0; i < tanks.Count; i++) {
            TankControl tank = tanks[i];
            sequence.Insert(0,tank.transform.DOLocalMoveY(show?tanksHeight[i]:200,duration));
        }

        terrainTween = sequence;


    }

    public void OnPollResult(PollResult pollResult) {
        GameCore.OnPollResult(pollResult.PlayerStates);
        int serverStatus = pollResult.RoundWinnerIndex;
        //Debug.LogFormat("got server status{0}",pollResult.RoundWinnerIndex);
        if (serverStatus != (int)currentStatus) {
            currentStatus = (EGameStatus)serverStatus;
            if (currentStatus !=  EGameStatus.PLAYING) {
                OnRoundEnded(pollResult.RoundWinnerIndex);
            }
            Debug.LogErrorFormat("CHanged status to {0}",currentStatus);
            if (statusChanged != null) statusChanged(currentStatus);
        }
        //Debug.LogErrorFormat("op is active: {0}", OpponentTank.PlayerStats.ControlledTank.IsActive);
        if (!OpponentTank.PlayerStats.ControlledTank.IsActive) {
            Gui.RemovePlayer();
        }
    }

    public void OnTerrainDeform() {
        //update tanks height...
        foreach (TankControl tank in tanks) {
            Vector3 tankPos = tank.transform.localPosition;
            float height = terrainComp.SampleHeight(new Vector3(tankPos.x, 0, tankPos.z));
            Debug.LogFormat("Tank {0} - old {1} new {2} ",tank.gameObject.name,tankPos.y,height);
            if (height != tankPos.y) {
                Debug.LogWarning("HIRTTTTTT");
                tank.transform.DOLocalMoveY(height,1);
            }
        }

        //update obstacle height...
        foreach (DestructibleObject obstacle in obstacles) {
            Vector3 obstaclePos = obstacle.transform.localPosition;
            float height = terrainComp.SampleHeight(new Vector3(obstaclePos.x, 0, obstaclePos.z));
            Debug.LogFormat("OBSTACLE {0} - old {1} new {2} ",obstacle.gameObject.name,obstaclePos.y,height);
            if (height != obstaclePos.y && !obstacle.name.Equals("Obstacle_Boulder") ) {
                Debug.LogWarning("SHHHRAAAA");
                obstacle.transform.DOLocalMoveY(height,1);
            }
        }
    }

    public void ResetGame() {
        //reset map
        terrain.GetComponent<TerrainLoader>().Reload();
        //reset tanks
        foreach (TankControl tank in tanks) {
            tank.ResetTank();
        }
        //set positions
        Gui.ResetGUI();
        for (int i = 0; i < tanks.Count; i++) {
            TankControl control = tanks[i];
            float x = MainUser.Instance.CurrentGame.PlayerPositions[currentRound][i].X;
            float z = MainUser.Instance.CurrentGame.PlayerPositions[currentRound][i].Y;
            float height = terrainComp.SampleHeight(new Vector3(x, 0, z));
            control.transform.localPosition = new Vector3(x, height, z);
            control.transform.localScale = Vector3.one * TANK_SCALE;
            control.PlayerStats.ControlledTank.PositionX = x;
            control.PlayerStats.ControlledTank.PositionY = height;
            control.PlayerStats.ControlledTank.PositionZ = z;
        }
        destroyObstacles();
    }
    private void destroyObstacles()
    {
        foreach(DestructibleObject destructibleObject in obstacles)
        {
            GameObject.Destroy(destructibleObject.gameObject);
        }
        initObstacles();
    }
   

}

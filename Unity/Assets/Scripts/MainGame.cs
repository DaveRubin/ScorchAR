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

public class MainGame : MonoBehaviour {

    private bool OFFLINE_MODE = true;
    private bool VUFORIA = false;

    private TankControl MyTank {
        get {
            return tanks[PlayerIndex];
        }
    }

    public CameraGUI Gui;
    public static ScorchEngine.Game GameCore;
    public static string gameID;
    private List<TankControl> tanks;
    private List<float> tanksHeight;
    private int PlayerIndex;
    private Transform rootTransform;
    public static GameObject terrain;
    private Terrain terrainComp;
    private VuforiaWrapper vuforiaWrapper;
    private Tween terrainTween;

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
        //CreateMockTerrain();
        InitializeGUI();

        //after all is set, start polling from server for changes
        GameCore.MyID = PlayerIndex;
        InvokeRepeating("Poll", 1, 1f);
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
            OnTrackerDetection(true);
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            OnTrackerDetection(false);
        }

    }

    private void Poll() {
        PlayerState pState = new PlayerState();
        Vector3 myPos= MyTank.gameObject.transform.localPosition;
        pState.Id = PlayerIndex;
        pState.AngleHorizontal = MyTank.PlayerStats.ControlledTank.AngleHorizontal;
        pState.Force = MyTank.PlayerStats.ControlledTank.Force;
        pState.AngleVertical= MyTank.PlayerStats.ControlledTank.AngleVertical;
        pState.IsReady = MyTank.PlayerStats.ControlledTank.IsReady;
        pState.PositionX = myPos.x;
        pState.PositionY = myPos.y;
        pState.PositionZ = myPos.z;
        //Debug.LogFormat(pState.ToString());
        if (!OFFLINE_MODE) {
            UnityServerWrapper.Instance.UpdatePlayerState(MainUser.Instance.CurrentGame.Id, pState, OnPollResult);
        }
       // GameCore.Poll(MainUser.Instance.CurrentGame.Id,pState);
        MyTank.PlayerStats.ControlledTank.IsReady = false;
        Gui.SetLocked(false);
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
        int seed = int.Parse(gameID);
        System.Random randomizer = new System.Random(seed);
        int i = randomizer.Next(0, 1);
        Vector2 v = initTank(players[i], seed, tanksRoot, randomizer,new Vector2(-1f,-1f));
        i = Math.Abs(i - 1);
        initTank(players[i], seed + randomizer.Next(5, 13), tanksRoot, randomizer ,v);

        //tank = GameObject.Find("Tank").GetComponent<TankControl>();
        //tank.SetPlayer(GameCore.self);
    }

    private Vector2 initTank(Player player, int seed, Transform tanksRoot, System.Random randomizer , Vector2 v)
    {
        TankControl tankGO = PrefabManager.InstantiatePrefab("Tank").GetComponent<TankControl>();
        tankGO.transform.SetParent(tanksRoot);
        //tankGO.transform.localPosition = Vector3Extension.FromCoordinate(player.ControlledTank.Position);

        int x = (seed + randomizer.Next(9, 22)) % 100;
        int y = (seed + randomizer.Next(3, 17)) % 100;
        if (!v.x.Equals(-1f) || !v.y.Equals(-1f))
        {
            if ( Math.Abs(x - v.x) < 35) 
            {
                x = (x + randomizer.Next(32, 38)) % 100;
            }

            if (Math.Abs(y - v.y) < 25)
            {
                y = (y + randomizer.Next(22, 28)) % 100;
            }
        }
        tankGO.onKill += onTankKilled;
        tankGO.onHit += onTankHit;
        float height = terrainComp.SampleHeight(new Vector3(x, 0, y));
        tankGO.transform.localPosition = new Vector3(x, height, y);
        tankGO.transform.localScale = Vector3.one * 0.75f;

        tankGO.SetPlayer(player);

        tanks.Add(tankGO);
        tanksHeight.Add(height);
        return  new Vector2(x,y);
    }

    /// <summary>
    /// Create terrain
    /// TODO - should find a way to have tanks on the terrain...
    /// </summary>
    public void CreateTerrain() {

        Transform terrainRoot= new GameObject().transform;

        terrain = PrefabManager.InstantiatePrefab("Terrain");
        terrain.transform.SetParent(terrainRoot);
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
    
    public void onTankKilled(TankControl tank) {
        Gui.ShowEndGame(tank != MyTank).AddListener(()=> {
            ServerWrapper.RemovePlayerFromGame(gameID,PlayerIndex);
            SceneManager.LoadScene("Menus");
        });
    }

    public void onTankHit(TankControl tank)
    {
        Debug.LogError("in onTankHit MainGame");
        if (tank == MyTank)
        {
            Gui.DoOnHitAnimation();
        }
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

    public void OnPollResult(List<PlayerState> result) {
        GameCore.OnPollResult(result);
        //Debug.Log("---------------");
       // Debug.LogFormat("{0} {1}",result[0],result[1]);
    }

}

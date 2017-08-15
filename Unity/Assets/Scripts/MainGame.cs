using System.Collections.Generic;
using ScorchEngine;
using ScorchEngine.Config;
using ScorchEngine.Models;
using ScorchEngine.Server;
using Server;
using UI;
using UnityEngine;
using Utils;

public class MainGame : MonoBehaviour {

    private const bool OFFLINE_MODE = false;
    private const bool VUFORIA = false;

    private TankControl MyTank {
        get {
            return tanks[PlayerIndex];
        }
    }

    public CameraGUI Gui;
    public static ScorchEngine.Game GameCore;
    public static string gameID;
    private List<TankControl> tanks;
    private int PlayerIndex;
    private Transform rootTransform;
    public static GameObject terrain;
    private VuforiaWrapper vuforiaWrapper;

    public static GameObject GetTerrain()
    {
        return terrain;
    }

    void Awake() {
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
                Id = "A",
                Name = "GAME"
            };
            Game.OFFLINE = true;
        }

        gameID = MainUser.Instance.CurrentGame.Id;
        PostLogin(MainUser.Instance.Index);
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
        OverlayControl.Instance.ToggleLoading(false);
        if (VUFORIA)
        {
            vuforiaWrapper.Init();
        }
        else {
            Gui.DisableErrors();
        }
    }

    private void Poll() {
        PlayerState pState = new PlayerState();
        pState.Id = PlayerIndex;
        pState.AngleHorizontal = MyTank.PlayerStats.ControlledTank.AngleHorizontal;
        pState.Force = MyTank.PlayerStats.ControlledTank.Force;
        pState.AngleVertical= MyTank.PlayerStats.ControlledTank.AngleVertical;
        pState.IsReady = MyTank.PlayerStats.ControlledTank.IsReady;
        //Debug.LogFormat(pState.ToString());
        if (OFFLINE_MODE) return;
        UnityServerWrapper.Instance.UpdatePlayerState(MainUser.Instance.CurrentGame.Id, pState, GameCore.OnPollResult);
       // GameCore.Poll(MainUser.Instance.CurrentGame.Id,pState);
        MyTank.PlayerStats.ControlledTank.IsReady = false;
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
        //Add players in game,
        //for each player create a tank, and initialize
        List<Player> players = new List<Player>() {
            Player.CreateMockPlayer(),
            Player.CreateMockPlayer()
        };

        foreach (Player player in players) {
            GameCore.AddPlayer(player);
        }

        for (int i = 0; i < players.Count; i++) {
            Player player = players[i];
            TankControl tankGO = PrefabManager.InstantiatePrefab("Tank").GetComponent<TankControl>();
            tankGO.transform.SetParent(tanksRoot);
            //tankGO.transform.localPosition = Vector3Extension.FromCoordinate(player.ControlledTank.Position);

            int x = i==0? 50:46;
            int y = i==0? 50:46;
            float height = terrain.GetComponentInChildren<Terrain>().SampleHeight(new Vector3(x,0,y));
            tankGO.transform.localPosition = new Vector3(x,height,y);
            tankGO.transform.localScale = Vector3.one*0.5f;

            tankGO.SetPlayer(player);

            tanks.Add(tankGO);
        }

        Debug.Log(tanks.Count);

//tank = GameObject.Find("Tank").GetComponent<TankControl>();
//tank.SetPlayer(GameCore.self);
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
        Gui.ToggleTrackerDetection(detected);
    }

}

using System.Collections.Generic;
using Extensions;
using ScorchEngine;
using ScorchEngine.Config;
using ScorchEngine.Models;
using ScorchEngine.Server;
using UI;
using UnityEngine;
using Utils;

public class MainGame : MonoBehaviour {

    private const bool OFFLINE_MODE = false;

    private TankControl MyTank {
        get {
            return tanks[PlayerIndex];
        }
    }

    List<TankControl> tanks;
    public CameraGUI Gui;
    public static ScorchEngine.Game GameCore;
    private int PlayerIndex;

    void Awake() {
        PrefabManager.Init();

        if (OFFLINE_MODE) {
            MainUser.Instance.Name = "Test";
            MainUser.Instance.Index = 1;
            MainUser.Instance.CurrentGame = new GameInfo{
                Id = "A",
                Name = "GAME"
            };
            Game.OFFLINE = true;
        }

        PostLogin(MainUser.Instance.Index);
    }

    /// <summary>
    /// After login is called start game...
    /// Needs error handling exc...
    /// </summary>
    /// <param name="index"></param>
    public void PostLogin(int index) {
        PlayerIndex = index;
        InitGame();
        InitializePlayers();
        CreateMockTerrain();
        InitializeGUI();

        //after all is set, start polling from server for changes
        InvokeRepeating("Poll", 1, 0.5f);
    }

    private void Poll() {
        PlayerState pState = new PlayerState();
        pState.Id = PlayerIndex;
        pState.AngleHorizontal = MyTank.PlayerStats.ControlledTank.AngleHorizontal;
        pState.Force = MyTank.PlayerStats.ControlledTank.Force;
        pState.AngleVertical= MyTank.PlayerStats.ControlledTank.AngleVertical;
        Debug.LogFormat(pState.ToString());
        if (OFFLINE_MODE) return;
        GameCore.Poll(MainUser.Instance.CurrentGame.Id,pState);
    }

    /// <summary>
    /// According to players given, create for each one a tank, position it and link add it to game
    /// </summary>
    public void InitializePlayers() {
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

        foreach (Player player in players) {
            TankControl tankGO = PrefabManager.InstantiatePrefab("Tank").GetComponent<TankControl>();
            tankGO.transform.position = Vector3Extension.FromCoordinate(player.ControlledTank.Position);
            tankGO.SetPlayer(player);
            tanks.Add(tankGO);
        }
        Debug.Log(tanks);

//tank = GameObject.Find("Tank").GetComponent<TankControl>();
//tank.SetPlayer(GameCore.self);
    }

    /// <summary>
    /// Create mock terrain
    /// TODO - should be replaced with Amitai's terrain object which will derive its topology from the game object
    /// </summary>
    public void CreateMockTerrain() {
        for (int x = 0; x < GameCore.Terrain.SizeX; x++) {
            for (int z = 0; z < GameCore.Terrain.SizeZ; z++) {
                GameObject go = PrefabManager.InstantiatePrefab("Cube");
                go.transform.localPosition = new Vector3(x, 0, z);
            }
        }
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
        Game.debugLog += (string obj) => Debug.LogError(obj);
        GameCore.TurnStarted += OnTurnStarted;

    }

    /// <summary>
    /// Setup gui, link tanks and game to it
    /// </summary>
    public void InitializeGUI() {
        Gui = GameObject.Find("GUI").GetComponent<CameraGUI>();
        TankControl tc = tanks[PlayerIndex];
        tc.LinkToGUI(Gui);
    }

    public void OnTurnStarted(int playerIndex) {
        Debug.LogFormat("Player #{0} turn ", playerIndex);
    }

}

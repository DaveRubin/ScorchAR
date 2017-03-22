using System.Collections.Generic;
using Extensions;
using ScorchEngine;
using ScorchEngine.Config;
using ScorchEngine.Server;
using UI;
using UnityEngine;
using Utils;

public class MainGame : MonoBehaviour {
    List<TankControl> tanks;
    public CameraGUI Gui;
    public static ScorchEngine.Game GameCore;
    private int ID;

    void Awake() {
        string GameID = "SomeGameUID";
        PrefabManager.Init();
        ServerWrapper.Login(GameID,Login);
    }

    /// <summary>
    /// After login is called start game...
    /// Needs error handling exc...
    /// </summary>
    /// <param name="id"></param>
    public void Login(int id) {
        ID = id;
        InitGame();
        InitializePlayers();
        CreateMockTerrain();
        InitializeGUI();
        //after all is set, start polling from server for changes
        InvokeRepeating("Poll",0,0.5f);
    }

    private void Poll() {
        Debug.Log("Polling");
        GameCore.Poll();
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
        GameCore.debugLog += (string obj) => Debug.Log(obj);
        GameCore.TurnStarted += OnTurnStarted;

    }

    /// <summary>
    /// Setup gui, link tanks and game to it
    /// </summary>
    public void InitializeGUI() {
        Gui = GameObject.Find("GUI").GetComponent<CameraGUI>();
        TankControl tc = tanks[ID];
        tc.LinkToGUI(Gui);
    }

    public void OnTurnStarted(int playerIndex) {
        Debug.LogFormat("Player #{0} turn ",playerIndex);
    }

}

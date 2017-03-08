using UI;
using ScorchEngine;
using ScorchEngine.Config;
using UnityEngine;

public class TestControl : MonoBehaviour {
    GameObject cubePrefab;
    TankControl tank;
    public CameraGUI Gui;
    public static ScorchEngine.Game Game;

// Use this for initialization
    void Awake() {
        cubePrefab = Resources.Load<GameObject>("Prefabs/Cube");
        GameConfig conf = new GameConfig();
        conf.Size = new int[]{ 50, 50 };
        conf.MaxPlayers = 1;
        Game = new Game(conf);
        Gui = GameObject.Find("GUI").GetComponent<CameraGUI>();
        AddPlayers();
        for (int x = 0; x < Game.Terrain.SizeX; x++) {
            for (int z = 0; z < Game.Terrain.SizeZ; z++) {
                GameObject go = GameObject.Instantiate(cubePrefab);
                go.transform.localPosition = new Vector3(x, 0, z);
            }
        }

        tank = GameObject.Find("Tank").GetComponent<TankControl>();
        tank.LinkToGUI(Gui);

        Game.debugLog += (string obj) => Debug.Log(obj);
        //game.TurnStarted += OnTurnStart;
        Player koko = new Player("koko");
        koko.SetTank(new Tank());
        Game.AddPlayer(koko);
    }

    void OnTurnStart(int obj) {
        Debug.Log("Turn Started");
    }

    public void AddPlayers() {

    }

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ScorchEngine;
using ScorchEngine.Config;

public class TestControl : MonoBehaviour {
    GameObject cubePrefab;
    Text yVal;
    Text xVal;
    Text forceVal;
    TankControl tank;
    public static ScorchEngine.Game game;

// Use this for initialization
    void Awake() {
        cubePrefab = Resources.Load<GameObject>("Prefabs/Cube");

        GameConfig conf = new GameConfig();
        conf.Size = new int[]{ 50, 50 };
        conf.MaxPlayers = 1;
        game = new Game(conf);

        for (int x = 0; x < game.Terrain.SizeX; x++) {
            for (int z = 0; z < game.Terrain.SizeZ; z++) {
                GameObject go = GameObject.Instantiate(cubePrefab);
                go.transform.localPosition = new Vector3(x, 0, z);
            }
        }
        tank = GameObject.Find("Tank").GetComponent<TankControl>();

        game.debugLog += (string obj) => Debug.Log(obj);
        //game.TurnStarted += OnTurnStart;
        Player koko = new Player("koko");
        koko.SetTank(new Tank());
        game.AddPlayer(koko);

        Slider sliderForce = GameObject.Find("SliderForce").GetComponent<Slider>();
        Slider sliderX = GameObject.Find("SliderX").GetComponent<Slider>();
        Slider sliderY = GameObject.Find("SliderY").GetComponent<Slider>();
        Button fireButton = GameObject.Find("FireButton").GetComponent<Button>();
        yVal = GameObject.Find("AnglesY").GetComponent<Text>();
        xVal = GameObject.Find("AnglesX").GetComponent<Text>();
        forceVal = GameObject.Find("Force").GetComponent<Text>();
        sliderForce.onValueChanged.AddListener(OnForceChange);
        sliderY.onValueChanged.AddListener(OnYChanged);
        sliderX.onValueChanged.AddListener(OnXChanged);
        fireButton.onClick.AddListener(OnFireClicked);
    }

    void OnFireClicked() {
        tank.Shoot();

    }

    void OnTurnStart(int obj) {
        Debug.Log("Turn Started");
    }

    void OnForceChange(float arg0) {
        forceVal.text = arg0.ToString();
        tank.force = arg0;
    }

    void OnYChanged(float arg0) {
        yVal.text = arg0.ToString();
        tank.onUpDownChanged(arg0);
    }

    void OnXChanged(float arg0) {
        xVal.text = arg0.ToString();
        tank.onLeftRightChanged(arg0);
    }

}

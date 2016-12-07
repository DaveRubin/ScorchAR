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
	// Use this for initialization
	void Awake () {
		cubePrefab = Resources.Load<GameObject>("Prefabs/Cube");
		GameConfig conf = new GameConfig();
		conf.Size = new int[]{ 50, 50 };
		conf.MaxPlayers = 1;
		ScorchEngine.Game game = new Game (conf);

		for (int x = 0; x < game.Terrain.SizeX; x++) {
			for (int z = 0; z < game.Terrain.SizeZ; z++) {
				GameObject go = GameObject.Instantiate (cubePrefab);
				go.transform.localPosition = new Vector3 (x, 0, z);
			}
		}
		game.debugLog += (string obj) => Debug.Log(obj);
		//game.TurnStarted += OnTurnStart;
		Player koko = new Player("koko");
		koko.SetTank (new Tank ());
		game.AddPlayer(koko);

		Slider sliderForce = GameObject.Find ("SliderForce").GetComponent<Slider> ();
		Slider sliderX = GameObject.Find ("SliderX").GetComponent<Slider> ();
		Slider sliderY = GameObject.Find ("SliderY").GetComponent<Slider> ();
		yVal = GameObject.Find ("AnglesY").GetComponent<Text> ();
		xVal = GameObject.Find ("AnglesX").GetComponent<Text> ();
		forceVal = GameObject.Find ("Force").GetComponent<Text> ();
		sliderForce.onValueChanged.AddListener (OnForceChange);
		sliderY.onValueChanged.AddListener (OnYChanged);
		sliderX.onValueChanged.AddListener (OnXChanged);
	}

	void OnTurnStart (int obj)
	{
		Debug.Log ("Turn Started");
	}

	void OnForceChange (float arg0)
	{
		forceVal.text = arg0.ToString ();
	}

	void OnYChanged (float arg0)
	{
		yVal.text = arg0.ToString ();
	}

	void OnXChanged (float arg0)
	{
		xVal.text = arg0.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

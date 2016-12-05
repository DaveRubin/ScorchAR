using UnityEngine;
using System.Collections;
using ScorchEngine;
using ScorchEngine.Config;

public class TestControl : MonoBehaviour {
	GameObject cubePrefab;
	// Use this for initialization
	void Start () {
		cubePrefab = Resources.Load<GameObject>("Prefabs/Cube");
		GameConfig conf = new GameConfig();
		conf.Size = new int[]{ 50, 50 };
		ScorchEngine.Game game = new Game (conf);

		for (int x = 0; x < game.Terrain.SizeX; x++) {
			for (int z = 0; z < game.Terrain.SizeZ; z++) {
				GameObject go = GameObject.Instantiate (cubePrefab);
				go.transform.localPosition = new Vector3 (x, 0, z);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

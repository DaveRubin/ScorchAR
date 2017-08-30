using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour {

    public AudioClip BGMusic;
    public AudioSource AudioSource;


	// Use this for initialization
	void Start () {
		AudioSource.clip = BGMusic;
        AudioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

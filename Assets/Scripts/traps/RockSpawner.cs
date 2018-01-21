using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour {
	public GameObject rock;
	public float repeatRate;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("spawnRock", 0f, repeatRate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void spawnRock(){
		Instantiate (rock, transform.position, transform.rotation);
	}



}

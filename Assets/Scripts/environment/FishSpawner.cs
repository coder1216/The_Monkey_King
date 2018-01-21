using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {
	public float repeatRate;
	public GameObject fish;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("spawn", 0, repeatRate);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void spawn(){
		GameObject tmp = Instantiate (fish, transform.position, transform.rotation) as GameObject;

	}
}

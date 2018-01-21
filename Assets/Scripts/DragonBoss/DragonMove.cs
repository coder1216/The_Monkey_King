using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (10f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		//transform.Translate (new Vector3 (10f * Time.deltaTime, 0f, 0f));
	}
}

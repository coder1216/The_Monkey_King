using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals ("PlayerBullet")) {
			Debug.Log ("hit");
			Destroy (col.gameObject);
		}

	}
}

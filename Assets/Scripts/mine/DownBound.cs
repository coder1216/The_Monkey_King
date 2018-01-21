using UnityEngine;
using System.Collections;

public class DownBound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//if the enemies falls, destroy it :)
	void OnTriggerEnter2D (Collider2D col){
		if (col.tag == "Enemy") {
			col.gameObject.GetComponent<MyEnemy> ().Death ();
		}

	}
}

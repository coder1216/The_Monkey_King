using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRockCtrl : MonoBehaviour {
	public bool ignoreWudiMode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag.Equals ("Player")) {
			col.gameObject.GetComponent<MonkeyControl> ().death (ignoreWudiMode);
			Destroy (gameObject);
		}
	}

}

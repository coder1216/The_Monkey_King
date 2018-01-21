using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
	
	private bool activated;
	// Use this for initialization
	void Start () {
		activated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!activated && col.tag.Equals("Player")) {
			activated = true;
		}
	}

	public bool isActivated(){
		return activated;
	}
}

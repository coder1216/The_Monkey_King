using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals("Enemy")){
			col.gameObject.GetComponent<Enemy>().Flip();
		}
	}
}

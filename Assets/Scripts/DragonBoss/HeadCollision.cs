using UnityEngine;
using System.Collections;

public class HeadCollision : MonoBehaviour {

	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Wall") {
			Debug.Log ("hit");
			gameObject.GetComponentInParent<DragonControl> ().stopRush ();
		} else if (col.tag == "Player") {
			player.GetComponent<MonkeyControl> ().death (false);
		}
	}
}

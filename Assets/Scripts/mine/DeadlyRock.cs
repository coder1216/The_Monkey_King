using UnityEngine;
using System.Collections;

public class DeadlyRock: MonoBehaviour {
	public float destroyTime;
	// Use this for initialization
	void Start () {
	
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!col.tag.Equals ("Rock")) {
			Destroy (gameObject);

			if (col.tag.Equals ("Player")) {
				col.gameObject.GetComponent<MonkeyControl> ().hurt ();
			} else if (col.tag.Equals ("Enemy")) {
				//col.gameObject.GetComponent<Enemy> ().hurt ();
			}
		}
	}
}

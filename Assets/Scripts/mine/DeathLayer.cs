using UnityEngine;
using System.Collections;

public class DeathLayer : MonoBehaviour {

	public bool ignoreWudiMode = false;

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player") {
			col.GetComponent<MonkeyControl> ().death (ignoreWudiMode);
			//	playerControl.death ();
		} else{
			if (col.GetComponent<EnemyHealth> () != null) {
				col.GetComponent<EnemyHealth> ().Death ();
			}
		}
	}
}



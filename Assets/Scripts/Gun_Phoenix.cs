using UnityEngine;
using System.Collections;

public class Gun_Phoenix : MonoBehaviour {

	private Enemy_Phoenix enemyPhoenix;		// Reference to the enemy_phoenix script.
	private Vector3 rightPos;
	private Vector3 leftPos;


	void Awake () {
		enemyPhoenix = transform.root.GetComponent<Enemy_Phoenix> ();
		rightPos = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0f);
		leftPos = new Vector3 (-transform.localPosition.x, transform.localPosition.y, 0f);
	}
	

	void FixedUpdate () {
		
		if (enemyPhoenix.facingRight) {
			transform.localPosition = rightPos;
		} else {
			transform.localPosition = leftPos;
		}
	}
}

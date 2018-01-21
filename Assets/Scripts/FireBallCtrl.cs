using UnityEngine;
using System.Collections;

public class FireBallCtrl: MonoBehaviour {
	public float moveSpeed = 1f;

	void Start(){
		Destroy(gameObject, 5);
	}
	void FixedUpdate(){
		transform.Translate (new Vector3 (0, -moveSpeed * Time.deltaTime, 0));
	}

	void OnCollisionEnter2D (Collision2D col)
	{
	}
}

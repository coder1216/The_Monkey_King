using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCtrl : MonoBehaviour {
	public float speed;

	private float randomDegrees;
	private Vector2 biasedRotate;


	// Use this for initialization
	void Start () {
		biasedRotate = new Vector2 (-1f, -1f);
		randomDegrees = Random.Range (-15f, 105f);
		transform.Rotate (new Vector3(0f, 0f, randomDegrees));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3 (speed * Time.deltaTime * biasedRotate.x, speed * Time.deltaTime * biasedRotate.y, 0f));
	}

	Vector2 rotateVector(Vector2 v, float degrees){
		float sin = Mathf.Sin (degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos (degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = cos * tx - sin * ty;
		v.y = sin * tx + cos * ty;
		return v;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMove : MonoBehaviour {
	public float speedLowerBound;
	public float speedUpperBound;

	public float angleLowerBound;
	public float angleUpperBound;

	private float speed;

	// Use this for initialization
	void Start () {
		float angle = Random.Range (angleLowerBound, angleUpperBound);
		speed = Random.Range (speedLowerBound, speedUpperBound);
		float scale = 0.09f / (speedUpperBound - speedLowerBound) * (speed - speedLowerBound) + 0.01f;
		transform.Rotate (new Vector3(0f, 0f, angle));
		transform.localScale = new Vector3 (scale, scale, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){

		transform.Translate (Vector3.right * Time.deltaTime * speed);
	}
}

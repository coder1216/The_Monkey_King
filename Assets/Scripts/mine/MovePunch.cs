using UnityEngine;
using System.Collections;

public class MovePunch : MonoBehaviour {
	public float speed = 20f;
	public float stopTime = 2.0f;
	public int direction;
	public float distance;

	private float bound1;
	private float bound2;

	private bool stopping;
	private float stopStart;
	private Rigidbody2D rb;

	// Use this for initialization
	void Awake () {
		stopStart = Time.time;
		stopping = true;
		rb = GetComponent<Rigidbody2D> ();
		bound1 = transform.position.y;
		bound2 = transform.position.y + distance;
	}
	
	void FixedUpdate(){
		if (stopping) {
			//if exceed stoptime ,start move
			if (Time.time - stopStart > stopTime) {
				stopping = false;
				if (direction == 1) {
					rb.velocity = new Vector2 (-speed, 0);
				} else if (direction == 2) {
					rb.velocity = new Vector2 (speed, 0);
				} else if (direction == 3) {
					rb.velocity = new Vector2 (0, -speed);
				} else {
					rb.velocity = new Vector2 (0, speed);
				}
			}				
		} else {
			if (direction == 1 && transform.position.x <= bound1 && rb.velocity.x < 0) {
				direction = 2;
				Halt ();
			}
			//right and reach the bound
			if (direction == 2 && transform.position.x >= bound2 && rb.velocity.x > 0) {
				direction = 1;
				Halt ();
			}
			//down and reach the bound
			if (direction == 3 && transform.position.y <= bound1 && rb.velocity.y < 0) {
				direction = 4;
				Halt();
			}
			//up and reach the bound
			if (direction == 4 && transform.position.y >= bound2 && rb.velocity.y > 0) {
				direction = 3;
				Halt ();
			}
		}
	}

	void Halt(){
		rb.velocity = new Vector2 (0, 0);
		stopping = true;
		stopStart = Time.time;
	}
}

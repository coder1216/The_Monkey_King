using UnityEngine;
using System.Collections;

public class MovingGround : MonoBehaviour {
	public int direction;

	public float speed;
	public int stayTime;



	private Rigidbody2D rb;
	//the min bound and the max bound
	private Transform bound1;
	private Transform bound2;

	//get the initial position of these bounds 
	private float bound1_x;
	private float bound1_y;
	private float bound2_x;
	private float bound2_y;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		ChangeSpeed ();

		bound1 = transform.GetChild (0);
		bound2 = transform.GetChild (1);

		bound1_x = bound1.position.x;
		bound2_x = bound2.position.x;
		bound1_y = bound1.position.y;
		bound2_y = bound2.position.y;

	}
	
	void FixedUpdate(){
		//left and reach the bound
		if (direction == 1 && transform.position.x <= bound1_x && rb.velocity.x < 0) {
			direction = 2;
			ChangeSpeed ();
		}
		//right and reach the bound
		if (direction == 2 && transform.position.x >= bound2_x && rb.velocity.x > 0) {
			direction = 1;
			ChangeSpeed ();
		}
		//down and reach the bound
		if (direction == 3 && transform.position.y <= bound1_y && rb.velocity.y < 0) {
			direction = 4;
			ChangeSpeed ();
		}
		//up and reach the bound
		if (direction == 4 && transform.position.y >= bound2_y && rb.velocity.y > 0) {
			direction = 3;
			ChangeSpeed ();
		}


	}

	void ChangeSpeed(){
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
}

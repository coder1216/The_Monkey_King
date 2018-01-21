using UnityEngine;
using System.Collections;

public class Enemy_Phoenix : MonoBehaviour {
	public float sightSizeX;
	public float sightSizeY;
	public float moveSpeed;
	public GameObject fireBall;
	public float fireRate = 0.5f;
	public Transform leftFirePos;
	public Transform rightFirePos;
	public float fireBallSpeed = 1f;


	private GameObject player;
	private bool forward;
	private Animator anim;
	private float nextFire = 0f;

	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]



	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		forward = true;
		anim = GetComponent<Animator> ();

	}

	void FixedUpdate(){
		if (withinSight (player))
			startAttack ();
		else
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);
	}

	bool withinSight(GameObject player){
		if (Mathf.Abs (player.transform.position.x - transform.position.x) < sightSizeX
			&& Mathf.Abs (player.transform.position.y - transform.position.y) < sightSizeY)
			return true;
		else
			return false;
	}

	void startAttack(){

		anim.SetFloat ("isLeft", transform.position.x - player.transform.position.x);
		Transform tmp = leftFirePos;
		if (transform.position.x - player.transform.position.x > 0.02) {
			facingRight = false;
			transform.Translate (new Vector3 (-moveSpeed * Time.deltaTime, 0, 0));


		}
		else if (transform.position.x - player.transform.position.x < -0.02){
			facingRight = true;
			transform.Translate (new Vector3 (moveSpeed * Time.deltaTime, 0, 0));
			tmp = rightFirePos;

		}


		if (Time.time > nextFire) {
	//		Transform trans = Instantiate (fireBall, tmp.position, tmp.rotation) as Transform;
			nextFire = Time.time + fireRate;
		}

		//Rigidbody2D bulletInstance = Instantiate(, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
	//	bulletInstance.velocity = new Vector2(speed, 0);



	}


}

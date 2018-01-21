// Use this script to enable any enemy to patrol between two points

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour {
	//enabling patroling
	public float moveSpeed = 2f;		// The speed the enemy moves at.


	public bool collisionDead = false;
	public bool isFiring = false;
	public bool facingRight = true;

	private bool isRushing = false;		// Whether or not the enemy is rushing.
	private float patrolPoint1_x;
	private float patrolPoint2_x;

	void Start(){
		
		patrolPoint1_x = transform.GetChild (0).position.x;
		patrolPoint2_x = transform.GetChild (1).position.x;
	}

	void Awake()
	{
		Physics2D.IgnoreLayerCollision(14, 14);
	}

	void FixedUpdate ()
	{
		if (!facingRight && transform.position.x <= patrolPoint1_x) {
			Flip ();
			Debug.Log ("left");
		}
		//right and reach the bound
		if (facingRight && transform.position.x >= patrolPoint2_x) {
			Debug.Log ("right");
			Flip ();
		}

		// other check
		if (!isFiring) {
			// Set the enemy's velocity to moveSpeed in the x direction.
			GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}
	}



	public void Flip()
	{
		facingRight = !facingRight;
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;

		if (transform.Find ("gun") != null)
		{
			Vector3 gun = transform.Find ("gun").localScale;
			gun.x *= -1;
			transform.Find ("gun").localScale = gun;
		}

		if (transform.Find ("frontCheck") != null)
		{
			Vector3 frontcheck = transform.Find ("frontCheck").localScale;
			frontcheck.x *= -1;
			transform.Find ("frontCheck").localScale = frontcheck;
		}

		if(transform.Find("frontCheckHero") != null)
		{
			Vector3 frontcheckhero = transform.Find ("frontCheckHero").localScale;
			frontcheckhero.x *= -1;
			transform.Find ("frontCheckHero").localScale = frontcheckhero;	
		}
	}




}

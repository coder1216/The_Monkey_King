// General control of enemy

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public bool collisionDead = false;
	public bool isFiring = false;


	private bool isRushing = false;		// Whether or not the enemy is rushing.

	void Awake()
	{
		
		Physics2D.IgnoreLayerCollision(14, 14);
	}



	void FixedUpdate ()
	{
		if (!isFiring) {
			// Set the enemy's velocity to moveSpeed in the x direction.
			GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}


	}
	
	public void Flip()
	{
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



	public void Rush()
	{
		if (!isRushing) {
			isRushing = true;
			moveSpeed = moveSpeed * 2;
		}
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.tag.Equals ("Player") && collisionDead) {
			col.GetComponent<MonkeyControl> ().death (false);
		} else if (col.tag.Equals("Obstacle")){
			Flip ();
		}
	}
}

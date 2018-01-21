// Use this script to enable the enemy automatically chase the player

using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour {


	public bool facingRight = true;

	public float moveSpeed = 2f;		// The speed the enemy moves at.

	public bool collisionDead = false;
	public bool isFiring = false;

	public float respondTime;
	public float sightDistance;

	private bool isRushing = false;		// Whether or not the enemy is rushing.
	private Transform target;//set target from inspector instead of looking in Update

	void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		Physics2D.IgnoreLayerCollision(14, 14);

		InvokeRepeating ("findPlayer", 0.01f, respondTime);
	}
		

	void FixedUpdate ()
	{
		if (withinSight())
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		else
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, GetComponent<Rigidbody2D> ().velocity.y);
	}

	void findPlayer(){
		if ((facingRight && transform.position.x > target.position.x) || (!facingRight && transform.position.x < target.position.x))
			Flip ();
		
	}

	bool withinSight(){
		if (distance (transform.position, target.position) < sightDistance *sightDistance) {
			return true;
		} else
			return false;
	}

	float distance(Vector3 v1, Vector3 v2){
		return (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z);
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


	void OnTriggerEnter2D(Collider2D col){
		
		if (col.tag.Equals ("Player") && collisionDead) {
			col.GetComponent<MonkeyControl> ().death (false);

		} 
	}

}
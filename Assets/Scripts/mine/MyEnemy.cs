using UnityEngine;
using System.Collections;

public class MyEnemy : MonoBehaviour
{
	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged.
	public AudioClip[] deathClips;		// An array of audioclips that can play when the enemy dies.

	public float deathSpinMin = -100f;			// A value to give the minimum amount of Torque when dying
	public float deathSpinMax = 100f;			// A value to give the maximum amount of Torque when dying
	public float sightSizeX = 10;		// the sight of the enmey
	public float sightSizeY = 10;

	private GameObject player; 			//the transform of the player;


	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private bool dead = false;			// Whether or not the enemy is dead.

	private bool forward;

	private GameObject gameCtrl = null;



	void Awake()
	{
		// Setting up the references.
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
		//score = GameObject.Find("Score").GetComponent<Score>();

		player = GameObject.FindGameObjectWithTag ("Player");

		forward = true;

	}



	void FixedUpdate ()
	{
		/*
		// Create an array of all the colliders in front of the enemy.
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		// Check each of the colliders.

		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an Obstacle...
			if(c.tag == "Obstacle")
			{
				// ... Flip the enemy and stop checking the other colliders.
				Flip ();
				break;
			}
		}
		*/

		if (withinSight (player))
			startAttack ();
		else
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);

		// If the enemy has one hit point left and has a damagedEnemy sprite...
		if (HP == 1 && damagedEnemy != null)
			// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
			ren.sprite = damagedEnemy;

		// If the enemy has zero or fewer hit points and isn't dead yet...
		if (HP <= 0 && !dead)
			// ... call the death function.
			Death ();
	}

	void startAttack(){

		if (transform.position.x - player.transform.position.x < 0 && !forward) {
			forward = true;
			transform.Rotate (new Vector3 (0, 180, 0), Space.Self);
		} else if (transform.position.x - player.transform.position.x > 0 && forward){
			forward = false;
			transform.Rotate (new Vector3 (0, 180, 0), Space.Self);
		}
		transform.Translate (new Vector3 (moveSpeed * Time.deltaTime, 0, 0));

	}

	bool withinSight(GameObject player){
		if (Mathf.Abs (player.transform.position.x - transform.position.x) < sightSizeX
			&& Mathf.Abs (player.transform.position.y - transform.position.y) < sightSizeY)
			return true;
		else
			return false;
	}

	public void Hurt()
	{
		// Reduce the number of hit points by one.
		HP--;
	}

	public void Death()
	{
		if (gameCtrl != null) {
			gameCtrl.GetComponent<GameController> ().DecreaseEnemy ();
		}
		Destroy (gameObject);
	}


	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}

	public void SetGameCtrl(GameObject obj){
		gameCtrl = obj;
	}
}

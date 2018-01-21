using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
	public float HP = 2;				// How many times the enemy can be hit before it dies.

	private bool dead = false;			// Whether or not the enemy is dead.
					
	private GameObject gameCtrl = null;
	private SpriteRenderer ren;			// Reference to the sprite renderer.

	public void SetGameCtrl(GameObject obj)
	{
		gameCtrl = obj;
	}

	// Use this for initialization
	void Start () {
		// Setting up the references.
		ren = transform.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// If the enemy has zero or fewer hit points and isn't dead yet...
		if(HP <= 0 && !dead)
		{
			Death ();
		}
	}

	public void Hurt(float damage)
	{
		Debug.Log ("hurt");
		// Reduce the number of hit points by one.
		HP = HP - damage;

	}

	public void Death()
	{
		if (!dead) {
			dead = true;
			Debug.Log ("killed");
			// important
			if (gameCtrl != null) {
				gameCtrl.GetComponent<GameController> ().DecreaseEnemy ();
			}
			// Find all of the sprite renderers on this object and it's children.
			SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer> ();

			// Disable all of them sprite renderers.
			foreach (SpriteRenderer s in otherRenderers) {
				s.enabled = false;
			}

			// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
			ren.enabled = true;

			// Set dead to true.
			dead = true;

			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
			rb.gravityScale = 1.0f;

			// Find all of the colliders on the gameobject and set them all to be triggers.
			Collider2D[] cols = GetComponents<Collider2D> ();
			foreach (Collider2D c in cols) {
				c.isTrigger = true;
			}

			Destroy (gameObject, 1f);
		}
	}


}

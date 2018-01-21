using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
	public Rigidbody2D bullet;				// Prefab of the rocket.
	public float speed = 10f;				// The speed the rocket will fire at.
	public float attackCoolDown = 3.0f;

	public Rigidbody2D body; //reference to the body of enemy
	public Animator enemyAnim; // reference to the enemy animator

	void Start ()
	{
		//StartCoroutine (Fire ());
		InvokeRepeating ("Fire", 0.5f, attackCoolDown);
	}
		
	void Fire()
	{
		if (enemyAnim != null)
			enemyAnim.SetTrigger ("fire");
		if (GetComponent<AudioSource>() != null)
			GetComponent<AudioSource>().Play();

		if (transform.localScale.x > 0) {
			Rigidbody2D bulletInstance = Instantiate (bullet, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2 (speed * Time.deltaTime, 0);
		} else {
			Rigidbody2D bulletInstance = Instantiate (bullet, transform.position, Quaternion.Euler (new Vector3 (0, 0, 180))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2 (speed * Time.deltaTime * -1, 0);
		}
	}
}

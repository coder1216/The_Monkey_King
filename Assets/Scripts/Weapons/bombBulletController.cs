using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombBulletController : MonoBehaviour {
	public float timeToBomb;
	public GameObject explosion;		// Prefab of explosion effect.
	public float bombRadius = 0.3f;
	public float bombDamage;

	private float timer;

	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timer > timeToBomb) {
			bigbang ();
		}
	}

	void bigbang(){
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);

		Destroy (gameObject);

		Collider2D[] objs = Physics2D.OverlapCircleAll (transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));
		foreach (Collider2D col in objs){
			if (col.tag.Equals ("Enemy")) {
				col.gameObject.GetComponent<EnemyHealth> ().Hurt (bombDamage);
			} else if (col.tag.Equals ("DragonBoss")) {
				col.gameObject.GetComponent<DragonControl> ().hurt (bombDamage);
			} else if (col.tag.Equals ("Surprise")) {
				col.gameObject.GetComponent <Surprise> ().hurt (bombDamage);
			}

		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals("Enemy") || col.tag.Equals("DragonBoss")){
			bigbang ();
		}
	}


}

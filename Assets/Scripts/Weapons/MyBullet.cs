using UnityEngine;
using System.Collections;

public class MyBullet : MonoBehaviour {
	public GameObject explosion;		// Prefab of explosion effect.
	public float bulletDamage;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void explode(){
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals ("Enemy")) {
			col.gameObject.GetComponent<EnemyHealth> ().Hurt (bulletDamage);
		} else if (col.tag.Equals ("DragonBoss")) {
			col.gameObject.GetComponent<DragonControl> ().hurt (bulletDamage);
		} else if (col.tag.Equals ("Surprise")) {
			col.gameObject.GetComponent <Surprise> ().hurt (bulletDamage);
		}


		if (!col.tag.Equals ("Obstacle") && !col.tag.Equals("WeaponBox") && !col.tag.Equals("Money") && !col.tag.Equals("EnemyBullet") && !col.tag.Equals("CheckPoint")) {
			explode ();
			Destroy (gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class WheelSpawner : MonoBehaviour {

	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float spawnTime = 1.0f;
	public float speed = 2f;				// The speed the rocket will fire at.
	public Transform parent;




	void Awake()
	{
		// Setting up the references.
		InvokeRepeating("Spawn",  0, spawnTime);
	}



	void Spawn ()
	{
		
		// Instantiate a rocket
		Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, parent.rotation) as Rigidbody2D;
		bulletInstance.velocity = new Vector2 ((transform.position.x - parent.position.x)*speed, (transform.position.y - parent.position.y)*speed);

	}
}

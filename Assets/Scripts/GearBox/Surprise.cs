using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surprise : MonoBehaviour {
	public GameObject[] surpriseList;
	public float health;
	public GameObject explosion;

	private bool isdead = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health < 0 && !isdead) {
			isdead = true;
			explode();
			int idx = (int)(Random.Range(0, surpriseList.Length - 1));
			Instantiate (surpriseList [idx], transform.position, surpriseList [idx].transform.rotation);
			Destroy (gameObject);
		}	
	}

	void explode(){
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}

	public void hurt(float damage){
		health -= damage;
	}
}

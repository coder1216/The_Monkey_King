using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	public Transform playerCheck;
	public float speed;
	public float respondTime;

	private bool startfall;
	private Animator animator;

	// Use this for initialization
	void Start () {
		startfall = false;
		animator = GetComponent<Animator> ();
		Destroy (gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
		if (!startfall) {
			if (Physics2D.Linecast (transform.position, playerCheck.position, 1 << LayerMask.NameToLayer("Player"))) {
				startfall = true;
				//Invoke ("fall", respondTime);
				Invoke ("fall", respondTime);
			}
		}

	}

	void fall(){
		GetComponent<BoxCollider2D> ().enabled = false;
		animator.SetTrigger("Destroy");
		Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length / 3);
	}
}

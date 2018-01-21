using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCtrl : MonoBehaviour {
	public float rotateSpeed;
	public int score = 100;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		transform.Rotate (new Vector3(0f, rotateSpeed * Time.deltaTime,0f));

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals ("Player")) {
			col.gameObject.GetComponent<MonkeyControl> ().updateScore (score);

			Destroy (gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour {
	public int weaponType;
	public int bulletNum;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals ("Player")) {
			GameObject.FindGameObjectWithTag ("Weapon").GetComponent<Weapon> ().switchWeapon (weaponType, bulletNum);
			Destroy (gameObject);
		}
	}
}

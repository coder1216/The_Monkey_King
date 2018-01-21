using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBulletController : MonoBehaviour {
	public float flameDamage;
	public float flameRange;
	public float damageCoolDown;

	private float timer;
	// Use this for initialization
	void Start () {
		timer = Time.time - damageCoolDown;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timer > damageCoolDown) {
			timer = Time.time;

			Vector2 origin;
			Vector2 direction;
			if (transform.rotation.eulerAngles.z < 180.0f) {
				direction = new Vector2 (1.0f, 0.0f);
				origin = new Vector2 (transform.position.x - 0.6f, transform.position.y);
			} else {
				direction = new Vector2 (-1.0f, 0.0f);
				origin = new Vector2 (transform.position.x + 0.6f, transform.position.y);
			}
				
				
			RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, flameRange);

			foreach (RaycastHit2D hit in hits) {
				if (hit.collider.tag.Equals ("Enemy")) {
					hit.collider.gameObject.GetComponent<EnemyHealth> ().Hurt (flameDamage);
				}else if (hit.collider.tag.Equals("Surprise")){
					hit.collider.gameObject.GetComponent<Surprise> ().hurt (flameDamage);
				}
			}
		}
		
	}
		


}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MoveObject : MonoBehaviour {
	public void Move(Vector2 axis) {
		GetComponent<Rigidbody>().AddForce(new Vector3(axis.x, 0, axis.y) * Time.deltaTime * 1000);
	}
}
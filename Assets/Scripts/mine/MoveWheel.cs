using UnityEngine;
using System.Collections;

public class MoveWheel : MonoBehaviour {
	public Transform point;
	public float spinSpeed = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (point.position, new Vector3(0,0,1), Time.deltaTime * spinSpeed);
	}
}

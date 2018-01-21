using UnityEngine;
using System.Collections;

public class TailRotator : MonoBehaviour {

	private float factor = 1.005f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float z = transform.localEulerAngles.z * factor;
		if (transform.GetChild(0) != null)
			transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, z);

	}
}

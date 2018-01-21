using UnityEngine;
using System.Collections;

public class RopeRender : MonoBehaviour {

	public Transform swingPoint;
	public Transform stone;

	private LineRenderer lr;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();
		lr.sortingLayerName = "Foreground";
		lr.sortingOrder = 0;
		lr.SetVertexCount (2);
		lr.SetPosition (0, stone.transform.position);
		lr.SetPosition (1, swingPoint.position);

	}
	
	// Update is called once per frame
	void Update () {
		lr.SetPosition (0, stone.transform.position);
		lr.SetPosition (1, swingPoint.position);
	}
}

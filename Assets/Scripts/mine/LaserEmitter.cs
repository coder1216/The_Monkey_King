using UnityEngine;
using System.Collections;

public class LaserEmitter : MonoBehaviour {
	public float laserWidth = 1.0f;
	public Color color = Color.red;
	public Vector2 direction = new Vector2(1,0);
	public float maxLength = 20f;
	public float speed = 2f;
	public int maxHits = 5;


	private LineRenderer lineRenderer;
	private Vector3 offset;
	private int curHits;

	private Vector3[] positions;

	private float curLength;


	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetWidth (laserWidth, laserWidth);
		offset = new Vector3 (0f, 0f, 0f);
		lineRenderer.sortingLayerName = "Foreground";
		lineRenderer.sortingOrder = 0;
		lineRenderer.SetColors (color, color);
		curLength = 0f;
		positions = new Vector3[maxHits];
	}
	
	// Update is called once per frame
	void FixedUpdate(){
		curLength += Time.deltaTime * speed;
		if (curLength > maxLength)
			curLength = maxLength;
		curHits = 0;
		FindHitPositions (curHits, transform, direction, curLength, GetComponent<BoxCollider2D>());

	}

	void FindHitPositions(int curHits, Transform curTrans, Vector2 direction, float remainLength, Collider2D col){
		if (curHits >= maxHits || remainLength < 0) {
			
			for (int i = 0; i < curHits; i++) {
				Debug.Log (i + " " + positions[i]);
			}
			return;
		}
		RaycastHit2D[] results = new RaycastHit2D[10];
		col.Raycast (direction,results, 10);

		foreach (RaycastHit2D hit in results){
			if (!hit.transform.Equals (curTrans)) {
				positions [curHits] = hit.transform.position;
				Vector2 reflectedDirection = Vector2.Reflect (direction, hit.normal);
				FindHitPositions (curHits + 1, hit.transform, reflectedDirection, remainLength - Vector3.Distance (curTrans.position, hit.transform.position), hit.collider);
				break;
			}
		}

	}
}

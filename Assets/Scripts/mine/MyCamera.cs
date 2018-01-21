using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.

	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.

	public bool fixedPos = false;   // whether the camera is stopped or not
	public bool shake = false;
	public float shakeAmount = 0.01f;


	private Transform player;		// Reference to the player's transform.

	private float xSmooth;		// How smoothly the camera catches up with it's target movement in the x axis.
	private float ySmooth = 2f;		// How smoothly the camera catches up with it's target movement in the y axis.
	private float smoothFactor;	// relate smooth value to chracter's speed

	private bool inPreviewMode; // whether in preview mode
	private Transform previewTarget;
	private float previewStartTime;
	private float previewTime;

	public float biasY = 0.0f;

	// Use this for initialization
	void Start()
	{
		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		float targetaspect = 16.0f / 9.0f;

		// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;

		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{
			Rect rect = camera.rect;

			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;

			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;

			Rect rect = camera.rect;

			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}


	}

	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
		fixedPos = false;
		inPreviewMode = false;
		smoothFactor = 2.0f / 75.0f;
		xSmooth = smoothFactor * player.gameObject.GetComponent<MonkeyControl> ().maxSpeed;
	}

	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}


	void FixedUpdate ()
	{
		//if the camera is not stopped, track the player
		if (!inPreviewMode)	TrackPlayer();
		if (inPreviewMode)
			moveCameraInPreviewMode ();
	}


	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		float fixedX = transform.position.x;

		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y) + biasY;

		// Set the camera's position to the target position with the same z component.
		if (!fixedPos) {
			if (shake) {
				transform.position = new Vector3 (targetX + Random.Range (-1f, 1f) * shakeAmount, targetY + Random.Range (-1f, 1f) * shakeAmount, transform.position.z); 
			} else {
				transform.position = new Vector3 (targetX, targetY, transform.position.z);
			}
		}
		else {
			
			transform.position = new Vector3 (fixedX, targetY, transform.position.z);
		}
		


	}

	public void previewMoving(Transform target, float ptime){
		inPreviewMode = true;
		player.gameObject.GetComponent<MonkeyControl> ().enabled = false;
		previewTarget = target;
		previewStartTime = Time.time;
		previewTime = ptime;
	}

	void moveCameraInPreviewMode (){
		if (Time.time - previewStartTime > previewTime) {
			inPreviewMode = false;
			player.gameObject.GetComponent<MonkeyControl> ().enabled = true;
		}
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		targetX = Mathf.Lerp(transform.position.x, previewTarget.position.x, xSmooth * Time.deltaTime);
		targetY = Mathf.Lerp(transform.position.y, previewTarget.position.y, ySmooth * Time.deltaTime);

		transform.position = new Vector3 (targetX, targetY, transform.position.z);
	}
		
}

using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.


	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.

	public float maxExtraJumpTime;
	public float extraJumpForce;
	public float delayToExtraJumpForce;

	public GameController gameController;



	private Transform groundCheck1;			// A position marking where to check if the player is grounded.
	private Transform groundCheck2;
	private bool grounded = false;			// Whether or not the player is grounded.
	private Rigidbody2D rigidbody2d;

	//private bool jumping = false;
	//private float jumpTimer = 0f;
	private bool move = false;
	private bool jumpButtonClicked = false;

	private bool wudiMode = false;

	void Awake()
	{
		// Setting up references.
		groundCheck1 = transform.Find("groundCheck1");
		groundCheck2 = transform.Find("groundCheck2");
		rigidbody2d = GetComponent<Rigidbody2D> ();
	}


	void Update()
	{
		if (Input.GetButtonDown ("wudi")) {
			wudiMode = !wudiMode;
		}
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"))
			|| Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground"));  

		// If the jump button is pressed and the player is grounded then the player should jump.
		if ((jumpButtonClicked || Input.GetButtonDown("Jump")) && grounded) {
			Debug.Log ("hit");
			jump = true;
			jumpButtonClicked = false;
		//	jumping = true;
		//	jumpTimer = Time.time;
		}
		//if (jumpButtonDown || Time.time - jumpTimer > maxExtraJumpTime) {
		//	jumping = false;
	//	}
			
	}

	public void moveCharacterRight(){
		move = true;
		if (!facingRight)
			Flip ();
	}

	public void moveCharacterLeft(){
		move = true;
		if (facingRight)
			Flip ();
	}
		
	public void stopCharacter(){
		move = false;
	}

	public void jbuttonClick(){
		jumpButtonClicked = true;

	}


	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		// cache the crouch input wen
	//	float c = Input.GetAxis("Crouch");

		if (move && facingRight || h > 0)
			rigidbody2d.velocity = new Vector2 (Vector2.right.x * maxSpeed * Time.deltaTime, rigidbody2d.velocity.y);
		else if (move && !facingRight || h < 0)
			rigidbody2d.velocity = new Vector2 (-Vector2.right.x * maxSpeed * Time.deltaTime, rigidbody2d.velocity.y);


		// If the player should jump...
		if(jump)
		{			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}

		//If our player is holding the jump button and a little bit of time has passed...
//		if (jumping && Time.time - jumpTimer > delayToExtraJumpForce){
//			rigidbody.AddForce(new Vector2(0,extraJumpForce)); //... then add some additional force to the jump
//		}
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	public void death(){
		if (!wudiMode)
			gameController.RebornPlayer ();
	}

	public void death_testMode(){
		transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
	}


}

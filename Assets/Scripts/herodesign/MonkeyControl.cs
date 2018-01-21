using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonkeyControl : MonoBehaviour
{
	
	public bool facingRight = true;			// For determining which way the player is currently facing.
	public float maxSpeed = 5000f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float monkeyHealth = 1.0f;


	public GameController gameController;

	public GameObject immutable_shield;

	public AudioClip jumpClip;
	public AudioClip collectCoinClip;
	public AudioClip deathClip;
	public bool wudiMode = false;


	private Transform groundCheck;
	private Transform groundCheck1;			// A position marking where to check if the player is grounded.
	private Transform groundCheck2;
	private bool grounded = false;			// Whether or not the player is grounded.
	private Rigidbody2D rigidbody2d;
	private Animator animator;
	private bool jump = false;				// Condition for whether the player should jump.
	private bool move = false;
	private bool isDead = false;
	private float curMonkeyHealth;
	private Vector3 healthScale;
	private bool missionOver;
	private bool jumpButtonClicked = false;
	private Vector2 input_axis;
	private BarScript healthBar;
	private int score;
	private Text ui_score;

	private float jumpCD = 0.4f;
	private float lastJumpTime;


	void Awake()
	{
		// Setting up references.
		groundCheck1 = transform.Find("groundCheck1");
		groundCheck2 = transform.Find("groundCheck2");
		rigidbody2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();

		isDead = false;
		curMonkeyHealth = monkeyHealth;
		missionOver = false;
		//healthScale = healthBar.transform.localScale;
		healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarScript>();
		healthBar.MaxValue = monkeyHealth;

		ui_score = GameObject.FindGameObjectWithTag ("UI_Score").GetComponent<Text> ();
	}
	void Update()
	{

		if (Input.GetButtonDown ("wudi")) {
			if (!wudiMode)
				startWudi ();
			else
				stopWudi ();
		}


		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"))
			|| Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground"));  

		// If the jump button is pressed and the player is grounded then the player should jump.
//		if (jumpButtonClicked  && grounded) {
//			Debug.Log ("hit");
//			jump = true;
//			jumpButtonClicked = false;
//			//	jumping = true;
//			//	jumpTimer = Time.time;
//		}
		if((Input.GetButtonDown("Jump") || jumpButtonClicked) && grounded){
			jump = true;
			jumpButtonClicked = false;
			GetComponent<AudioSource> ().clip = jumpClip;
			GetComponent<AudioSource> ().Play ();
		}

		if(Input.GetButtonDown("Death")){
			death (true);
		}

		if (Input.GetButtonDown ("MoveRight")) {
			moveCharacterRight ();
		} else if (Input.GetButtonDown ("MoveLeft")) {
			moveCharacterLeft ();
		}

		if (Input.GetButtonUp ("MoveRight") || Input.GetButtonUp ("MoveLeft")) {
			stopCharacter ();
		}

		if (move) {
			animator.SetBool ("Move", true);
		} else {
			animator.SetBool ("Move", false);
		}
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
		rigidbody2d.velocity = new Vector2 (0.0f, rigidbody2d.velocity.y);
	}

	public void jbuttonClick(){
		if (Time.time - lastJumpTime > jumpCD) {
			lastJumpTime = Time.time;
			jumpButtonClicked = true;
		}

	}

	public void Move(Vector2 axis) {
		if (axis.x > 0.2f)
			moveCharacterRight();
		else if (axis.x < -0.2f)
			moveCharacterLeft ();
		else stopCharacter ();
		
		/*
		if (axis.y > 0.3f && Time.time - lastJumpTime > jumpCD) {
			lastJumpTime = Time.time;
			jbuttonClick ();
		}
		*/
		
	}


	void FixedUpdate ()
	{
		if (!missionOver) {
			if (move && facingRight) {
				rigidbody2d.velocity = new Vector2 (Vector2.right.x * maxSpeed * Time.deltaTime, rigidbody2d.velocity.y);
			} else if (move && !facingRight) {
				rigidbody2d.velocity = new Vector2 (-Vector2.right.x * maxSpeed * Time.deltaTime, rigidbody2d.velocity.y);
			} 

			// If the player should jump...
			if (jump) {		
				animator.SetTrigger ("Jump");
				// Add a vertical force to the player.
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0f, jumpForce));

				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jump = false;
			}


			if (Input.GetButtonDown ("Fire1")) {
			
			} 
		}
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.private Animator animator;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void hurt(){
		if (wudiMode)
			return;
		
		curMonkeyHealth = curMonkeyHealth - 1.0f;
		updateHealth ();
		if (curMonkeyHealth <= 0.0f) {
			death (false);
		}
		
	}

	public void death(bool ignoreWudiMode){
		if ((ignoreWudiMode || !wudiMode) && !isDead) {
			GetComponent<MonkeyControl> ().enabled = false;
			animator.SetTrigger ("Die");
			animator.SetBool ("Dead", true);
			rigidbody2d.velocity = new Vector2 (0.0f, 0.0f);
			//rigidbody2d.simulated = false;
			move = false;
			jump = false;
			grounded = false;
			isDead = true;
			GetComponent<AudioSource> ().clip = deathClip;
			GetComponent<AudioSource> ().Play ();

			gameController.RebornPlayer ();

		}
	}

	public void reset(Vector3 rebornPos){
		isDead = false;
		transform.position = rebornPos;
		rigidbody2d.velocity = new Vector2 (0.0f, 0.0f);
		animator.SetBool("Dead", false);
		curMonkeyHealth = monkeyHealth;
		updateHealth ();
		startWudi ();
		Invoke ("stopWudi", 2.0f);
		GetComponentInChildren<Weapon> ().resetWeapon ();
	}

	public void missionComplete(){
		missionOver = true;
		//cheer
	}

	public void updateHealth(){
		//healthBar.material.color = Color.Lerp (Color.green, Color.red, 1 - curMonkeyHealth * 0.1f);
	//	healthBar.transform.localScale = new Vector3 (healthScale.x * curMonkeyHealth * 0.1f, healthScale.y, 1);
		healthBar.Value = curMonkeyHealth;
	}

	public void updateScore(int s){
		GetComponent<AudioSource> ().clip = collectCoinClip;
		GetComponent<AudioSource> ().Play ();
		score += s;

		ui_score.text = "Score: " + score.ToString ();
	}

	public void startWudi(){
		wudiMode = true;
		immutable_shield.SetActive (true);
	}

	public void stopWudi(){
		wudiMode = false;
		immutable_shield.SetActive (false);

	}

	public int getScore(){
		return score;
	}
}
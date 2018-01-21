using UnityEngine;
using System.Collections;

//attack mode:
// 0: noraml 30%
// 1: rush   30%
// 2: roar   30%
// 3: ulti   10%


public class DragonControl : MonoBehaviour {
	public float rushSpeed;		// speed of the rush
	public float roarTime = 2.0f;	// duration time for roar

	public GameObject headLaserEmitter;	// the laser emitter of the head
	public GameObject headLaserPartical;

	public float attackCoolDown = 3.0f;	// the cooldown between different modes
	public float ultiStandBy = 1.0f;
	public GameObject shield;
	public float dragonHealth = 10.0f;

	public GameObject explosion;

	public GameObject rock;
	public GameObject weaponBox;

	public Transform ground;

	public bool test;

	public float timer;	// the timer of the overall battle
	public Transform rockSpawnPos;

	public LaserBeam lb;
	private float ultiStandByTimer;

	private Animator anim;	// the animator controls the dragon boss
	private Rigidbody2D rb;
	private int mode;		// attack mode
	private bool attacking;	// is the dragon attacking
	private Transform player;	// the transform of the player
	private bool facingRight;	// is the dragon facing right or left
	private float startRoar;	// the timer for the dragon when roaring
	private MyCamera myCamera;

	private LineRenderer headLR;	// the lineRenderer of the head laser

	private float angle;	// controls the head laser
	private GameObject tempShield;
	private Coroutine rockCoroutine = null;
	private bool dead;

	private float[] attack_modes;
	private float laser_mode = 40f;
	private float rush_mode = 30f;
	private float roar_mode = 30f;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		timer = Time.time;
		attacking = false;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		myCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MyCamera>();
		facingRight = false;

		headLR = headLaserEmitter.GetComponent<LineRenderer> ();

		gameObject.GetComponent<DragonControl> ().enabled = test;
		dead = false;
		attack_modes = new float[3];
		attack_modes [0] = laser_mode;
		attack_modes [1] = rush_mode;
		attack_modes [2] = roar_mode;

	}

	// Update is called once per frame
	void Update () {
		// control the dragon to always face the player
		if (!dead) {
			if (!attacking) {
				if (player.transform.position.x < transform.position.x - 1.0f) {
					if (facingRight)
						Flip ();
				} else if (player.transform.position.x > transform.position.x + 1.0f) {
					if (!facingRight)
						Flip ();
				}
			}

			//if not attacking and not in the cooldown
			if (!attacking && Time.time - timer > attackCoolDown) {
				// randomly select the mod
				mode = getAttackMode();

				// normal attack
				if (mode == 0) {
					attacking = true;
					ultiStandByTimer = Time.time;
					ultiReady ();
					}
				// rush mode
				else if (mode == 1) {
						rush ();
						attacking = true;
					}
				// roar mode
				else if (mode == 2) {
						roar ();
						attacking = true;
					}
			} else if (attacking) {
					// if in roar mode and exceeds the roar time limitation, stop the roar mode;
					if (mode == 2 && Time.time - startRoar > roarTime) {
						stopRoar ();
					} 
				// if in the laser mode and exceeds the standby time, start emit laser
				else if (mode == 0 && ultiStandByTimer > 0 && Time.time - ultiStandByTimer > ultiStandBy) {
						ultiStandByTimer = -ultiStandByTimer;
						ulti ();
				}
			}
		}
	}


	void FixedUpdate(){
		if (attacking) {
			
		}


		if (dead) {
			Quaternion randomRotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));

			// Instantiate the explosion where the rocket is with the random rotation.
			float scale = 1f;
			Vector3 rndPos = new Vector3 (transform.position.x + Random.Range (-1f, 1f) * scale, transform.position.y + Random.Range (-1f, 1f) * scale, 0);
			Instantiate (explosion, rndPos , randomRotation);
		}
	}

	int getAttackMode(){
		float sum = 0;
		for (int i = 0; i < 3; i++) {
			sum += attack_modes [i];
		}

		int ret = 0;
		float rnd = Random.Range (0f,sum);
		if (rnd < attack_modes [0]) {
			ret = 0;
			attack_modes [1] *= 2;
			attack_modes [2] *= 2;
			attack_modes [0] = laser_mode;
		} else if (rnd < attack_modes [0] + attack_modes [1]) {
			ret = 1;
			attack_modes [0] *= 2;
			attack_modes [2] *= 2;
			attack_modes [1] = rush_mode;
		} else {
			ret = 2;
			attack_modes [0] *= 2;
			attack_modes [1] *= 2;
			attack_modes [2] = roar_mode;
		}
		return ret;
	}

	// stop the roar mode, stop the animation and reset timer, stop camera shake
	void stopRoar(){
		attacking = false;
		anim.SetBool ("Roar", false);
		timer = Time.time;
		myCamera.shake = false;

		GetComponent<AudioSource> ().Stop ();

		if (rockCoroutine != null) StopCoroutine (rockCoroutine);
	}

	// stop the rush mode, reset the dragon's speed, reset timer
	public void stopRush(){
		attacking = false;
		anim.SetBool ("Rush", false);
		rb.velocity = new Vector2 (0.0f, 0.0f);
		timer = Time.time;
	}

	// stop the laser mode, disable the particle system, destroy the shield
	public void stopUlti(){
		attacking = false;
		anim.SetBool ("Ulti", false);
		timer = Time.time;

		if (tempShield != null)
			Destroy (tempShield);
	}

	void roar(){
		anim.SetBool ("Roar", true);
		startRoar = Time.time;
		myCamera.shake = true;
		//stop dropping rocks
		rockCoroutine =  StartCoroutine (SpawnRocks ());

		GetComponent<AudioSource> ().Play ();

	}

	IEnumerator SpawnRocks ()
	{
		yield return new WaitForSeconds (0);
		while (true)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector3 spawnPosition = new Vector3 (Random.Range (rockSpawnPos.position.x, rockSpawnPos.position.x + 6f), rockSpawnPos.position.y, 0f);
				float tmp = Random.Range (0f, 1f);
				if (tmp < 0.93f)
					Instantiate (rock, spawnPosition, rock.transform.rotation);
				else
					Instantiate (weaponBox, spawnPosition, rock.transform.rotation);
				yield return new WaitForSeconds (0.2f);
			}
			yield return new WaitForSeconds (1f);
		}
	}



	// in the ready stage, instantiate a shield for player to hide
	void ultiReady(){
		anim.SetBool ("Ulti", true);

		Invoke ("produceShield", 1f);

	}
	void ulti(){
		lb.FireLaser ();
	}

	void produceShield(){
		Vector3 shieldPos = new Vector3 ();
		shieldPos.x = player.position.x + Random.Range (-0.3f, 0.3f);
		shieldPos.y = ground.position.y + 1.3f;
		shieldPos.z = 0.0f;
		tempShield = Instantiate (shield, shieldPos, shield.transform.rotation) as GameObject;
	}

	void rush(){
		anim.SetBool ("Rush", true);
		// rush to the player
		if (facingRight) rb.velocity = new Vector2 (transform.right.x * rushSpeed * Time.deltaTime, rb.velocity.y);
		else rb.velocity = new Vector2 (-transform.right.x * rushSpeed * Time.deltaTime, rb.velocity.y);
		//Debug.Log (rb.velocity);
	//	rb.AddForce(new Vector2(10000f, 0f));
	}

	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
		facingRight = !facingRight;
		lb.setFacing (facingRight);

	}

	public void hurt(float damage){
		dragonHealth = dragonHealth - damage;

		if (dragonHealth < 0.0f) {
			death ();

		}
	}

	private void death(){
		
		dead = true;
		stopRoar ();
		stopRush ();
		stopUlti ();
		attacking = false;


		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().missionComplete ();
	}
}

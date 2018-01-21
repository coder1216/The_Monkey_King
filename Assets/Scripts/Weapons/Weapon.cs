using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
	
	public float speed = 20f;				// The speed the rocket will fire at.
	public Rigidbody2D normalBullet;
	public Rigidbody2D superBullet;
	public Rigidbody2D bombBullet;
	public Rigidbody2D flameBullet;
	public AudioClip normalBulletAudio;
	public AudioClip superBulletAudio;
	public AudioClip bombBulletAudio;
	public AudioClip flameBulletAudio;
	public AudioClip changeWeapon;

	public float normalFireCoolDown;
	public float superFireCoolDown;


	public Vector2 bombForce;
	public GameObject[] ui_weapons;

	private MonkeyControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	private bool fireButtonClicked = false;
	private bool fireButtonClickedDown = false;
	private Rigidbody2D bullet;				// Prefab of the rocket.
	// 0: normal, 1: super, 2:bomb, 3:flame
	private int weaponMode;
	private bool isHoldFire = false;
	private float lastFireTime = 0.0f;
	private Rigidbody2D currentFlameBullet;
	private AudioClip curAudio;

	private int curBulletNum = 0;
	private Text ui_weapon_num;
	private string infiniteStr = "∞";


	void Awake()
	{
		// Setting up the references.
		anim = transform.parent.gameObject.GetComponent<Animator>();
		playerCtrl = transform.parent.GetComponent<MonkeyControl> ();
		ui_weapon_num = GameObject.FindGameObjectWithTag ("UI_Weapon_num").GetComponent<Text> ();

		initWeapon ();
	}

	public void initWeapon(){
		curBulletNum = 0;
		weaponMode = 0;
		curAudio = normalBulletAudio;
		switchWeapon (weaponMode, 20);
		ui_weapon_num.text = "x " + infiniteStr;
		ui_weapons [0].SetActive (true);
	}

	public void pressFire(){
		fireButtonClicked = true;
	}

	public void holdFire(){
		isHoldFire = true;
		fireButtonClickedDown = true;
	}

	public void releaseFire(){
		isHoldFire = false;
	}

	public void switchWeapon(int mode, int bullets){
		GetComponent<AudioSource> ().clip = changeWeapon;
		GetComponent<AudioSource> ().Play ();

		ui_weapons [weaponMode].SetActive (false);
		ui_weapons [mode].SetActive (true);
		if (weaponMode == mode) {
			curBulletNum += bullets;
			return;
		} else
			curBulletNum = bullets;

		weaponMode = mode;
		if (mode == 0) {
			curAudio = normalBulletAudio;
			ui_weapon_num.text = "x " + infiniteStr;

		} else if (mode == 1) {
			curAudio = superBulletAudio;
			ui_weapon_num.text = "x " + bullets.ToString();
		} else if (mode == 2) {
			curAudio = bombBulletAudio;
			ui_weapon_num.text = "x " + bullets.ToString();
		} else if (mode == 3) {
			curAudio = flameBulletAudio;
			ui_weapon_num.text = "x " + infiniteStr;
		}
	}

	void Update ()
	{
		if (curBulletNum <= 0) {
			switchWeapon (0, 20);
			return;
		}

		if (Input.GetButtonDown ("SwitchWeapon")) {
			switchWeapon((weaponMode + 1) % 4, 20);
		}


		// in normal mode
		if (weaponMode == 0) {
			bullet = normalBullet;
			// If the fire button is pressed...
			if (Time.time - lastFireTime > normalFireCoolDown && (Input.GetButtonDown ("Fire1") || fireButtonClicked)) {
				// ... set the animator Shoot trigger parameter and play the audioclip.
				fireButtonClicked = false;
				anim.SetTrigger ("Shoot");
				GetComponent<AudioSource> ().clip = curAudio;
				GetComponent<AudioSource> ().Play ();
				--curBulletNum;

				// If the player is facing right...
				if (playerCtrl.facingRight) {
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D bulletInstance = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2 (speed, 0);
				} else {
					// Otherwise instantiate the rocket facing left and set it's velocity to the left.
					Rigidbody2D bulletInstance = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 180f))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2 (-speed, 0);
				}

				lastFireTime = Time.time;
			}
		}
		// in super mode
		else if (weaponMode == 1) {
			bullet = superBullet;
			if (Input.GetButton ("Fire1") || isHoldFire) {
				if (Time.time - lastFireTime > superFireCoolDown) {
					anim.SetTrigger ("Shoot");
					GetComponent<AudioSource> ().clip = curAudio;
					GetComponent<AudioSource> ().Play ();
					--curBulletNum;
					ui_weapon_num.text = "x " + curBulletNum.ToString();
					// If the player is facing right...
					if (playerCtrl.facingRight) {
						// ... instantiate the rocket facing right and set it's velocity to the right. 
						Rigidbody2D bulletInstance = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
						bulletInstance.velocity = new Vector2 (speed, 0);
					} else {
						// Otherwise instantiate the rocket facing left and set it's velocity to the left.
						Rigidbody2D bulletInstance = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 180f))) as Rigidbody2D;
						bulletInstance.velocity = new Vector2 (-speed, 0);
					}

					lastFireTime = Time.time;
				}

			}
		}
		// in bomb mode
		else if (weaponMode == 2) {
			bullet = bombBullet;
			if (Input.GetButtonDown ("Fire1") || fireButtonClicked) {
				// ... set the animator Shoot trigger parameter and play the audioclip.
				fireButtonClicked = false;
				anim.SetTrigger ("Shoot");
				GetComponent<AudioSource> ().clip = curAudio;
				GetComponent<AudioSource> ().Play ();
				--curBulletNum;
				ui_weapon_num.text = "x " + curBulletNum.ToString();
				// If the player is facing right...
				if (playerCtrl.facingRight) {
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D bulletInstance = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
					bulletInstance.AddForce (new Vector2(bombForce.x, bombForce.y));
				} else {
					// Otherwise instantiate the rocket facing left and set it's velocity to the left.
					Rigidbody2D bulletInstance = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 180f))) as Rigidbody2D;
					bulletInstance.AddForce (new Vector2(-bombForce.x, bombForce.y));
				}
			}
		}
		// in flame mode
		else if (weaponMode == 3) {
			bullet = flameBullet;
			// If the fire button is pressed...
			if (Input.GetButtonDown ("Fire1") || fireButtonClickedDown) {
				
				// ... set the animator Shoot trigger parameter and play the audioclip.
				fireButtonClickedDown = false;

				currentFlameBullet = Instantiate (bullet, transform.GetChild (0).position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;

				GetComponent<AudioSource> ().clip = curAudio;
				GetComponent<AudioSource> ().Play ();

			}
			if (!Input.GetButton ("Fire1") && !isHoldFire) {
				
				if (currentFlameBullet != null) {
					Destroy (currentFlameBullet.gameObject);
					currentFlameBullet = null;
				}
			} else {
				Vector3 targetPos = gameObject.transform.position;
			
				if (playerCtrl.facingRight) {
					currentFlameBullet.transform.position = new Vector3 (targetPos.x + 0.6f, targetPos.y, targetPos.z);
					currentFlameBullet.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
				} else {
					currentFlameBullet.transform.position = new Vector3 (targetPos.x - 0.6f, targetPos.y, targetPos.z);
					currentFlameBullet.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
				}
			}
		}

	}

	public void resetWeapon(){
		switchWeapon (0, 100);
	}
}


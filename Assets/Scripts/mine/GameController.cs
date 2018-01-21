using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject camera;				// get the MainCamera
	public Transform playerPos;				// get the player's transform

	public Transform[] checkPoints;			// get the transforms of all checkPoints
	public Transform[] rebornPoints;		// get the transforms of the rebornPoints
	public Transform[] previewPoints;

	public GameObject wall;					// the "PREFAB" wall which will be instantiated when player arrives the checkPoint
	public GameObject missionCompleteTitle;		// mission complete title
	public GameObject UI_gameOver;
	public GameObject rebornDialog;
	public Text ui_life_reborn;


	public Transform bossCheck;

	public float playerLifes = 6;
	public float totalScore = 2900;


	private bool[] checkPointPass;			// decide whether checkPoints are passed
	private int curCP;						// current checkPoint
	private int enemySurvivedNum;			// how many enemies still alived
	private GameObject invisWall;			// the instantiation of the wall
	private float cameraWidth;				// the width size of the camera
	private bool inCheckPoint;				// whether the player in a checkPoint event

	private int pendingRebornPoint;
	private int pendingPreviewPoint;
	private bool inBossFight;

	private Text ui_life;
	private Text ui_time;
	private float used_time;

	private bool needReborn = false;
	private float dieTime;
	private float curPlayerLifes;

	// Use this for initialization
	void Start () {
		int n = checkPoints.Length;
		checkPointPass = new bool[n];		
		curCP = 0;

		inCheckPoint = false;
		enemySurvivedNum = 0;
		pendingRebornPoint = 0;
		pendingPreviewPoint = 0;
		inBossFight = false;
		ui_life = GameObject.FindGameObjectWithTag ("UI_Life").GetComponent<Text> ();
		ui_life.text = "x " +playerLifes.ToString();
		ui_time = GameObject.FindGameObjectWithTag ("UI_Time").GetComponent<Text> ();
		ui_time.text = "time: 00:00";
		used_time = 0f;
		curPlayerLifes = playerLifes;
	}
		
	// Update is called once per frame
	void Update () {
		updateTimer ();

		// if a player is in a checkPoint and find that all enemies are destroyed
		// current checkPoint pass
		if (inCheckPoint && enemySurvivedNum == 0) {
			checkPointPass [curCP] = true;
			inCheckPoint = false;
		}

		// if there are remaining checkPoints
		if (curCP < checkPointPass.Length) {
			// if the player arrives the checkPoint and hasn't passed the checkPoint
			if (checkPoints [curCP].GetComponent<CheckPoint>().isActivated() && !checkPointPass [curCP]) {
				// at first, set the enemyNum to be destroyed and stop the camera
				if (!inCheckPoint) {
					GetEnemyNum ();
					StopCameraMoving ();
				}
			}
			// if the player arrives the checkPoint and has passed the checkPoint
			else if (checkPoints [curCP].GetComponent<CheckPoint>().isActivated() && checkPointPass [curCP]) {
				// make camera move
				ResumeCameraMoving ();
				// set current checkPoint to the next
				++curCP;
				// all checkPoints passed means the mission completed
				if (curCP >= checkPointPass.Length) {
					//MissionComplete ();
				}
			}
		}

		if (pendingRebornPoint < rebornPoints.Length && rebornPoints [pendingRebornPoint].GetComponent<CheckPoint>().isActivated()) {
			++pendingRebornPoint;
		}

		if (!inCheckPoint && pendingPreviewPoint < previewPoints.Length && previewPoints [pendingPreviewPoint].GetComponent<CheckPoint>().isActivated()) {
			camera.GetComponent<MyCamera> ().previewMoving (previewPoints [pendingPreviewPoint].GetChild (0), 3.0f);
			++pendingPreviewPoint;
		}

		if (!inBossFight && bossCheck.GetComponent<CheckPoint>().isActivated()) {
			GameObject boss = GameObject.FindGameObjectWithTag ("DragonBoss");
			boss.GetComponent<DragonControl> ().enabled = true;
			boss.GetComponent<DragonControl> ().timer = Time.time;
			playerPos.gameObject.GetComponent<MonkeyControl> ().jumpForce = 250f;
			inBossFight = true;
		}

	}

	private void updateTimer(){
		used_time += Time.deltaTime;
		var minutes = used_time / 60;
		var seconds = used_time % 60;

		ui_time.text = string.Format ("Time: {0:00}:{1:00}", minutes, seconds);
	}

	public void disablePlayer(){
		playerPos.gameObject.GetComponent<MonkeyControl> ().enabled = false;
	}

	public void enablePlayer(){
		playerPos.gameObject.GetComponent<MonkeyControl> ().enabled = true;
	}


	public void missionComplete(){
		// mission complete, disable all the control script
		playerPos.gameObject.GetComponent<MonkeyControl> ().enabled = false;
		playerPos.gameObject.GetComponentInChildren<Weapon> ().enabled = false;

		missionCompleteTitle.SetActive (true);
		GetComponent<AudioSource> ().Play ();


		missionCompleteTitle.GetComponent<CompleteMenu> ().setStars (calculateStars ());
		missionCompleteTitle.GetComponent<CompleteMenu> ().setScore ();
		// set the text
		//missionComplete.GetComponent<Text> ().text = "MissionComplete!";
	}

	private int calculateStars(){
		int curScore = playerPos.gameObject.GetComponent<MonkeyControl> ().getScore ();
		float res = (curScore * 1.0f / totalScore + curPlayerLifes * 1.0f / playerLifes) / 2;
		if (res < 0.4f) {
			return 1;
		} else if (res < 0.7f) {
			return 2;
		} else {
			return 3;
		}
	}

	private void SetInvisWall(){
		Debug.Log ("set wall");
		cameraWidth = camera.GetComponent<Camera> ().aspect * camera.GetComponent<Camera> ().orthographicSize;

		// the the position of this invisWall
		Vector3 tmp = new Vector3 (camera.transform.position.x + cameraWidth, 0, 0);
		// instantiate the invisWall
		invisWall = Instantiate (wall, tmp, wall.transform.rotation) as GameObject;

	}

	// get the enemyNum to be destroyed
	private void GetEnemyNum(){
		// now player is in the checkPoint event
		inCheckPoint = true;
		// get all the spawners in this checkPoint
		MySpawner[] list = checkPoints [curCP].GetComponentsInChildren<MySpawner> ();
		// sum the enemies up
		foreach (MySpawner sp in list){
			sp.StartSpawn ();
			enemySurvivedNum += sp.numToSpawn;
			sp.transform.GetChild (0).gameObject.SetActive (true);
		}

	}

	private void stopEnemySpawn(){
		if (inCheckPoint) {
			MySpawner[] list = checkPoints [curCP].GetComponentsInChildren<MySpawner> ();
			// sum the enemies up
			foreach (MySpawner sp in list){
				sp.stopSpawn ();
			}
		}
	}

	private void resumeEnemySpawn(){
		if (inCheckPoint) {
			MySpawner[] list = checkPoints [curCP].GetComponentsInChildren<MySpawner> ();
			// sum the enemies up
			foreach (MySpawner sp in list){
				sp.resumeSpawn ();

			}
		}
	}

	// not used yet
	public void IncreaseEnemy(){
		++enemySurvivedNum;

	}

	// if enemy destroyed, decrease the enemyNum
	public void DecreaseEnemy(){
		--enemySurvivedNum;
	}

	// make the camera move
	void ResumeCameraMoving(){
		// destroy the invisWall
		if (invisWall != null) {
			Debug.Log ("destroy wall");
			Destroy (invisWall);
			invisWall = null;
		}
		Debug.Log ("camera resume");
		camera.GetComponent<MyCamera> ().fixedPos = false;
	}

	// set the invisWall and make the camera stop
	void StopCameraMoving(){
		SetInvisWall ();
		Debug.Log ("camera stop");
		camera.GetComponent<MyCamera> ().fixedPos = true;
	}

	public void RebornPlayer(){
		stopEnemySpawn ();
		curPlayerLifes--;
		if (curPlayerLifes <= 0) {
			UI_gameOver.SetActive (true);

		} else {
			ui_life.text = "x " + curPlayerLifes.ToString ();

			rebornDialog.SetActive (true);

			ui_life_reborn.text = "x  " + curPlayerLifes.ToString ();
			Invoke ("StartReborn", 1f);
		}


	}

	public void StartReborn(){
		rebornDialog.SetActive (false);
		MonkeyControl mc = playerPos.gameObject.GetComponent<MonkeyControl> ();
		mc.enabled = true;
		mc.reset (rebornPoints [pendingRebornPoint - 1].position);
		rebornDialog.SetActive (false);
		resumeEnemySpawn ();
	}

	public void backToMissionSelect(){
		SceneManager.LoadScene ("LevelSelect");
	}





}

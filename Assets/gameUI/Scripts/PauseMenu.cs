using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	public int currentScene;

	public int mainMenu;

	public bool isPaused;

	public int levelSelect;

	public GameObject pauseMenuCanvas;

	public Button pauseBtn;

//	void Start() {
//		pauseMenuCanvas.SetActive (true);
//		Time.timeScale = 0f;
//	}

	// Update is called once per frame
	void Update () {
		if (isPaused) {
			pauseMenuCanvas.SetActive (true);
			Time.timeScale = 0f;
		} else {
			pauseMenuCanvas.SetActive (false);
			Time.timeScale = 1f;
		}

		pauseBtn.onClick.AddListener (TurnIsPaused);
			
	}

	public void Resume() {
		isPaused = false;
//		pauseMenuCanvas.SetActive(false);
//		Time.timeScale = 1f;
	}

	public void Restart() {
		SceneManager.LoadScene (currentScene);
	}

	public void MainMenu() {
		SceneManager.LoadScene (mainMenu);
	}

	public void LevelSelect(){
		SceneManager.LoadScene (levelSelect);
	}

	public void NextLevel(int nextLevel){
		SceneManager.LoadScene (nextLevel);
	}

	void TurnIsPaused() {
		isPaused = true;
	}
}

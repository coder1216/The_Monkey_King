using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {

	[System.Serializable]
	public class Level
	{
		public string LevelText;
		public int UnLocked;
		public bool IsInteractable;

//		public Button.ButtonClickedEvent OnClickEvent;
	}

	public GameObject levelButton;
	public Transform Spacer;
	public List<Level> LevelList;

	// Use this for initialization
	void Start () {
//		DeleteAll ();
		FillList ();
	}

	void FillList()
	{
		foreach (var level in LevelList) 
		{
			GameObject newbutton = Instantiate (levelButton) as GameObject;
			LevelButton button = newbutton.GetComponent<LevelButton> ();
			button.LevelText.text = level.LevelText;

			if (PlayerPrefs.GetInt ("Level" + button.LevelText.text) == 1) {
				level.UnLocked = 1;
				level.IsInteractable = true;
			}

			button.unlocked = level.UnLocked;
			button.GetComponent<Button> ().interactable = level.IsInteractable;
			button.GetComponent<Button> ().onClick.AddListener (() => LoadLevel ("Level" + button.LevelText.text));

			if (PlayerPrefs.GetInt ("Level" + button.LevelText.text + "_score") > 0) {
				button.StarBG1.SetActive (false);
				button.Star1.SetActive (true);
			}
			if (PlayerPrefs.GetInt ("Level" + button.LevelText.text + "_score") >= 5000) {
				button.StarBG2.SetActive (false);
				button.Star2.SetActive (true);
			}
			if (PlayerPrefs.GetInt ("Level" + button.LevelText.text + "_score") >= 10000) {
				button.StarBG3.SetActive (false);
				button.Star3.SetActive (true);
			}

			newbutton.transform.SetParent (Spacer, false);
		}

		SaveAll ();
	}

	void SaveAll() {
//		if(PlayerPrefs.HasKey("Level1"))
//		{
//			return;
//		}

		GameObject[] allButtons = GameObject.FindGameObjectsWithTag ("LevelButton");
		foreach (GameObject buttons in allButtons) {
			LevelButton button = buttons.GetComponent<LevelButton> ();
			PlayerPrefs.SetInt ("Level" + button.LevelText.text, button.unlocked);
		}
	}

	void LoadLevel(string LevelName) {
		SceneManager.LoadScene (LevelName);
	}

	void DeleteAll() {
		PlayerPrefs.DeleteAll ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour {

	int score = 5000;
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("Level2", 1);
		PlayerPrefs.SetInt ("Level1_score", score);
		StartCoroutine (Time ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Time()
	{
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("LevelSelect");
	}
}

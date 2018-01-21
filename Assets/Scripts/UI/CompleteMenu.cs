using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteMenu : MonoBehaviour {

	public GameObject star1;
	public GameObject star2;
	public GameObject star3;

	public Text ui_score;
	private int score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setStars(int num){
		if (num >= 1)
			star1.SetActive (true);
		if (num >= 2)
			star2.SetActive (true);
		if (num >= 3)
			star3.SetActive (true);
	}

	public void setScore(){
		score = GameObject.FindGameObjectWithTag ("Player").GetComponent<MonkeyControl> ().getScore ();


		StartCoroutine (SpawnScores ());
	}

	IEnumerator SpawnScores ()
	{
		yield return new WaitForSeconds (0);
		for (int i = 0; i <= score; i += 100){
			ui_score.text = i.ToString ();
			GetComponent<AudioSource> ().Play ();
			yield return new WaitForSeconds (0.2f);
		}
	//	GetComponent<AudioSource> ().Stop ();
	}
}

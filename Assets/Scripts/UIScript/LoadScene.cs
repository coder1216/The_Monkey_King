using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public GameObject loadingImage;

	void Start(){
		
	}
	public void SceneLoad(int level){
		loadingImage.SetActive (true);
		SceneManager.LoadScene (level);
	}
}

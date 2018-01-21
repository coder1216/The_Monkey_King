using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionSelect : MonoBehaviour {

	public GameObject loadingImage;
	public GameObject panel;

	private GameObject curObj = null;

	void Start(){
		curObj = null;
	}
	public void SceneLoad(int level){
		loadingImage.SetActive (true);
		SceneManager.LoadScene (level);
	}

	public void RegionSelected(GameObject obj){
		panel.SetActive (true);
		curObj = obj;
	}

	public void CancelSelection(){
		panel.SetActive (false);
		if (curObj != null) {
			curObj.GetComponent<Image> ().color = Color.white;
		}
	}
}

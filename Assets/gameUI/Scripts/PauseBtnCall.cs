using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtnCall : MonoBehaviour {
	public GameObject pauseMenuCanvas;

	public void CallPause() {
		pauseMenuCanvas.SetActive (true);
	}

}

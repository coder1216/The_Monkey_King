using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageSelected : MonoBehaviour {

	public void ChangeColor(){
	    Image img = GetComponent<Image> ();
		img.color = Color.red;
	}
}

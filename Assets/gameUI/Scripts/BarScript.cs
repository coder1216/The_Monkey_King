using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField]
	private float fillAmount;

	[SerializeField]
	private Image content;

	[SerializeField]
	private Text valueText;

	[SerializeField]
	private Color fullColor;

	[SerializeField]
	private Color midColor;

	[SerializeField]
	private Color lowColor;

	public float MaxValue {
		get;
		set;
	}

	public float Value
	{
		set
		{ 
			valueText.text = "HP: " + value;
			fillAmount = Map (value, 0, MaxValue, 0, 1);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		HandleBar();
	}

	private void HandleBar()
	{
		if (fillAmount != content.fillAmount) 
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * 3);
		}

		if (content.fillAmount > 0.8) {
			content.color = fullColor;
		} else if (content.fillAmount > 0.4) {
			content.color = midColor;
		} else {
			content.color = lowColor;
		}

//		content.color = Color.Lerp (lowColor, fullColor, fillAmount);
	}

	private float Map(float value, float healthMin, float healthMax, float expMin, float expMax)
	{
		return (value - healthMin) * (expMax - expMin) / (healthMax - healthMin) + expMin;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spinner : MonoBehaviour {

	public Image SpinnerImage;

	// Use this for initialization
	void Start () {
		SpinnerImage.fillAmount = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (SpinnerImage.fillAmount == 1f) {
			SpinnerImage.fillAmount = 0f;
		}

		SpinnerImage.fillAmount += Time.deltaTime;
	}
}

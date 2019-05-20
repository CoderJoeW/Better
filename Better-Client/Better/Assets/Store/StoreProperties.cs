using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class StoreProperties : MonoBehaviour {

	public static StoreProperties INSTANCE;

	void Awake() {
		INSTANCE = this;
	}

	public string currencyCode;

	public enum StoreTheme {
		BASIC,
		AQUA_PAPER,
		DARK_STONE,
		DIAMOND,
		BUBBLES,
		MARBLE,
		METAL,
		MOSS,
		PINSTRIPE,
		WEATHERED,
		WOOD
	}
	
	public StoreTheme storeTheme = StoreTheme.BASIC;

	public enum PayPalEndpoint {
		SANDBOX,
		LIVE
	}

	public PayPalEndpoint payPalEndpoint;

	[HideInInspector]
	public GameObject[] storeScreens;

	[HideInInspector]
	public Text[] textBoxes;

	// Use this for initialization
	void Start () {

		//if basic is selected then don't change background
		if (storeTheme != StoreTheme.BASIC) {
	
			for (int i=0; i<storeScreens.Length; i++) {
				GameObject nextStoreScreen = storeScreens [i];
				nextStoreScreen.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("StoreThemes/" + storeTheme.ToString ());
				nextStoreScreen.GetComponent<Image> ().color = Color.white;
			}
		}

		setTextColours ();

		GetComponent<GetAccessTokenAPI_Call> ().enabled = true;

		InvokeRepeating ("checkForValidPayPalCreds", 1f, 1f);
		DialogScreenActions.INSTANCE.setContextStoreOpen ();
		DialogScreenActions.INSTANCE.ShowDialogScreen();

	}

	private void checkForValidPayPalCreds() {

		bool validCreds = GetComponent<GetAccessTokenAPI_Call> ().API_SuccessResponse.access_token != "" ;
		bool invalidCreds =GetComponent<GetAccessTokenAPI_Call> ().API_ErrorResponse.message != "";

		if (validCreds) {
			Debug.Log ("valid creds");
			StoreActions.INSTANCE.OpenStore ();
			DialogScreenActions.INSTANCE.HideDialogScreen ();
			CancelInvoke ("checkForValidPayPalCreds");
		} else if (invalidCreds) {
			CancelInvoke ("checkForValidPayPalCreds");
			DialogScreenActions.INSTANCE.setContextStoreLoadFailure ();
		} else {
			//keep waiting for one of above conditions to happen
		}

	}

	public bool isUsingSandbox() {

		return payPalEndpoint == PayPalEndpoint.SANDBOX;

	}

	private void setTextColours() {

		Color textColorToUse = Color.black;

		switch (storeTheme) {

		case StoreTheme.METAL : 
		case StoreTheme.DARK_STONE :
		case StoreTheme.BASIC :
		case StoreTheme.WEATHERED : {

				textColorToUse = Color.white;

			} break;

		}

		foreach (Text t in textBoxes) {
			t.color = textColorToUse;
		}

	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAccessTokenAPI_Call : MonoBehaviour {

	public string clientID;
	public string secret;

	//[HideInInspector]
	public PayPalGetAccessTokenJsonResponse API_SuccessResponse;

	//[HideInInspector]
	public PayPalErrorJsonResponse API_ErrorResponse;

	// Use this for initialization
	void Start () {
		Debug.Log("calling coroutine");
		StartCoroutine (MakePayAPIcall ());
	}

	void handleSuccessResponse(string responseText) {

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalGetAccessTokenJsonResponse>(responseText);
		Debug.Log ("parsed response");

	}

	void handleErrorResponse(string responseText, string errorText) {

		//attempt to parse error response 
		API_ErrorResponse = JsonUtility.FromJson<PayPalErrorJsonResponse>(responseText);

		//if no responseText and only error text
		if (API_ErrorResponse == null) {
			API_ErrorResponse = new PayPalErrorJsonResponse ();
			API_ErrorResponse.message = errorText;
		}

		Debug.Log ("parsed response");

	}

	IEnumerator MakePayAPIcall() {

		Dictionary<string,string> headers = new Dictionary<string, string >();

		headers.Add("Accept","application/json");
		headers.Add("Accept-Language","en_US");
		headers.Add("Authorization","Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes( clientID + ":" + secret)));

		WWWForm postData = new WWWForm();

		postData.AddField("grant_type", "client_credentials");

		string endpointURL = StoreProperties.INSTANCE.isUsingSandbox () ?
			"https://api.sandbox.paypal.com/v1/oauth2/token" :
			"https://api.paypal.com/v1/oauth2/token";

		WWW www = new WWW(endpointURL, postData.data, headers);

		Debug.Log("Making call to: " + endpointURL);

		yield return www;

		//if ok response
		if (www.error == null) {
			Debug.Log("WWW Ok! Full Text: " + www.text);
			handleSuccessResponse (www.text);

		} else {
			Debug.Log("WWW Error: "+ www.error);
			handleErrorResponse (www.text, www.error);
		}    
	}
}

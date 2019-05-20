using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPaymentAPI_Call : MonoBehaviour {

	public string payID;

	public string accessToken;

	//[HideInInspector]
	public PayPalShowPaymentJsonResponse API_SuccessResponse;

	//[HideInInspector]
	public PayPalErrorJsonResponse API_ErrorResponse;

	// Use this for initialization
	void Start () {
		Debug.Log("calling coroutine");
		StartCoroutine (MakePayAPIcall ());
	}

	void handleSuccessResponse(string responseText) {

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalShowPaymentJsonResponse>(responseText);
		Debug.Log ("parsed response");

	}

	void handleErrorResponse(string responseText, string errorText) {

		//attempt to parse error response 
		API_ErrorResponse = JsonUtility.FromJson<PayPalErrorJsonResponse>(responseText);
		Debug.Log ("parsed response");

	}

	IEnumerator MakePayAPIcall() {

		Dictionary<string,string> headers = new Dictionary<string, string >();

		headers.Add("Content-Type","application/json");
		headers.Add("Authorization","Bearer " + accessToken);

		string baseEndpointURL = StoreProperties.INSTANCE.isUsingSandbox () ?
			"https://api.sandbox.paypal.com/v1/payments/payment/" :
			"https://api.paypal.com/v1/payments/payment/";

		string endpointURL = baseEndpointURL + payID;
		WWW www = new WWW(endpointURL, null, headers);

		Debug.Log("Making call to: " + endpointURL);

		yield return www;

		//if ok response
		if (www.error == null) {
			Debug.Log("WWW Ok! Full Text: " + www.text);

			handleSuccessResponse (www.text);

		} else {
			Debug.Log("WWW Error: "+ www.error);
			Debug.Log("WWW Text: "+ www.text);

			handleErrorResponse (www.text, www.error);
		}    
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePaymentAPI_Call : MonoBehaviour {

	public string accessToken;

	public string transactionDescription;

	public string itemName;

	public string itemDescription;

	public string itemPrice;

	public string itemCurrency;

	public TextAsset JSON_CreatePaymentRequest;

	//[HideInInspector]
	public PayPalCreatePaymentJsonResponse API_SuccessResponse;

	//[HideInInspector]
	public PayPalErrorJsonResponse API_ErrorResponse;

	// Use this for initialization
	void Start () {
		Debug.Log("calling coroutine");
		StartCoroutine (MakePayAPIcall ());
	}

	void handleSuccessResponse(string responseText) {

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalCreatePaymentJsonResponse>(responseText);
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

		byte[] formData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(createRequest()));

		string endpointURL = StoreProperties.INSTANCE.isUsingSandbox () ?
			"https://api.sandbox.paypal.com/v1/payments/payment" :
			"https://api.paypal.com/v1/payments/payment";

		WWW www = new WWW(endpointURL, formData, headers);

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

	PayPalCreatePaymentJsonRequest createRequest() {

		//create skeleton request object
		PayPalCreatePaymentJsonRequest request = JsonUtility.FromJson<PayPalCreatePaymentJsonRequest> (JSON_CreatePaymentRequest.text);

		//map provided values into skeleton object
		request.transactions [0].amount.total = itemPrice;
		request.transactions [0].amount.currency = itemCurrency;
		request.transactions [0].description = transactionDescription;
		request.transactions [0].invoice_number = System.Guid.NewGuid().ToString();
		request.transactions [0].item_list.items [0].name = itemName;
		request.transactions [0].item_list.items [0].description = itemDescription;
		request.transactions [0].item_list.items [0].price = itemPrice;
		request.transactions [0].item_list.items [0].currency = itemCurrency;

		return request;

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentListener : MonoBehaviour {
	
	public string payID;

	public string accessToken;

	public float listeningInterval;

	public enum ListenerState { NOT_STARTED, LISTENING, VERIFIED, SUCCESS, FAILURE };

	public ListenerState listenerStatus;

	void Start() {
		StartListening ();
	}

	void Update() {

		if (listenerStatus == ListenerState.LISTENING) {
			checkAPI_Response ();
		}

		if (listenerStatus == ListenerState.VERIFIED) {
			checkExecutePaymentAPI_Response ();
		}

	}

	// Use this for initialization
	public void StartListening () {
		listenerStatus = ListenerState.LISTENING;
		InvokeRepeating ("pollPayPalShowPaymentAPI", 0f, listeningInterval);
	}


	private void pollPayPalShowPaymentAPI() {

		ShowPaymentAPI_Call lastAPIcall = this.GetComponent<ShowPaymentAPI_Call> ();

		if (lastAPIcall == null) {
			Debug.Log ("no previous call exists"); 
		} else {
			Debug.Log ("not yet verified");
			Destroy (lastAPIcall);
		}

		Debug.Log ("creating new api call");
		ShowPaymentAPI_Call apiCall = this.gameObject.AddComponent<ShowPaymentAPI_Call> ();

		apiCall.accessToken = accessToken;
		apiCall.payID = payID;

	}


	private void pollPayPalExecutePaymentAPI() {

		ExecutePaymentAPI_Call lastAPIcall = this.GetComponent<ExecutePaymentAPI_Call> ();

		if (lastAPIcall == null) {
			Debug.Log ("no previous call exists"); 
		} else {
			Debug.Log ("not yet verified");
			Destroy (lastAPIcall);
		}

		Debug.Log ("creating new api call");
		ExecutePaymentAPI_Call apiCall = this.gameObject.AddComponent<ExecutePaymentAPI_Call> ();

		apiCall.accessToken = accessToken;
		apiCall.paymentID = payID;
		apiCall.payerID = GetComponent<ShowPaymentAPI_Call> ().API_SuccessResponse.payer.payer_info.payer_id;

	}

	private void checkAPI_Response() {

		ShowPaymentAPI_Call lastAPIcall = GetComponent<ShowPaymentAPI_Call> ();

		if (lastAPIcall != null && lastAPIcall.API_SuccessResponse != null && lastAPIcall.API_SuccessResponse.payer.status == "VERIFIED") {
			CancelInvoke ("pollPayPalShowPaymentAPI");
			transitionToVerified ();
		}

	}

	private void checkExecutePaymentAPI_Response() {

		ExecutePaymentAPI_Call executePaymentAPI_Call = this.gameObject.GetComponent<ExecutePaymentAPI_Call> ();

		if (executePaymentAPI_Call != null && executePaymentAPI_Call.API_SuccessResponse != null && executePaymentAPI_Call.API_SuccessResponse.state == "approved") {
			transitionToSuccess ();
		} 

		if (executePaymentAPI_Call != null && executePaymentAPI_Call.API_SuccessResponse != null && executePaymentAPI_Call.API_SuccessResponse.state == "failed") {
			transitionToFailure ();
		} 

	}

	private void transitionToVerified() {
		listenerStatus = ListenerState.VERIFIED;
		Debug.Log ("verified");

		InvokeRepeating ("pollPayPalExecutePaymentAPI", 0f, listeningInterval);
	}
		
	private void transitionToSuccess() {
		listenerStatus = ListenerState.SUCCESS;
		CancelInvoke ("pollPayPalExecutePaymentAPI");
		Debug.Log ("success");
	}
		
	private void transitionToFailure() {
		listenerStatus = ListenerState.FAILURE;
		CancelInvoke ("pollPayPalExecutePaymentAPI");
		Debug.Log ("failure");

	}

}

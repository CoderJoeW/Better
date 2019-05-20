using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogScreenActions : MonoBehaviour {

	public static DialogScreenActions INSTANCE;

	public enum DialogContext {
		UNSPECIFIED,
		STORE_LOADING,
		STORE_LOAD_FAILURE,
		CONFIRM_ABORT_PAYMENT,
		PURCHASE_SUCCESS
	};

	public DialogContext dialogContext;

	public GameObject dialogScreen;

	public Text dialogTitle;

	public Text dialogMessage;

	public Button YesButton;

	public Button NoButton;

	public Button OkButton;

	public GameObject DialogScreenSpinner;

	void Awake() {
		INSTANCE = this;
	}
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowDialogScreen() {
		dialogScreen.SetActive (true);
	}

	public void HideDialogScreen() {
		dialogScreen.SetActive (false);
		dialogTitle.text = "";
		dialogMessage.text = "";
		OkButton.gameObject.GetComponentInChildren<Text> ().text = "OK";
		dialogContext = DialogContext.UNSPECIFIED;
	}

	public void onClickYes() {
		if (dialogContext == DialogContext.CONFIRM_ABORT_PAYMENT) {
			HideDialogScreen ();
			StoreActions.INSTANCE.resetCheckoutScreen ();
			MenuNavigation.INSTANCE.selectStoreIcon ();
		}
	}

	public void onClickNo() {
		if (dialogContext == DialogContext.CONFIRM_ABORT_PAYMENT) {
			HideDialogScreen ();
		}
	}

	public void onClickOk() {

		if (dialogContext == DialogContext.STORE_LOAD_FAILURE) {
			HideDialogScreen ();
		}

		if (dialogContext == DialogContext.STORE_LOADING) {
			HideDialogScreen ();
		}

		if (dialogContext == DialogContext.PURCHASE_SUCCESS) {
			UseItem.Use (StoreActions.INSTANCE.PurchaseItemNameField.text);
			HideDialogScreen ();
			StoreActions.INSTANCE.resetCheckoutScreen ();
			MenuNavigation.INSTANCE.selectStoreIcon ();
		}

	}

	public void setContextStoreOpen() {

		dialogContext = DialogContext.STORE_LOADING;
		dialogTitle.text = "Loading...";
		dialogMessage.text = "";

		DialogScreenSpinner.SetActive (true);

		NoButton.gameObject.SetActive (false);
		YesButton.gameObject.SetActive (false);
		OkButton.gameObject.SetActive (true);
		OkButton.gameObject.GetComponentInChildren<Text> ().text = "Close";
	}

	public void setContextStoreLoadFailure() {

		dialogContext = DialogContext.STORE_LOAD_FAILURE;
		dialogTitle.text = "Error";
		dialogMessage.text = "Store failed to load. Please Try again Later.";
		OkButton.gameObject.SetActive (true);
		NoButton.gameObject.SetActive (false);
		YesButton.gameObject.SetActive (false);

		DialogScreenSpinner.SetActive (false);

	}

	public void setContextConfirmAbortPayment() {

		dialogContext = DialogContext.CONFIRM_ABORT_PAYMENT;
		dialogTitle.text = "Cancel Purchase?";
		dialogMessage.text = "Are you sure you want to cancel this purchase?";
		OkButton.gameObject.SetActive (false);
		NoButton.gameObject.SetActive (true);
		YesButton.gameObject.SetActive (true);

		DialogScreenSpinner.SetActive (false);

	}

	public void setContextPurchaseSuccess() {

		dialogContext = DialogContext.PURCHASE_SUCCESS;
		dialogTitle.text = "Purchase Complete!";
		dialogMessage.text = "Your purchase was successfull!  Click OK to return to the store.";
		OkButton.gameObject.SetActive (true);
		NoButton.gameObject.SetActive (false);
		YesButton.gameObject.SetActive (false);

		DialogScreenSpinner.SetActive (false);

	}
		
}

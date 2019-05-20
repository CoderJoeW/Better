using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour {

	public static MenuNavigation INSTANCE;

	public void Awake () {
		INSTANCE = this;
	}

	public GameObject MainStoreScreen;
	public GameObject PurchaseScreen;
	public GameObject HelpScreen;

	public RectTransform MainStoreHeaderParent;
	public RectTransform PurchaseHeaderParent;
	public RectTransform HelpHeaderParent;

	public RectTransform Header;

	public Image storeIconBG;
	public Image purchaseIconBG;
	public Image helpIconBG;


	public void selectStoreIcon() {

		if ( ( (Color32) storeIconBG.color).a == 0)  {
			return;
		}

		if (StoreActions.INSTANCE.currentPurchaseStatus == StoreActions.PurchaseStatus.WAITING) {
			DialogScreenActions.INSTANCE.setContextConfirmAbortPayment ();
			DialogScreenActions.INSTANCE.ShowDialogScreen ();
			return;
		}
			
		storeIconBG.color = new Color32 (255, 255, 255, 0);
		helpIconBG.color = new Color32(255, 255, 255,50);
		purchaseIconBG.color = new Color32(255, 255, 255,50);

		Header.SetParent (MainStoreHeaderParent);

		MainStoreScreen.SetActive (true);
		HelpScreen.SetActive (false);
		PurchaseScreen.SetActive (false);

	}

	public void selectPurchaseIcon() {
		
		if ( ( (Color32) purchaseIconBG.color).a == 0)  {
			return;
		}
		
		storeIconBG.color = new Color32(255, 255, 255,50);
		helpIconBG.color = new Color32(255, 255, 255,50);
		purchaseIconBG.color = new Color32(255, 255, 255,0);
		
		Header.SetParent (PurchaseHeaderParent);
		
		MainStoreScreen.SetActive (false);
		HelpScreen.SetActive (false);
		PurchaseScreen.SetActive (true);
				
	}

	public void selectHelpIcon() {

		if ( ( (Color32) helpIconBG.color).a == 0)  {
			return;
		}

		storeIconBG.color = new Color32(255, 255, 255,50);
		helpIconBG.color = new Color32(255, 255, 255,0);
		purchaseIconBG.color = new Color32(255, 255, 255,50);

		Header.SetParent (HelpHeaderParent);

		MainStoreScreen.SetActive (false);
		HelpScreen.SetActive (true);
		PurchaseScreen.SetActive (false);

	}

}

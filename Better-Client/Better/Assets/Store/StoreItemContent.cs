using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class StoreItemContent : MonoBehaviour {

	/* expose these values to user for convinience 
	 * (NOTE: these values will override any values set on the store item fields in the inspector)
	 */
	public Sprite itemImage;
	public string itemName;
	public string itemCost;
	public string itemDesc;

	private Image itemImageField;
	private Text itemNameTextField;
	private Text itemCostTextField;
	private Text itemCurCodeTextField;
	private Text itemDescTextField;

	// Use this for initialization
	void Start () {

		itemImageField = transform.Find ("ItemImage").GetComponent<Image> ();
		itemNameTextField = transform.Find ("ItemName").GetComponent<Text> ();
		itemCostTextField = transform.Find ("ItemCost").GetComponent<Text> ();
		itemCurCodeTextField = transform.Find ("ItemCurCode").GetComponent<Text> ();
		itemDescTextField = transform.Find ("ItemDesc").GetComponent<Text> ();
			
		if (itemImage == null) {
			itemImage = Resources.Load <Sprite> ("ItemSprites/DefaultImage");
		}

		itemImageField.sprite = itemImage;

		if (itemName.Length > 100) {
			itemName = itemName.Substring(0,99);
		}

		itemNameTextField.text = itemName;

		itemCostTextField.text = string.Format("{0:N}", itemCost);
		itemCostTextField.text = CurrencyCodeMapper.GetCurrencySymbol (StoreProperties.INSTANCE.currencyCode) + itemCostTextField.text;

		itemCurCodeTextField.text = "(" + StoreProperties.INSTANCE.currencyCode + ")";

		itemDescTextField.text = itemDesc;

	}
		
	public void BuyItemAction() {
		Debug.Log ("Tried to buy a "  + itemName);
		StoreActions.INSTANCE.OpenPurchaseItemScreen (this);
	}
	
}

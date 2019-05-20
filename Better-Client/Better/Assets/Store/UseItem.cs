using UnityEngine;
using System.Collections;

public class UseItem : MonoBehaviour {


	public static void Use (string itemName) {
        ClientTCP.PACKET_PurchaseCompleted(itemName);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPopupController : Singleton<LoadPopupController>{
    [SerializeField]
    private GameObject popupObject;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text messageText;

    [SerializeField]
    private Button actionBtn;

    [SerializeField]
    private Text btnText;

    public void ShowPopup(string title,string message,string action) {
        titleText.text = title;
        messageText.text = message;
        actionBtn.onClick.AddListener(delegate {
            Action(action);
        });
        btnText.text = action;
        popupObject.SetActive(true);
    }

    private void Action(string action) {
        switch (action) {
            case "Update":
                GameManager.Instance.LoadURL("https://deathcrow.altervista.org/better.apk");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAvailable : MonoBehaviour{
    [SerializeField]
    private string updateUrl;

    [SerializeField]
    private Button openButton;

    private void Start() {
        openButton.onClick.AddListener(delegate {
            GameManager.Instance.LoadURL(updateUrl);
        });
    }
}

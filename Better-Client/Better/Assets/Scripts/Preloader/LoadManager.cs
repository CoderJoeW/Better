using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : Singleton<LoadManager>{
    [SerializeField]
    private Slider progressBar;

    [SerializeField]
    private Text statusText;

    [SerializeField]
    private Text hintText;

    public void UpdateProgress(string status) {
        switch (status) {
            case "VersionCheck":
                progressBar.value = .25f;
                statusText.text = "Checking For Update!";
                break;
            case "AccountVerification":
                progressBar.value = .45f;
                statusText.text = "Checking For Account!";
                break;
            case "AccountCreation":
                progressBar.value = .75f;
                statusText.text = "Creating Account!";
                break;
            case "AccountVerified":
                progressBar.value = 1;
                statusText.text = "Done!";
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : ChangeSceneManager {

    public Toggle Azure;
    public Toggle LocalHost;
    public InputField lbl_LocalHost;
    void ChangeSetting(string Scene)
    {
        if(Azure.isOn)
        {
            PlayerPrefs.SetString("api", "https://projectar.azurewebsites.net");
        }
        else if(LocalHost)
        {
            IPAddress IP;
            bool ok=IPAddress.TryParse(lbl_LocalHost.text, out IP);
            if(ok)
            {
                PlayerPrefs.SetString("api", IP.ToString());
            }
            else
            {
                PlayerPrefs.SetString("api", "0.0.0.0");
            }
        }
        GoToScene(Scene);
    }
    private void Start()
    {
        if(PlayerPrefs.GetString("api") == "https://projectar.azurewebsites.net")
        {
            Azure.isOn = true;
        }
        else
        {
            LocalHost.isOn = true;
        }
    }
}

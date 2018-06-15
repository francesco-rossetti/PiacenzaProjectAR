using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : ChangeSceneManager {

    public Toggle Azure;
    public Toggle LocalHost;
    public InputField lbl_LocalHost;
    public InputField lbl_port;
    public void ChangeSetting(string Scene)
    {
        if(Azure.isOn)
        {
            PlayerPrefs.SetString("api", "http://projectar.rossdev.it");
        }
        else if(LocalHost)
        {
            IPAddress IP;
            int port;
            bool okp = int.TryParse(lbl_port.text, out port);
            bool ok=IPAddress.TryParse(lbl_LocalHost.text, out IP);
            if(ok || okp)
            {
                PlayerPrefs.SetString("api", "http://" + IP.ToString() + ":" + (okp ? port : 80));
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
        if(PlayerPrefs.GetString("api") == "http://projectar.rossdev.it")
        {
            Azure.isOn = true;
            LocalHost.isOn = false;
        }
        else
        {
            Azure.isOn = false;
            LocalHost.isOn = true;
        }
    }
}

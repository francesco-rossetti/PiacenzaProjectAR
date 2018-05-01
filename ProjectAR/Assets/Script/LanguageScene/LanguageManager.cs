using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public GameObject Loading; //Loading graphics
    public ToggleElement[] toggles; //All languages toggle
    // Use this for initialization
    private void Awake()
    {
        Button btn = GameObject.Find("BackButton").GetComponent<Button>();
        btn.onClick.AddListener(() => {  StartCoroutine(BackButton()); });
        foreach (ToggleElement te in toggles)//For each toggles add OnValueChanged listener that run only when it's checked
        {
            te.t.onValueChanged.AddListener((bool on) =>
            {
                if (on)
                {
                    ChangeLanguage(te.value);
                }
            });
        }
    }
    void Start() //enable only the toggle of the language activated
    {
        GameObject toggle = GameObject.Find("Toggle_" + PlayerPrefs.GetString("Language"));
        if (toggle != null)
        {
            toggle.GetComponent<Toggle>().isOn = true;
        }
    }

    // Update is called once per frame
    public void ChangeLanguage(string Lang) //Change the current language
    {
        PlayerPrefs.SetString("Language", Lang);
        //LocalizationManager.instance.LoadLocalizedText("language_" + PlayerPrefs.GetString("Language") + ".json");
        Debug.Log(PlayerPrefs.GetString("Language"));

    }
    public IEnumerator BackButton()
    {
        Debug.Log("BackButton");
        Loading.SetActive(true);
        foreach (ToggleElement te in toggles)//For each toggles add OnValueChanged listener that run only when it's checked
        {
            te.t.enabled = false;
        }
        LocalizationManager.instance.LoadLocalizedText();
        while (!LocalizationManager.instance.GetIsReady)
        {
            yield return null;
        }

        SceneManager.LoadScene("GameScene"); //Introduce loading scene
    }
}
[System.Serializable]
public class ToggleElement
{
    public Toggle t;
    public string value;
}



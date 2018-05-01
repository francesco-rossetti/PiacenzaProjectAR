using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Load or deserialize json and load into the dictionary
/// </summary>
public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance; //To access from other script to this instance
    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Err001";

    void Awake()
    { //Run before other thing  We only want 1 instance of this object
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Language")))
        {
            PlayerPrefs.SetString("Language", "it");

        }

        Debug.Log(PlayerPrefs.GetString("Language"));
        LoadLocalizedText();
        DontDestroyOnLoad(gameObject);
    }
    public void LoadLocalizedText()  //Load the file with all the translated text
    {
        isReady = false;
        string fileName = "language_" + PlayerPrefs.GetString("Language") + ".json";
        localizedText = new Dictionary<string, string>();

#if UNITY_EDITOR
        string filepath = Application.streamingAssetsPath + "/" + fileName;
        if (File.Exists(filepath)) //Check if the file with language exists
        {
            string dataAsJson = File.ReadAllText(filepath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            // Debug.Log(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            //Unity cannot serialize unorder collections, so you cannot make public dictionary and editable in the inspector

        }
        else
        {
            Debug.LogError("Cannot find file");
            Debug.LogError(filepath);
        }
#elif UNITY_ANDROID
        Debug.Log("Ok");
        string filepath = "jar:file://" + Application.dataPath + "!/assets/"+fileName;
         if (Application.platform == RuntimePlatform.Android)
             {
                 WWW reader = new WWW(filepath);
                 while (!reader.isDone) { }
                 string jsonString = reader.text;
                    LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonString);
            // Debug.Log(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
             }
#endif


        isReady = true;
        Debug.Log("IS READY DUDE");
    }

    public string GetLocalizedValue(string key) //To return the text of the dictionary
    {
        string result = missingTextString;

        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public bool GetIsReady
    {
        get { return isReady; }
    }
}

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

        string Path = "language_" + PlayerPrefs.GetString("Language") + ".json";
        Debug.Log(PlayerPrefs.GetString("Language"));
        LoadLocalizedText(Path);
        DontDestroyOnLoad(gameObject);
    }
    public void LoadLocalizedText(string fileName)  //Load the file with all the translated text
    {
        localizedText = new Dictionary<string, string>();
        string filepath = Application.streamingAssetsPath + "/" + fileName;// Path.Combine("jar:file://" + Application.dataPath + "!/assets/",fileName);
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

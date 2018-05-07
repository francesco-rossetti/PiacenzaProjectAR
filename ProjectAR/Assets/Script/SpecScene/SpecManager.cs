using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecManager : ChangeSceneManager {

    public Text title;
    public Text[] field;
    public Image[] image;
    /*
     * Err001 language not found
     * Err002 title not found
     * Err002a Connection error title
     * Err003 field not found
     * Err003a Connection error field
     * Err004 insert/update/delete error
     * Err005 museum url not found
     */
    // Use this for initialization
    private GameObject Variable;
    private APIManager Api;
    int idMonument;
    void Start () {
        idMonument = 1; // PlayerPrefs.GetInt("iDMonument");
        Api = new APIManager();
        Title();
        Field();
    }
    void Title()
    {   
        Monument ApiTitle = Api.GetMonumentName(idMonument);
        if (ApiTitle.status == "ok")
        {
            title.text = ApiTitle.result;
        }
        else
        {
            title.text = "Err002a"; //Connection Error
        }
    }
    void Field()
    {
        Field ApiField = Api.GetField(idMonument);
        if (ApiField.status == "ok")
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            foreach(BufferItem BI in ApiField.result)
            {
                Result.Add(BI.key, BI.value);
            }
            for (int i = 1; i < 4; i++)
            {
                field[i - 1].text = Result["field_" + i];
            }
        }
        else
        {
            title.text = "Err002a"; //Connection Error
        }
    }

    void URL()
    {
        Monument ApiTitle = Api.GetMonumentURL(idMonument);
        if (ApiTitle.status == "ok")
        {
            if (ApiTitle.result != "Err005")
                Application.OpenURL(ApiTitle.result);
            else
            {
                //TODO: gestiscila come più ti piace
            }
        }
        else
        {
            //TODO: gestiscila come più ti piace
        }
    }
}
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
     */
    // Use this for initialization
    private GameObject Variable;
    private APIManager Api;
    int idMonument;
    void Start () {
        idMonument = PlayerPrefs.GetInt("iDMonument");
        Api = new APIManager();
        Title();
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
            if(ApiField.keys["result"].Length==0)
            {

            }
        }
        else
        {
            title.text = "Err002a"; //Connection Error
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecManager : ChangeSceneManager {

    public Text title;
    public Text[] field;
    public Image[] image;
    // Use this for initialization
    private GameObject Variable;
    private APIManager Api;
    void Start () {

        Api = new APIManager();
        Title();
        Field();
    }
    void Title()
    {   
        Monument ApiTitle = Api.GetMonumentName();
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
        Field ApiField = Api.GetField();
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


}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager
{
    /*
    * ErrStatus status
    * Err001 language not found
    * Err002 title not found
    * Err002a Connection error title
    * Err003 field not found
    * Err003a Connection error field
    * Err004 insert/update/delete error
    * Err005 museum url not found
    */
    private string api;

    public APIManager()
    {
        api = PlayerPrefs.GetString("api");
    }
    public Monument GetMonumentName()
    {
        int id = PlayerPrefs.GetInt("APIID");
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        WebRequest request = (WebRequest)WebRequest.Create(new Uri(api + "/api/getMonumentName?idmon=" + id));
        request.ContentType = "application/json";
        request.Method = "GET";

        using (WebResponse response = request.GetResponse())
        {
            // Get a stream representation of the HTTP web response:
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                Monument jsonDoc = (Monument)Monument.CreateFromJSON(sr.ReadToEnd());
                return jsonDoc;
            }
        }
    }

    public Monument GetMonumentURL()
    {
        int id = PlayerPrefs.GetInt("APIID");
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        WebRequest request = (WebRequest)WebRequest.Create(new Uri(api + "/api/getURL?idmon=" + id));
        request.ContentType = "application/json";
        request.Method = "GET";

        using (WebResponse response = request.GetResponse())
        {
            // Get a stream representation of the HTTP web response:
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                Monument jsonDoc = (Monument)Monument.CreateFromJSON(sr.ReadToEnd());
                return jsonDoc;
            }
        }
    }

    public Field GetField()
    {
        int id = PlayerPrefs.GetInt("APIID");
        string lang = PlayerPrefs.GetString("Language");
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        WebRequest request = (WebRequest)WebRequest.Create(new Uri(api + "/api/getField?idmon=" + id + "&lang=" + lang));
        request.ContentType = "application/json";
        request.Method = "GET";

        using (WebResponse response = request.GetResponse())
        {
            // Get a stream representation of the HTTP web response:
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                Field jsonDoc = Field.FromJSON(sr.ReadToEnd());
                return jsonDoc;
            }
        }
    }
}

[Serializable]
public class Monument
{
    public string status;
    public string result;
    public static object CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Monument>(jsonString);
    }
}

[Serializable]
public class Field
{
    public string status;
    public BufferItem[] result;
    public static Field FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Field>(jsonString);
    }
}


[Serializable]
public class BufferItem
{
    public string key;
    public string value;
}

/*
{
    "status": "ok",
    "result": [
        {
            "key": "field_1",
            "value": "Err003"
        },
        {
            "key": "field_2",
            "value": "Err003"
        },
        {
            "key": "field_3",
            "value": "Err003"
        }
    ]
}
*/
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
    private string api = "https://projectar.azurewebsites.net";

    public Monument GetMonumentName(int id)
    {
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

    public Monument GetMonumentURL(int id)
    {
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

    public Field GetField(int id)
    {
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
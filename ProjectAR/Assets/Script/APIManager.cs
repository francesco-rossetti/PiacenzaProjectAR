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

    public Field GetField(int id, string lang)
    {
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
                Field jsonDoc = (Field)Field.CreateFromJSON(sr.ReadToEnd());
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
    public Dictionary<string, string> keys;
    public static object CreateFromJSON(string jsonString)
    {
        var dess = JsonUtility.FromJson<Field>(jsonString);
        dess.keys = BufferJson.CreateFromJSON(jsonString);
        return dess;
    }
}

[Serializable]
public class BufferJson
{
    public BufferItem[] result;
    public static Dictionary<string, string> CreateFromJSON(string jsonString)
    {
        Dictionary<string, string> keys = new Dictionary<string, string>();
        BufferJson des = JsonUtility.FromJson<BufferJson>(jsonString);
        for (int i = 0; i < des.result.Length; i++)
        {
            keys.Add("field_" + des.result[i].key, des.result[i].value);
        }

        return keys;
    }
}

public class BufferItem
{
    public string key;
    public string value;
}
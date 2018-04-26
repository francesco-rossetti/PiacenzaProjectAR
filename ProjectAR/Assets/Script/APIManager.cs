using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class APIManager
{
    private string api = "http://192.168.1.100:3000";
    AsyncOperation async;
    public string GetMonument(int ID)
    {
        try
        {
            Monument Json;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api + "/GetMonument?idMonument=" + ID);
            request.ContentType = "application/json; charset=utf-8";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string Text=reader.ReadToEnd();
                Json = JsonUtility.FromJson<Monument>(Text);
            }
            if (Json.status == "ok")
                return Json.title;
            else
                return "Err";
        }
        catch
        {
            return "Exception";
        }

    }
}

public class Monument
{
    public string status;
    public string title;
}
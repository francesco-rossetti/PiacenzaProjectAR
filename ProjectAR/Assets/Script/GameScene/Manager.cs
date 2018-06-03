using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : ChangeSceneManager
{
    public Texture2D Map;
    private APIManager api;
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private AndroidJavaObject currentActivity;
    private string ToastString;

    void Start()
    {

        PlayerPrefs.SetInt("idMonument", 0);
        dragDistance = Screen.height * 5 / 100; //dragDistance is 5% height of the screen
        api = new APIManager();
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

    }

    void Update()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
                if (Mathf.Abs(lp.y - fp.y) > dragDistance)
                {

                    if (lp.y > fp.y && PlayerPrefs.GetInt("idMonument") != 0)  //Move Up
                    {                      
                        GoToScene("SpecScene");
                    }
                    else
                    {
                        ToastString = LocalizationManager.instance.GetLocalizedValue("Manager_Err");//"Errore selezione immagine"; 
                        showToastOnUiThread();
                    }
                }
                fp = touch.position;
            }
            //else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            //{
            //    if(isDrag)

            //    isDrag = false;
            //}
        }
    } //Check for the swipe UP

    public void MapsButton()
    {
        if (PlayerPrefs.GetInt("idMonument") != 0)
        {
            Monument ApiTitle = api.GetMonumentURL();
            if (ApiTitle.status == "ok")
            {
                if (ApiTitle.result != "Err005")
                    Application.OpenURL(ApiTitle.result);
                else
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        ToastString = "Err005"; //Lingua
                        showToastOnUiThread();
                    }
                }

            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    ToastString = "err status";
                    showToastOnUiThread();
                }
            }
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                ToastString = LocalizationManager.instance.GetLocalizedValue("Manager_Err");//"Errore selezione immagine"; 
                //ToastString = "Errore selezione immagine"; //Lingua
                showToastOnUiThread();
            }
        }
    }
    void showToastOnUiThread()
    {
        //Get the value of element, in this case currentActivity

        //the showToast which we pass as parameter here is a method which we will write next.
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
    }
    void showToast()
    {
        Debug.Log("Running on UI thread");

        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext"); //Get the application context
        /*
         * AndroidJavaClass --> It is the class, NOT THE INSTANCE
         * AndroidJavaObject --> It is The instance
         */
        //https://developer.android.com/reference/android/widget/Toast
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast"); //Instantiate Toast Java Class
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", ToastString);  //Instantiate the string ToastString as JavaString
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));//Call makeText Method( context,string,length)

        toast.Call("show");

    }
    public void DownloadMap()
    {
        var dir = "/storage/emulated/0/";
        Directory.CreateDirectory(dir + "Map");
        File.WriteAllBytes(dir + "Map/Map.jpg",Map.EncodeToJPG());
        if (Application.platform == RuntimePlatform.Android)
        {
            ToastString = LocalizationManager.instance.GetLocalizedValue("DownloadMap_msg");//"Errore selezione immagine";                                                                               //ToastString = "Errore selezione immagine"; //Lingua
            showToastOnUiThread();
        }


    }
}

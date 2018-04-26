using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    // Use this for initialization
    public GameObject Loading;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    AsyncOperation async; //To load scene asycn
    void Start()
    {
        PlayerPrefs.SetInt("idMonument", 0);
        dragDistance = Screen.height * 5 / 100; //dragDistance is 15% height of the screen
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

                    if (lp.y > fp.y && PlayerPrefs.GetInt("idMonument")!=0)  //Move Up
                    {
                        
                        Loading.SetActive(true);
                        StartCoroutine(LoadScene());
                        async.allowSceneActivation = true;
                        
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

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("SpecScene");
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void ChangeLanguage()
    {
        if(PlayerPrefs.GetString("Language") =="en") //Change to it
        {
            PlayerPrefs.SetString("Language", "it");
            LocalizationManager.instance.LoadLocalizedText("language_" + PlayerPrefs.GetString("Language") + ".json");
        }
        else
        {
            PlayerPrefs.SetString("Language", "en");
            LocalizationManager.instance.LoadLocalizedText("language_" + PlayerPrefs.GetString("Language")+".json");
        }
        SceneManager.LoadScene("GameScene");
    }
}

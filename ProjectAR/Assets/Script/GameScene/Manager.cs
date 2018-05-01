using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : ChangeSceneManager
{

    // Use this for initialization
    public Manager instance;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        PlayerPrefs.SetInt("idMonument", 0);
        dragDistance = Screen.height * 5 / 100; //dragDistance is 15% height of the screen
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "gameScene")
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
                    }
                    fp = touch.position;
                }
                //else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                //{
                //    if(isDrag)

                //    isDrag = false;
                //}
            }
        }

    } //Check for the swipe UP
}

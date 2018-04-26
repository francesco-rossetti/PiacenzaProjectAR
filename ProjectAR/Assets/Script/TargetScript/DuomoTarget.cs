using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DuomoTarget : MonoBehaviour, ITrackableEventHandler
{
    // Use this for initialization
    private GameObject GameManager;
    private void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || //if the Image target appear
     newStatus == TrackableBehaviour.Status.TRACKED ||
     newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            PlayerPrefs.SetInt("idMonument", 2);
        }
        else
        {
            PlayerPrefs.SetInt("idMonument", 0);
        }
    }
}

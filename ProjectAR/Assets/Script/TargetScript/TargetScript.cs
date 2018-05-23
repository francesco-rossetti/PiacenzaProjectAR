using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetScript : MonoBehaviour, ITrackableEventHandler
{
    public int idMonument;
    private GameObject GameManager;
    private TrackableBehaviour mTrackableBehaviour;

    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || //if the Image target appear
     newStatus == TrackableBehaviour.Status.TRACKED ||
     newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            PlayerPrefs.SetInt("idMonument", idMonument);
        }
        else
        {
            PlayerPrefs.SetInt("idMonument", 0);
        }

    }
}

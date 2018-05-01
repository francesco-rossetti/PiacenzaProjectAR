using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class HowToUseManager : ChangeSceneManager
{

    // Use this for initialization
    public VideoPlayer video;
    public void Play()
    {
        video.Play();
    }
    public void Pause()
    {
        video.Pause();
    }
    public void Rewind()
    {
        video.Stop();
        video.Play();
    }
}

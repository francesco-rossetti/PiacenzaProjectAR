using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour {
    AsyncOperation async; //To load scene asycn
    public GameObject Loading;
    public void GoToScene(string NameScene)
    {
        Loading.SetActive(true);
        StartCoroutine(LoadScene(NameScene));
        async.allowSceneActivation = true;
    }
    IEnumerator LoadScene(string NameScene)
    {
        async = SceneManager.LoadSceneAsync(NameScene);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}

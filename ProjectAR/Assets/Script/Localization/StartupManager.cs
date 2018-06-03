using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartupManager : MonoBehaviour {

	// Use this for initialization
	private IEnumerator Start()
    {
        PlayerPrefs.SetString("api", "https://projectar.azurewebsites.net");
        while (!LocalizationManager.instance.GetIsReady)
        {
            yield return null;
        }

        SceneManager.LoadScene("GameScene"); //Introduce loading scene
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecImage : MonoBehaviour {

    public Image image;
    public string key;
	void Start () {
        image.sprite=SpecImageManager.instance.GetImage(PlayerPrefs.GetInt("APIID")+"_"+key);
	}
	

}

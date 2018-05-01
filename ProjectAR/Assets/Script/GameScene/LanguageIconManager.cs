using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageIconManager : MonoBehaviour
{
    public DictionaryImage[] flags;
    private Dictionary<string, Sprite> LanguageImg;

    public void Start()
    {
        LanguageImg = new Dictionary<string, Sprite>();
        for(int i=0;i<flags.Length;i++)
        {
            LanguageImg.Add(flags[i].key, flags[i].flag);
        }
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = LanguageImg[PlayerPrefs.GetString("Language")];
    }
}


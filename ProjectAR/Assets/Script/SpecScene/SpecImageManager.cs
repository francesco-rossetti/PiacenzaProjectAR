using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecImageManager : MonoBehaviour
{
    public SpecDictionaryClass[] items;
    public Dictionary<string, Sprite> GalleryDictionary;
    public static SpecImageManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        GalleryDictionary = new Dictionary<string, Sprite>();
        foreach (SpecDictionaryClass it in items)
        {
            GalleryDictionary.Add(it.Key, it.Image);
        }
    }
    public Sprite GetImage(string key)
    {
        return GalleryDictionary[key];
    }

}
[System.Serializable]
public class SpecDictionaryClass
{
    public string Key;
    public Sprite Image;
}

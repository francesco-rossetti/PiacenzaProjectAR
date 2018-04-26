[System.Serializable]
public class LocalizationData {
    public LocalizationItem[] items;
}

[System.Serializable] //To serialize and deserialize to JSON
public class LocalizationItem
{
    public string key;
    public string value;
}

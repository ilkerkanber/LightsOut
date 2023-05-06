using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class DataManager
{
    public static void SaveData(MapData mapData)
    {
        string dataString = JsonUtility.ToJson(mapData);
        PlayerPrefs.SetString("data", dataString);
    }
    public static void LoadData(MapData mapData)
    {
        if (!PlayerPrefs.HasKey("data"))
        {
            SaveData(mapData);
            return;
        }

        string dataString = PlayerPrefs.GetString("data");
        JsonUtility.FromJsonOverwrite(dataString, mapData);
    }
}

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StartOperations : Singleton<StartOperations>
{
    public GameDatas gameDatas;

    void Awake()
    {
      

        LoadData();

    }


    void LoadData()
    {
#if UNITY_EDITOR

        return;
#endif

        if (PlayerPrefs.HasKey("GameDatas"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("GameDatas"), gameDatas);
        }
    }

    void SaveData()
    {
        PlayerPrefs.SetString("GameDatas", JsonUtility.ToJson(gameDatas));
        PlayerPrefs.Save();

    }

    void OnApplicationQuit()
    {
        SaveData();
    }


    void OnApplicationPause(bool paused)
    {
        if (paused)
            OnApplicationQuit();
    }
}
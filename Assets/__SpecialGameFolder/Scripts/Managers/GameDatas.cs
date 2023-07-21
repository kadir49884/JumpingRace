using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



[CreateAssetMenu(fileName = "GameDatas", menuName = "GameDataManager/GameDatas", order = 1)]
public class GameDatas : ScriptableObject
{
    public int LevelIndex = 1;


    [Button]
    public void ResetGameData()
    {
        LevelIndex = 1;
        Debug.Log("GameData Reset");

    }
}

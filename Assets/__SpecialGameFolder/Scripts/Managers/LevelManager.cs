using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private StartOperations startOperations;
    private GameManager gameManager;
    private int activeLevel;
    private int levelIndex;

    private void Start()
    {
        gameManager = GameManager.Instance;
        startOperations = StartOperations.Instance;
        gameManager.GameWin += GameWin;
        levelIndex = startOperations.gameDatas.LevelIndex;
        if (levelIndex > 3)
        {
            levelIndex = Random.Range(1, 4);
        }
        Instantiate(Resources.Load<GameObject>("Levels/Level" + levelIndex));
    }

    public void GameWin()
    {
        startOperations.gameDatas.LevelIndex++;
    }

}

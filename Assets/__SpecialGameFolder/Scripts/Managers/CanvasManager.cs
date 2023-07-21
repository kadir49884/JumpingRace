using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : Singleton<CanvasManager>
{


    [SerializeField] GameObject tapToStartPanel;
    [SerializeField] GameObject holdPanel;
    [SerializeField] GameObject gameWinPanel;
    [SerializeField] GameObject gameFailPanel;
    [SerializeField] TextMeshProUGUI activeLevelText;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] Image progressBarFill;
    [SerializeField] TextMeshProUGUI successText;

    private GameDatas gameDatas;

    [SerializeField] private List<TextMeshProUGUI> playersOrderTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> playersWinOrderTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> playersFailOrderTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<Color32> successTextColorList = new List<Color32>();



    private GameManager gameManager;

    public Image ProgressBarFill { get => progressBarFill; set => progressBarFill = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameWin += GameWin;
        gameManager.GameFail += GameFail;
        gameDatas = StartOperations.Instance.gameDatas;
        activeLevelText.text = gameDatas.LevelIndex.ToString();
        nextLevelText.text = (gameDatas.LevelIndex + 1).ToString();
    }

    public void TapToPlay()
    {
        GameManager.Instance.Initialize();
        GameManager.Instance.GameStart();
        tapToStartPanel.SetActive(false);
        holdPanel.SetActive(true);

    }

    public void SetProgressBarFill(float getFillValue)
    {
        DOTween.To(value => progressBarFill.fillAmount = value, progressBarFill.fillAmount, getFillValue, 0.2f);
    }

    public void GameWin()
    {
        OpeningObject(gameWinPanel);
        DOTween.To(value => progressBarFill.fillAmount = value, progressBarFill.fillAmount, 1, 0.5f);

    }
    public void GameFail()
    {
        OpeningObject(gameFailPanel);
    }

    private void OpeningObject(GameObject getObject)
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            getObject.SetActive(true);

        });
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayersOrder(List<OrderFinder> orderFinderList)
    {
        for (int i = 0; i < orderFinderList.Count; i++)
        {

            playersOrderTextList[i].text = orderFinderList[i].CharacterName;

            playersWinOrderTextList[i].text = orderFinderList[i].CharacterName;
            playersFailOrderTextList[i].text = orderFinderList[i].CharacterName;
        }

    }

    public void WriteSuccessText(float getDistance)
    {
        if (getDistance > 15)
        {
            successText.color = successTextColorList[2];
            successText.text = "LONG JUMP";
        }
        else if (getDistance < 0.45f)
        {
            successText.color = successTextColorList[0];
            successText.text = "PERFECT";
        }
        else
        {
            successText.color = successTextColorList[1];
            successText.text = "GOOD";
        }
        successText.gameObject.SetActive(true);

        DOVirtual.DelayedCall(1f, () =>
        {
            successText.gameObject.SetActive(false);
        });
    }



}


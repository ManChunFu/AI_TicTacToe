using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.Networking;
using System;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = default;
    [SerializeField] private Text playerName = default;
    [SerializeField] private Text userName = default;
    [SerializeField] private Text userScore = default;
    [SerializeField] private Text aiScore = default;
    [SerializeField] private GameObject gameOverPanel = default;
    [SerializeField] private Text displayResult = default;

    private bool flip;
    private int currentUserScore, currentAIScore = 0;
    private AI ai = new AI();
    private void Awake()
    {
        Assert.IsNotNull(menuPanel, "No reference to MenuPanel");
        Assert.IsNotNull(playerName, "No reference to Text(Name)");
        Assert.IsNotNull(userName, "No reference to Text(Username)");
        Assert.IsNotNull(userScore, "No reference to Text(UserScore)");
        Assert.IsNotNull(aiScore, "No reference to Text(aiScore)");
        Assert.IsNotNull(gameOverPanel, "No reference to Game Over Panel");
        Assert.IsNotNull(displayResult, "No reference to TEXT(DisplayResult");
    }

    private void Start()
    {
        StartCoroutine(ai.WakeUpAPI());

        if (!menuPanel.activeSelf)
            menuPanel.SetActive(true);

        if (gameOverPanel.activeSelf)
            gameOverPanel.SetActive(false);
    }

    public void FillInName()
    {
        if (userName && playerName)
            userName.text = playerName.text + " :";
    }

    // called by button click
    public void StartGame()
    {
        //FillInName();
        if (menuPanel)
            menuPanel.SetActive(false);
    }

    public void AddScore(GameOverTypes type)
    {
        switch (type)
        {
            case GameOverTypes.UserWon:
                currentUserScore++;
                userScore.text = currentUserScore.ToString();
                break;
            case GameOverTypes.AIWon:
                currentAIScore++;
                aiScore.text = currentAIScore.ToString();
                break;
            default:
                break;
        }


    }

    public IEnumerator SendScoreToAPI(Action onComplete = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("userScore", currentUserScore.ToString());
        form.AddField("aiScore", currentAIScore.ToString());
        using (var apiService = UnityWebRequest.Post("https://apiaiminimax.azurewebsites.net/api/scoreboard", form))
        {
            yield return apiService.SendWebRequest();
            if (apiService.isNetworkError || apiService.isHttpError)
                print(apiService.error);
            else onComplete?.Invoke();
        }
    }

    public void DisplayResult(GameOverTypes type)
    {
        switch (type)
        {
            case GameOverTypes.UserWon:
                displayResult.text = "You Win!";
                break;
            case GameOverTypes.AIWon:
                displayResult.text = "You Lose!";
                break;
            case GameOverTypes.Tie:
                displayResult.text = "Tie";
                break;
            default:
                break;
        }
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        displayResult.text = "";
        gameOverPanel.SetActive(false);
    }
}

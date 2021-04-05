using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScren : MonoBehaviour
{
    public Text getScore;

    private PlayFabManager playFabManager;

     void Start()
    {
        getScore = GameObject.Find("displayScore").GetComponent<Text>();
        getScore.GetComponent<Text>().text = ScoreManager.currentScore + "";
        playFabManager = GameObject.Find("PlayfabManager").GetComponent<PlayFabManager>();
        playFabManager.SendLeaderboard(DifficultyStatic.playfabScoreboard, ScoreManager.currentScore);
        Cursor.visible = true;
    }

    public void mainMenuButton(){
        SceneManager.LoadScene("MainMenu");
    }

    public void playAgainButton(){
        SceneManager.LoadScene("Main");
    }
}


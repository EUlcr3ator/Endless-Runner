using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins;
    public TextMeshProUGUI coins;
    private void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGameStarted = false;
        numberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);
    }

    private void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            PlayerPrefs.SetInt("NumberOfCoins", numberOfCoins);
        }
        coins.text = "Coins: " + numberOfCoins;
        //if (SwipeManager.tap)
        //{
        //    isGameStarted = true;
        //    Destroy(startingText);

        //}
    }
}

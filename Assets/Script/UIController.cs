using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // Initialisations
    public GameManager gameManager;
    public GameObject timerContainer;
    public GameObject endScreenContainer;
    public TMP_Text timerText;
    public TMP_Text timeTakenText;
    public TMP_Text openedPairText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTimer(float minutes, float seconds)
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void endGame(float timeTaken, int correct)
    {
        // hide timer and show game over screen
        timerContainer.SetActive(false);
        endScreenContainer.SetActive(true);

        // time taken
        int minutes = Mathf.FloorToInt(timeTaken / 60);
        int seconds = Mathf.FloorToInt(timeTaken % 60);
        timeTakenText.text = string.Format("Time spent: {0:00}:{1:00}", minutes, seconds);

        // correct
        openedPairText.text = $"Opened pair: {correct}";
    }

    public void startGame()
    {
        // show timer and hide game over screen
        timerContainer.SetActive(true);
        endScreenContainer.SetActive(false);

        // start game
        gameManager.SpawnTiles();
    }

    public void exitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public enum state {
        RUNNING,
        PAUSED,
        GAMEOVER
    }

    [Header("Reward Configuration")]
    public int rewardSatisfiedRide = 30;
    public int rewardInAttraction = 10;
    public int penaltyInQueue = -5;
    public int penaltyUnsatisfiedRide = -10;

    [Header("Game Phase Configuration")]
    public Color goodColor;
    public Color mediumColor;
    public Color badColor;
    public int mediumTopLimit = 70;
    public int mediumBottomLimit = 30;
    public int gameOverLimit = 25;

    [Header("NPC Configuration")]
    public int spawnAmount = 1;
    public int idleTimeMin = 5;
    public int idleTimeMax = 5;

    [Header("Player Configuration")]
    public float playerSpeed = 7;

    [Header("Level Configuration")]
    public int currentLevel = 1;
    public int baseVisitorsNeeded = 20;
    public float multiplierPerStage = 2;

    [Header("Game Information")]
    public state gameState = state.RUNNING;
    public float happinessPercentage;

    HUDManager hud;


    void Start()
    {
        hud = FindObjectOfType<HUDManager>();
    }

    public void StartGame()
    {
        gameState = state.RUNNING;
        StartCoroutine(LoadLevel("main"));
    }

    public void StopGame()
    {
        if(gameState == state.RUNNING || gameState == state.PAUSED)
        {
            StartCoroutine(LoadLevel("menu"));
        }
        else
        {
            Debug.Log("Wrong game state detected");
        }
    }

    public void PauseGame()
    {
        if(gameState == state.RUNNING)
        {
            hud.ShowPauseMenu();

            Scene active = SceneManager.GetActiveScene();
            GameObject[] roots = active.GetRootGameObjects();
            foreach (GameObject root in roots)
            {
                root.BroadcastMessage("onGamePaused", SendMessageOptions.DontRequireReceiver);
            }

            gameState = state.PAUSED;
        }
        else if (gameState == state.PAUSED)
        {
            Time.timeScale = 1;

            hud.HidePauseMenu();
            Scene active = SceneManager.GetActiveScene();
            GameObject[] roots = active.GetRootGameObjects();
            foreach (GameObject root in roots) {
                root.BroadcastMessage("onGameResumed", SendMessageOptions.DontRequireReceiver);
            }

            gameState = state.RUNNING;
        }
        else
        {
            Debug.Log("Wrong game state detected");
        }
    }

    public void CheckGameOver()
    {
        if(gameState == state.RUNNING)
        {
            if(happinessPercentage < gameOverLimit)
            {
                gameState = state.GAMEOVER;

                hud.ShowGameOverPanel();
            }
        }
    }

    IEnumerator LoadLevel(string levelName)
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public enum state {
        RUNNING,
        PAUSED,
        GAMEOVER,
        STOPPED
    }

    [Header("Reward Configuration")]
    public int rewardSatisfiedRide = 30;
    public int rewardInAttraction = 10;
    public int penaltyInQueue = -5;
    public float overfilledQueuePenaltyMultiplier = 2;
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
    public int gameOverCountdown = 5;
    int gameOverCountdownStore;
    [Header("Global Settings")]
    public Animation mainCameraAnimation;
    public Animator entranceAnimation;


    [Space(20)]
    [Header("Prefabs")]
    public GameObject offscreenAttractionIndicator;
    public GameObject happinessPopupTextPrefab;

    HUDManager hud;
    VisitorManager visitorManager;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
        hud = FindObjectOfType<HUDManager>();
        visitorManager = FindObjectOfType<VisitorManager>();
        GameStatistics.AddTotalNumberOfPlays();
        gameOverCountdown += 1; //Increase countdown by 1 to start at 5 and end by 0
        gameOverCountdownStore = gameOverCountdown;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            if(gameState == state.PAUSED)
            {
                Application.Quit();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PlayIntro()
    {
        mainCameraAnimation.Play();
        entranceAnimation.SetBool("isActive", true);
    }

    public IEnumerator StartGame()
    {
        Time.timeScale = 1;
        PlayIntro();
        yield return WaitForAnimation(mainCameraAnimation);

        gameState = state.RUNNING;
        Broadcast("OnGameStarted", SendMessageOptions.DontRequireReceiver);
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        gameState = state.STOPPED;
        StartCoroutine(LoadScene("main"));
    }

    public void StopGame()
    {
        Time.timeScale = 1;
        if(gameState == state.RUNNING || gameState == state.PAUSED || gameState == state.GAMEOVER)
        {
            gameState = state.STOPPED;
            StartCoroutine(LoadScene("menu"));
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
            Broadcast("OnGamePaused", SendMessageOptions.DontRequireReceiver);

            gameState = state.PAUSED;
        }
        else if (gameState == state.PAUSED)
        {
            Time.timeScale = 1;

            Broadcast("OnGameResumed", SendMessageOptions.DontRequireReceiver);

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

                gameOverCountdown -= 1;
                hud.ShowGameOverCountdown(gameOverCountdown);
                Debug.Log(gameOverCountdown + " to gameover");
                if (gameOverCountdown > 0) return;

                Broadcast("OnGameStopped", SendMessageOptions.DontRequireReceiver);
                gameState = state.GAMEOVER;
                hud.HideGameOverCountdown();
                hud.ShowGameOverPanel();
                OffscreenIndicator[] oi = FindObjectsOfType<OffscreenIndicator>();
                foreach(OffscreenIndicator o in oi)
                {
                    o.indicator.gameObject.SetActive(false);
                }
                GameStatistics.AddTotalNumberOfVisitors(visitorManager.allVisitors.Count);
            }else
            {
                gameOverCountdown = gameOverCountdownStore;
                hud.HideGameOverCountdown();
            }
        }
    }

    IEnumerator LoadScene(string levelName)
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);

        hud.ShowLoadingPanel();
        while (!async.isDone)
        {
            yield return null;
        }
        hud.HideLoadingPanel();
    }

    void Broadcast(string functionName, SendMessageOptions messageOptions)
    {
        Scene active = SceneManager.GetActiveScene();
        GameObject[] roots = active.GetRootGameObjects();
        foreach(GameObject root in roots)
        {
            root.BroadcastMessage(functionName, messageOptions);
        }
    }

    IEnumerator WaitForAnimation(Animation animation)
    {
        do
        {
            yield return null;
        } while(animation.isPlaying);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class HUDManager : MonoBehaviour
{
    GameController game;
    ScoreManager scoreManager;
    ScoreUploader externalScoreService;
    public AttractionController currentAttraction = null;

    public Animator UIAttractionStartAnim;
    public Animator UIAttractionControlAnim;

    [Header("Happiness")]
    public Slider happinessSlider;
    public Image happinessSliderFilling;
    public TextMeshProUGUI HappinessPercentageText;

    [Header("Waiting for Visitors Panel")]
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI freeSlotsText;
    public TextMeshProUGUI attractionNameText;

    [Header("Start/Stop Buttons")]
    public GameObject startButton;
    public GameObject stopButton;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Tutorial")]
    public Animator UITutorialAnim;

    [Header("Alert Message")]
    public Animator alertPanelAnimator;
    public TextMeshProUGUI alertMessageText;
    public List<string> alertsQueue = new List<string>();
    public float alertDuration = 4;
    bool alertVisible = false;
    // Use this for initialization

    [Header("Pause Settings")]
    public Sprite pauseIcon;
    public Sprite resumeIcon;
    public Image pauseButtonBackground;
    public Animator pausePanelAnimator;

    [Header("GameOver")]
    public TextMeshProUGUI gameOverTimeText;
    public TextMeshProUGUI gameOverHighscoreText;
    public Animator gameOverPanelAnimator;
    public GameObject nameInputPanel;
    public TMP_InputField nameInput;
    public Button submitHighscoreButton;

    [Header("Sound")]
    public AudioClip[] ansageSoundArray;
    public AudioSource effectsAudioSource;
    public AudioSource musicAudioSource;
    public AudioClip mainTheme, gameOverTheme;

    [Header("Misc")]
    public GameObject loadingPanel;


    void Start()
    {
        game = FindObjectOfType<GameController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        externalScoreService = FindObjectOfType<ScoreUploader>();
        waitingText.text = "0";
        musicAudioSource.clip = mainTheme;
        musicAudioSource.Play();

        loadingPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(currentAttraction != null)
        {
            waitingText.text = currentAttraction.npcsWaiting.Count.ToString();
            freeSlotsText.text = currentAttraction.npcsActive.Count.ToString() + " / " + currentAttraction.npcAmount.ToString();

            if(currentAttraction.running)
            {
                startButton.SetActive(false);
                stopButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(true);
                stopButton.SetActive(false);
            }
        }

        AlertUpdate();

    }

    #region Alert
    void AlertUpdate() {
        if (alertsQueue.Count > 0 && !alertVisible) {
            AlertPopup();
        }
    }
    public void ShowAlert(string _text) {

        alertsQueue.Add(_text);

    }
    public void AlertPopup() {
        StartCoroutine(AnimateAlert());
    }
    IEnumerator AnimateAlert() {
        alertPanelAnimator.SetBool("isActive", true);
        alertVisible = true;
        alertMessageText.text = alertsQueue[0];
        alertsQueue.RemoveAt(0); //Remove from queue
        yield return new WaitForSeconds(alertDuration);
        alertVisible = false;
        alertPanelAnimator.SetBool("isActive", false);
    }
    #endregion

    public void PlayAttraction()
    {
        if(currentAttraction == null)
        {
            return;
        }

        if(!currentAttraction.running)
        {
            bool success = currentAttraction.StartAttraction();

            if(success)
            {
                ButtonCooldown cooldown = stopButton.GetComponent<ButtonCooldown>();
                cooldown.cooldown = (float)currentAttraction.duration;
                cooldown.cooldownRunning = true;

                PlayRandomAnsage();

                //UIAttractionControlAnim.SetBool("isOpen", true);
                //UIAttractionStartAnim.SetBool("isOpen", false);
            }
        }

    }

    public void StopAttraction()
    {
        if(currentAttraction == null)
        {
            return;
        }

        if (currentAttraction.running)
        {
            currentAttraction.StopAttraction();

            ButtonCooldown cooldown = stopButton.GetComponent<ButtonCooldown>();
            cooldown.cooldownRunning = false;
        }
    }

    public void SetupCurrentAttraction(AttractionController _attraction)
    {
        currentAttraction = _attraction;
        attractionNameText.text = _attraction.attractionName;

        UIAttractionStartAnim.SetBool("isOpen", true);
    }

    public void LeaveAttraction()
    {
        currentAttraction = null;
        attractionNameText.text = "";
        //UIAttractionControlAnim.SetBool("isOpen", false);
        UIAttractionStartAnim.SetBool("isOpen", false);
    }

    public void Ansage()
    {
        currentAttraction.Ansage();
        UIAttractionControlAnim.SetBool("isOpen", true);
        UIAttractionStartAnim.SetBool("isOpen", false);
    }

    public void Special()
    {
        currentAttraction.Special();
    }

    public void Notstop()
    {
        StopAttraction();
        currentAttraction.Notstop();
        //UIAttractionControlAnim.SetBool("isOpen", false);
        //UIAttractionStartAnim.SetBool("isOpen", true);
    }

    public void UpdateHappiness(float _val)
    {
        //Calculate new percentage scale
        float a = _val-game.gameOverLimit;
        float b = 100 - game.gameOverLimit;
        
        float visibleValue = Mathf.Clamp(a*100/b,0,100);

        
        happinessSlider.value = visibleValue;
        if (visibleValue > game.mediumTopLimit)
        {
            happinessSliderFilling.color = game.goodColor;
        }
        else if (visibleValue < game.mediumBottomLimit)
        {
            happinessSliderFilling.color = game.badColor;
        }
        else
        {
            happinessSliderFilling.color = game.mediumColor;
        }
        HappinessPercentageText.text = visibleValue.ToString("N0") + "%";
    }

    public void PlayRandomAnsage()
    {
        effectsAudioSource.clip = ansageSoundArray[Random.Range(0, ansageSoundArray.Length)];
        effectsAudioSource.Play();
    }

    public void StopGame()
    {
        game.StopGame();
    }

    public void RestartGame()
    {
        game.LoadGame();
    }

    public void StartGame()
    {
        UITutorialAnim.SetBool("isActive", false);
        StartCoroutine(game.StartGame());
    }

    public void OnGamePaused()
    {
        pauseButtonBackground.sprite = resumeIcon;
    }

    public void OnGameResumed()
    {
        pauseButtonBackground.sprite = pauseIcon;
    }

    public void ShowGameOverPanel()
    {
        musicAudioSource.clip = gameOverTheme;
        musicAudioSource.Play();
        scoreManager.GameOver();
        if(scoreManager.GetScoreString() == scoreManager.GetHighscoreString())
        {
            gameOverTimeText.SetText(string.Format("Neuer Highscore: {0}", scoreManager.GetHighscoreString()));
            gameOverHighscoreText.gameObject.SetActive(false);
            nameInputPanel.gameObject.SetActive(true);
            nameInput.onValueChanged.AddListener(OnHighscoreNameInputChange);

            if(PlayerPrefsConstants.GetHighscorePlayerName() != string.Empty &&
                PlayerPrefsConstants.GetHighscorePlayerName().Length >= 3)
            {
                nameInput.text = PlayerPrefsConstants.GetHighscorePlayerName();
                submitHighscoreButton.interactable = true;
            }
            submitHighscoreButton.onClick.AddListener(OnSubmitHighscore);
        }
        else
        {
            nameInputPanel.gameObject.SetActive(false);
            gameOverHighscoreText.gameObject.SetActive(true);
            gameOverTimeText.SetText(string.Format("Du hast {0} durchgehalten!", scoreManager.GetScoreString()));
            gameOverHighscoreText.SetText(string.Format("Dein Highscore: {0}", scoreManager.GetHighscoreString()));
        }

        gameOverPanelAnimator.SetBool("isActive", true);
    }

    void OnHighscoreNameInputChange(string _text)
    {
        if(nameInput.text.Trim().Length <= 3)
        {
            submitHighscoreButton.interactable = false;
        }else
        {
            submitHighscoreButton.interactable = true;
        }
    }

    void OnSubmitHighscore()
    {
        string name = nameInput.text.Trim();
        if (name.Length <= 3)return;

        int score = scoreManager.GetHighscoreTimeInSeconds();
        PlayerPrefsConstants.SetHighscorePlayerName(name);
        StartCoroutine(externalScoreService.PostScores(name, score));
    }

    public void ShowPauseMenu()
    {
        pausePanelAnimator.SetBool("isActive", true);
    }

    public void HidePauseMenu()
    {
        pausePanelAnimator.SetBool("isActive", false);
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}

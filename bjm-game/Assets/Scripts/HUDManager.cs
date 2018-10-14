using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour {
    GameController game;

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
    public TextMeshProUGUI gameOverText;
    public Animator gameOverPanelAnimator;

    [Header("Sound")]
    public AudioClip[] ansageSoundArray;

    void Start() {
        game = FindObjectOfType<GameController>();

        waitingText.text = "0";

    }

    private void FixedUpdate() {
        if (currentAttraction != null) {
            waitingText.text = currentAttraction.npcsWaiting.Count.ToString();
            freeSlotsText.text = currentAttraction.npcsActive.Count.ToString() + " / " + currentAttraction.npcAmount.ToString();

            if (currentAttraction.running) {
                startButton.SetActive(false);
                stopButton.SetActive(true);
            } else {
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
    public void PlayAttraction() {
        if (currentAttraction == null) return;

        if (!currentAttraction.running) {
            bool success = currentAttraction.StartAttraction();

            if (success) {
                ButtonCooldown cooldown = stopButton.GetComponent<ButtonCooldown>();
                cooldown.cooldown = (float)currentAttraction.duration;
                cooldown.cooldownRunning = true;

                PlayRandomAnsage();

                //UIAttractionControlAnim.SetBool("isOpen", true);
                //UIAttractionStartAnim.SetBool("isOpen", false);
            }
        }

    }
    public void StopAttraction() {
        if (currentAttraction == null) return;
        if (currentAttraction.running) {
            currentAttraction.StopAttraction();

            ButtonCooldown cooldown = stopButton.GetComponent<ButtonCooldown>();
            cooldown.cooldownRunning = false;
        }
    }

    public void SetupCurrentAttraction(AttractionController _attraction) {
        currentAttraction = _attraction;
        attractionNameText.text = _attraction.attractionName;
        /*if (_attraction.running) {
            _attraction.StopAttraction();
        } else {
            _attraction.StartAttraction();
        }*/
        UIAttractionStartAnim.SetBool("isOpen", true);
    }
    public void LeaveAttraction() {
        currentAttraction = null;
        attractionNameText.text = "";
        //UIAttractionControlAnim.SetBool("isOpen", false);
        UIAttractionStartAnim.SetBool("isOpen", false);
    }

    public void Ansage() {
        currentAttraction.Ansage();
        UIAttractionControlAnim.SetBool("isOpen", true);
        UIAttractionStartAnim.SetBool("isOpen", false);
    }

    public void Special() {
        currentAttraction.Special();
    }

    public void Notstop() {
        StopAttraction();
        currentAttraction.Notstop();
        //UIAttractionControlAnim.SetBool("isOpen", false);
        //UIAttractionStartAnim.SetBool("isOpen", true);
    }



    public void UpdateHappiness(float _val) {

        //Calculate new percentage scale
        float a = _val-game.gameOverLimit;
        float b = 100 - game.gameOverLimit;
        
        float visibleValue = a*100/b;

        
        happinessSlider.value = visibleValue;
        if (visibleValue > game.mediumTopLimit) {
            happinessSliderFilling.color = game.goodColor;
        } else if (visibleValue < game.mediumBottomLimit) {
            happinessSliderFilling.color = game.badColor;
        } else {
            happinessSliderFilling.color = game.mediumColor;
        }
        HappinessPercentageText.text = visibleValue.ToString("N0") + "%";

    }

    public void PlayRandomAnsage() {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = ansageSoundArray[Random.Range(0, ansageSoundArray.Length)];
        audio.Play();
    }

    public void NavigateToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("menu");
    }

    public void NavigateToMain() {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("main");
    }

    public void SkipTutorial() {
        UITutorialAnim.SetBool("isActive", false);
    }

    public void onGamePaused() {
        pauseButtonBackground.sprite = resumeIcon;
    }

    public void onGameResumed() {
        pauseButtonBackground.sprite = pauseIcon;
    }

    public void ShowGameOverPanel() {
        gameOverText.SetText("Du hast " + scoreText.text + " durchgehalten!");
        gameOverPanelAnimator.SetBool("isActive", true);
    }

    public void ShowPauseMenu() {
        pausePanelAnimator.SetBool("isActive", true);
    }

    public void HidePauseMenu() {
        pausePanelAnimator.SetBool("isActive", false);
    }
}

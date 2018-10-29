using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.PostProcessing;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public PlayableDirector timelineCredits;
    public Camera cam;
    public bool isInCredits = false;
    public GameObject loadingPanel;
    public Canvas canvasMain;
    public Canvas canvasCredits;
    public Animator settingsPanelAnim;

    [Header("Settings")]

    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider announcerSlider;

    [Header("Highscore")]
    public GameObject highscorePage;
    public TextMeshProUGUI highscoreTextPos;
    public TextMeshProUGUI highscoreTextNames;
    public TextMeshProUGUI highscoreTextScores;
    public TextMeshProUGUI highscoreTextConnection;
    public TextMeshProUGUI highscoreTextLoading;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        loadingPanel.SetActive(false);
        canvasMain.gameObject.SetActive(true);
        canvasCredits.gameObject.SetActive(false);

        SetupSettings();
#if UNITY_ANDROID
        if (cam == null) cam = FindObjectOfType<Camera>();
        if (cam.GetComponent<PostProcessingBehaviour>() != null && cam != null)
        {
            cam.GetComponent<PostProcessingBehaviour>().enabled = false;
        }
#endif
    }

    void Update()
    {
        if(isInCredits && timelineCredits.state != PlayState.Playing)
        {
            CreditsEnd();
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnButtonStart()
    {
        StartCoroutine(LoadLevel("main"));
    }

    public void OnButtonSettings()
    {
        settingsPanelAnim.SetBool("isActive", true);
    }

    public void OnButtonSettingsSave()
    {
        SaveSettings();
        settingsPanelAnim.SetBool("isActive", false);
    }

    public void OnButtonCredits()
    {
        isInCredits = true;
        timelineCredits.Play();
        canvasMain.gameObject.SetActive(false);
        canvasCredits.gameObject.SetActive(true);
    }

    public void CreditsEnd()
    {
        SceneManager.LoadSceneAsync("menu");
    }

    IEnumerator LoadLevel(string levelName)
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);

        loadingPanel.SetActive(true);
        while (!async.isDone)
        {
            yield return null;
        }
        loadingPanel.SetActive(false);
    }

    IEnumerator RefreshHighScoreTable()
    {
        highscoreTextLoading.enabled = true;
        highscoreTextConnection.enabled = false;
        CoroutineWithData cd = new CoroutineWithData(this, ScoreUploader.GetScores());
        yield return cd.coroutine;
        List<string[]> highscoreTable = (List<string[]>)cd.result;

        highscoreTextPos.text = "";
        highscoreTextNames.text = "";
        highscoreTextScores.text = "";
        highscoreTextLoading.enabled = false;
        if (highscoreTable.Count == 0)
        {
            highscoreTextConnection.enabled = true;
        }
        else
        {
            highscoreTextConnection.enabled = false;

            for (int i = 0; i < highscoreTable.Count; ++i)
            {
                highscoreTextPos.text += (i + 1).ToString() + "\n";
                highscoreTextNames.text += highscoreTable[i][0] + "\n";
                highscoreTextScores.text += highscoreTable[i][1] + "\n";
            }
        }

        
    }

    public void ShowHighScores()
    {
        highscorePage.SetActive(true);
        StartCoroutine(RefreshHighScoreTable());
    }

    public void HideHighScores()
    {
        highscorePage.SetActive(false);
    }

    #region Settings
    void SetupSettings()
    {
        sfxSlider.value = PlayerPrefsConstants.GetSFXVolume()*100;
        musicSlider.value = PlayerPrefsConstants.GetMusicVolume() * 100;
        announcerSlider.value = PlayerPrefsConstants.GetAnnouncerVolume() * 100;
    }

    void SaveSettings()
    {
        float sfxVal = sfxSlider.value / 100;
        float musicVal = musicSlider.value / 100;
        float announcerVal = announcerSlider.value / 100;

        PlayerPrefsConstants.SetSFXVolume(sfxVal);
        PlayerPrefsConstants.SetMusicVolume(musicVal);
        PlayerPrefsConstants.SetAnnouncerVolume(announcerVal);

        if (FindObjectOfType<SoundSource>())
        {
            SoundSource[] allSoundSources = FindObjectsOfType<SoundSource>();
            foreach(SoundSource s in allSoundSources)
            {
                s.SetupSound();
            }
        }
    }

    #endregion
}

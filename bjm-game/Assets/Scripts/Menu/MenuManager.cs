using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MenuManager : MonoBehaviour
{
    public PlayableDirector timelineCredits;
    public bool isInCredits = false;

    public GameObject loadingPanel;

    public Canvas canvasMain;
    public Canvas canvasCredits;

    public Animator settingsPanelAnim;

    private int nextUpdate = 1;

    private void Start() {
        loadingPanel.SetActive(false);
        canvasMain.gameObject.SetActive(true);
        canvasCredits.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isInCredits && timelineCredits.state != PlayState.Playing)
        {
            creditsEnd();
        }
    }

    public void buttonStart() {
        StartCoroutine(LoadLevel());
    }

    public void buttonSettings()
    {
        settingsPanelAnim.SetBool("isActive", true);
    }

    public void buttonSettingsSave()
    {
        settingsPanelAnim.SetBool("isActive", false);
    }

    public void buttonCredits()
    {
        isInCredits = true;
        timelineCredits.Play();
        canvasMain.gameObject.SetActive(false);
        canvasCredits.gameObject.SetActive(true);
    }

    public void creditsEnd()
    {
        SceneManager.LoadSceneAsync(0);
    }

    IEnumerator LoadLevel() {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        int cnt = 0;
        loadingPanel.SetActive(true);
        while (!async.isDone) {
            if (Time.time >= nextUpdate) {
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;

                if (cnt == 2) {
                    cnt = 0;
                } else {
                    ++cnt;
                }
            }

            yield return null;
        }

        loadingPanel.SetActive(false);
    }
}

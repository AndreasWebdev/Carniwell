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

    public GameObject loadingPanel;

    private int nextUpdate = 1;

    private void Start() {
        loadingPanel.SetActive(false);
    }

    void buttonStart() {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        int cnt = 0;
        loadingPanel.SetActive(true);
        while (!async.isDone) {
            if (Time.time >= nextUpdate) {
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                updateLoadingPanel(cnt);

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

    void updateLoadingPanel(int state) {
        Text loadingText = loadingPanel.GetComponentInChildren<Text>();
        switch (state) {
            case 0:
                loadingText.text = "Loading page .";
                break;
            case 1:
                loadingText.text = "Loading page ..";
                break;
            case 2:
                loadingText.text = "Loading page ...";
                break;
            default:
                // Nothing to do
                break;
        }
    }
}

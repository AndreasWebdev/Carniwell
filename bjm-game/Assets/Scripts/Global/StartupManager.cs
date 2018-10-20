// Reference: https://unity3d.com/de/learn/tutorials/topics/scripting/localization-manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartupManager : MonoBehaviour
{
    // Use this for initialization


        void Start()
        {
        StartCoroutine(StartCo());
        }
    IEnumerator StartCo()
    {
        StartCoroutine(InitLocalization());
        while(!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }

        StartCoroutine(LoadLevel("menu"));
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

    IEnumerator InitLocalization()
    {
        if(Application.systemLanguage == SystemLanguage.German)
        {
            LocalizationManager.instance.LoadLocalizedText("localizedText_de.json");
            yield return null;
        }
        else
        {
            LocalizationManager.instance.LoadLocalizedText("localizedText_en.json");
            yield return null;
        }
    }
}

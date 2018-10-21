// Reference: https://unity3d.com/de/learn/tutorials/topics/scripting/localization-manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartupManager : MonoBehaviour
{

        public enum Language { English,German}
        [Header("Debug only")]
        public Language loadLanguage;


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
#if !UNITY_EDITOR
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
#endif
#if UNITY_EDITOR
        if (loadLanguage == Language.German)
        {
            LocalizationManager.instance.LoadLocalizedText("localizedText_de.json");
            yield return null;
        }
        else if(loadLanguage == Language.English)
        {
            LocalizationManager.instance.LoadLocalizedText("localizedText_en.json");
            yield return null;
        }
#endif
    }
}

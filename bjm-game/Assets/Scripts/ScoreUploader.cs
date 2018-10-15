using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreUploader : MonoBehaviour
{
    public string webBaseURL;
    public string scoreURL;

	void Start ()
    {

	}


	void Update ()
    {

	}


    IEnumerator UploadHighscoreToWeb()
    {
        string parameter = "";
        WWW url = new WWW(webBaseURL + scoreURL + parameter/*WEB URL + PARAMETER*/); //Wenn wir über php gehen
        yield return url;

        if (!string.IsNullOrEmpty(url.error))
        {
            Debug.Log("ScoreUploader:: Error from " + webBaseURL + ": " + url.error);
        }


    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreUploader : MonoBehaviour
{
    string secretKey = "62HUYi5xq|k>Q2mqr$-2jJHmH0,#$)"; 
    static string addScoreURL = "http://kindaworking.de/carniwell/addscore.php?";
    static string highscoreURL = "http://kindaworking.de/carniwell/display.php";


    void Start()
    {
        //StartCoroutine(GetScores());
    }

    // remember to use StartCoroutine when calling this function!
    public IEnumerator PostScores(string name, float score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Utility.Md5Sum(name + score + secretKey);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done


        if(hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
            yield return 0;
        }
        else
        {
            Debug.Log(hs_post.text);
            yield return 1;
        }
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    public static IEnumerator GetScores()
    {
        List<string[]> result = new List<string[]>();

        WWW hs_get = new WWW(highscoreURL + "?limit=" + 10);
        yield return hs_get;

        if(hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            string scores = hs_get.text;
            string[] entries = scores.Split('\n');

            foreach(string entry in entries)
            {
                if(entry != "")
                {
                    string[] entryInfo = entry.Split('\t');
                    if(entryInfo.Length == 2)
                    {
                        result.Add(entryInfo);
                    }
                }
            }
        }

        yield return result;
    }

}

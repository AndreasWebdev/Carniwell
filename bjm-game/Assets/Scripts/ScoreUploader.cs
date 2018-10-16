using UnityEngine;
using System.Collections;

public class ScoreUploader : MonoBehaviour
{
    private string secretKey = "62HUYi5xq|k>Q2mqr$-2jJHmH0,#$)"; 
    private string addScoreURL = "http://kindaworking.de/carniwell/addscore.php?";
    private string highscoreURL = "http://kindaworking.de/carniwell/display.php";

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
            print("There was an error posting the high score: " + hs_post.error);
        }
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
        WWW hs_get = new WWW(highscoreURL + "?limit=" + 10);
        yield return hs_get;

        if(hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
          //Show Scores
        }
    }

}

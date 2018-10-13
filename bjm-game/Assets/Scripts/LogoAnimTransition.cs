using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoAnimTransition : MonoBehaviour {
   

	// Use this for initialization
	void Start () {
        StartCoroutine(Transition());
	}
	
	IEnumerator Transition()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("main");
    }
}

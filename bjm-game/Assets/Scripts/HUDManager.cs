using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    public AttractionController currentAttraction = null;

    public Animator bottomHudAnim;
	// Use this for initialization
	void Start () {
		
	}
	
    public void SetupCurrentAttraction(AttractionController _attraction)
    {
        currentAttraction = _attraction;
        _attraction.StartAttraction();
        bottomHudAnim.SetBool("isOpen", true);
    }
    public void LeaveAttraction()
    {
        currentAttraction = null;
        bottomHudAnim.SetBool("isOpen", false);
    }

	public void Ansage()
    {
        currentAttraction.Ansage();
    }

    public void Special()
    {
        currentAttraction.Special();
    }

    public void Notstop()
    {
        currentAttraction.Notstop();
    }
}

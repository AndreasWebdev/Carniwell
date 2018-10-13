using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour {
    public AttractionController currentAttraction = null;

    public Animator UIAttractionStartAnim;
    public Animator UIAttractionControlAnim;

    public Slider happinessSlider;
    public Image happinessSliderFilling;
    public Color goodColor, mediumColor, badColor;


    public TextMeshProUGUI waitingText;
	// Use this for initialization
	void Start () {
        waitingText.text = "0";

    }

    private void FixedUpdate()
    {
        
        if (currentAttraction != null)
        {
            waitingText.text = currentAttraction.npcsWaiting.Count.ToString();
        }
        
    }

    public void PlayAttraction()
    {
        if (currentAttraction == null) return;

        if (currentAttraction.running)
        {
           currentAttraction.StopAttraction();
        }
        else
        {
           currentAttraction.StartAttraction();
        }
    }

    public void SetupCurrentAttraction(AttractionController _attraction)
    {
        currentAttraction = _attraction;
        /*if (_attraction.running) {
            _attraction.StopAttraction();
        } else {
            _attraction.StartAttraction();
        }*/
        UIAttractionStartAnim.SetBool("isOpen", true);
    }
    public void LeaveAttraction()
    {
        currentAttraction = null;
        UIAttractionStartAnim.SetBool("isOpen", false);
    }

	public void Ansage()
    {
        currentAttraction.Ansage();
        UIAttractionControlAnim.SetBool("isOpen", true);
        UIAttractionStartAnim.SetBool("isOpen", false);
    }

    public void Special()
    {
        currentAttraction.Special();
    }

    public void Notstop()
    {
        currentAttraction.Notstop();
        UIAttractionControlAnim.SetBool("isOpen", false);
        UIAttractionStartAnim.SetBool("isOpen", true);
    }



    public void UpdateHappiness(float _val)
    {
        
        happinessSlider.value = _val;
        if (_val >= 70)
        {
            happinessSliderFilling.color = goodColor;
        }else if(_val < 70 && _val > 30)
        {
            happinessSliderFilling.color = mediumColor;
        }else
        {
            happinessSliderFilling.color = badColor;
        }
    }
}

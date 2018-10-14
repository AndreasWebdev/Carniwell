using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour {
    public AttractionController currentAttraction = null;

    public Animator UIAttractionStartAnim;
    public Animator UIAttractionControlAnim;

    [Header("Happiness")]
    public Slider happinessSlider;
    public Image happinessSliderFilling;
    public Color goodColor, mediumColor, badColor;
    public TextMeshProUGUI HappinessPercentageText;

    [Header("Waiting for Visitors Panel")]
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI freeSlotsText;
    public TextMeshProUGUI attractionNameText;

    [Header("Start/Stop Buttons")]
    public GameObject startButton;
    public GameObject stopButton;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Alert Message")]
    public TextMeshProUGUI alertMessageText;
    public List<string> alertsQueue;
    public float alertDuration = 4;
    public float alertDurationStore;
    // Use this for initialization
    

    [Header("Sound")]
    public AudioClip[] ansageSoundArray;

    void Start()
    {
        waitingText.text = "0";

    }

    private void FixedUpdate()
    {
        if (currentAttraction != null) {
            waitingText.text = currentAttraction.npcsWaiting.Count.ToString();
            freeSlotsText.text = currentAttraction.npcsActive.Count.ToString() + "/" + currentAttraction.npcAmount.ToString() + "\n Sitze frei";

            if (currentAttraction.running) {
                startButton.SetActive(false);
                stopButton.SetActive(true);
            } else {
                startButton.SetActive(true);
                stopButton.SetActive(false);
            }
        }
        if(alertsQueue.Count > 0)
        {

        }
    }

    public void ShowAlert(string _text)
    {

        alertsQueue.Add(_text);

    }
    public void AlertPopup()
    {
        
        alertMessageText.text = alertsQueue[0];
        alertsQueue.RemoveAt(0);
        
    }

    public void PlayAttraction()
    {
        if (currentAttraction == null) return;

        if (!currentAttraction.running)
        {
            bool success = currentAttraction.StartAttraction();

            if (success) {
                ButtonCooldown cooldown = stopButton.GetComponent<ButtonCooldown>();
                cooldown.cooldown = (float)currentAttraction.duration;
                cooldown.cooldownRunning = true;

                PlayRandomAnsage();

                //UIAttractionControlAnim.SetBool("isOpen", true);
                //UIAttractionStartAnim.SetBool("isOpen", false);
            }
        } 
        
    }
    public void StopAttraction()
    {
        if (currentAttraction == null) return;
        if (currentAttraction.running)
        {
            currentAttraction.StopAttraction();

            ButtonCooldown cooldown = stopButton.GetComponent<ButtonCooldown>();
            cooldown.cooldownRunning = false;
        }
    }

    public void SetupCurrentAttraction(AttractionController _attraction)
    {
        currentAttraction = _attraction;
        attractionNameText.text = _attraction.attractionName;
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
        attractionNameText.text = "";
        //UIAttractionControlAnim.SetBool("isOpen", false);
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
        StopAttraction();
        currentAttraction.Notstop();
        //UIAttractionControlAnim.SetBool("isOpen", false);
        //UIAttractionStartAnim.SetBool("isOpen", true);
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
        HappinessPercentageText.text = _val.ToString("N0") + "%";
    }

    public void PlayRandomAnsage()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = ansageSoundArray[Random.Range(0, ansageSoundArray.Length)];
        audio.Play();
    }

    
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class AttractionController : MonoBehaviour
{


    public int id = 0;
    public string attractionName;

    public Transform entrancePosition;
    // Duration of one run
    public double duration = 120;
    public int npcAmount = 10;

    public bool running = false;
    double timeLeft;
    [HideInInspector]
    public int completedRides = 0; //Wie oft ist diese Attraktion bereits gefahren

    int nextUpdate = 1;
    bool isAnimation = false;

    public List<GameObject> npcsActive = new List<GameObject>();
    public List<GameObject> npcsWaiting = new List<GameObject>();

    AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip failureSound;


    GameController game;
    HUDManager hud;
    VisitorManager visitorMangager;

    public TextMeshPro waitingCountText;

    OffscreenIndicator myIndicator;
    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<GameController>();
        timeLeft = duration;
        hud = FindObjectOfType<HUDManager>();
        visitorMangager = FindObjectOfType<VisitorManager>();
        if (gameObject.GetComponent<Animator>())
        {
            isAnimation = true;
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        
        transform.localEulerAngles = Vector3.zero;
        waitingCountText.transform.parent.eulerAngles = Vector3.zero;

        GameObject indicator = Instantiate(game.offscreenAttractionIndicator);
        myIndicator = indicator.GetComponent<OffscreenIndicator>();
        myIndicator.SetTarget(this.transform);
        myIndicator.transform.SetParent(this.transform, false);
        myIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        HandleFloatingWaitingText();

#if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("Attraction started");

            StartAttraction();
        } else if(Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("Attraction stopped");

            hud.ShowAlert(
                LocalizationManager.instance != null ? LocalizationManager.instance.GetLocalizedValue("alert_stopped_attraction") : "Attraktion gestoppt!"
                );
            StopAttraction();
        }
#endif
    }

    void HandleFloatingWaitingText()
    {
        waitingCountText.text = npcsWaiting.Count.ToString("00");
        if (npcsWaiting.Count > npcAmount)
        {
            waitingCountText.color = Color.red;
            myIndicator.gameObject.SetActive(true);
        }
        else
        {
            waitingCountText.color = Color.black;
            myIndicator.gameObject.SetActive(false);
        }
    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        if(running) {
            if(timeLeft > 0)
            {
                --timeLeft;
            } else
            {
                StopAttraction();
                // ToDo: Send Attraction stopped signal
            }
        }
    }

    public void AddNPCToQueue(GameObject npc)
    {
        npcsWaiting.Add(npc);
        NPC npcScript = npc.GetComponent<NPC>();
        npcScript.SetStatus(NPC.status.QUEUE);
    }

    public bool StartAttraction()
    {
        if(!running)
        {
            running = true;

            // Move waiting NPCs to attraction
            for(int i = 0; i < npcAmount; ++i)
            {
                if(npcsWaiting.Count == 0)
                {
                    break;
                }
                GameObject npc = npcsWaiting[0];
                npcsWaiting.Remove(npc);
                NPC npcScript = npc.GetComponent<NPC>();
                npcScript.SetStatus(NPC.status.ATTRACTION);

                npcsActive.Add(npc);
            }

            // Only start attraction if there are some NPCS who wanna drive
            if(npcsActive.Count > 0)
            {

                // Lock player while running
                PlayerMovement player = FindObjectOfType<PlayerMovement>();
                if(player)
                {
                    player.LockMovement();
                }

                // Start attraction
                timeLeft = duration;
                StartAnimation();
            } else
            {
                // TODO: Throw error message - No NPCS
                Debug.Log("No NPCs available");

                hud.ShowAlert(
                    LocalizationManager.instance != null ? LocalizationManager.instance.GetLocalizedValue("alert_no_one_in_queue") : "Keiner in der Warteschlange."
                    );

                running = false;
            }
        }

        return running;
    }

    public void StopAttraction()
    {
        if(running)
        {
            running = false;
            StopAnimation();

            bool aborted = false;

            // Abort or regular stop?
            if(timeLeft > 0)
            {
                aborted = true;
            }

            PlaySound(aborted);

            // Unlock player
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if(player)
            {
                player.UnlockMovement();
            }

            if (!aborted)
            {
                if (game.happinessPopupTextPrefab != null)
                {
                    float gainedHappiness = npcsActive.Count * game.rewardSatisfiedRide;
                    GameObject popup = (GameObject)Instantiate(game.happinessPopupTextPrefab);
                    popup.transform.position = entrancePosition.transform.position + (Vector3.up);
                    popup.transform.SetParent(entrancePosition.transform);
                    popup.GetComponent<TMPro.TextMeshPro>().text = "+" + gainedHappiness.ToString("N0");
                }
                completedRides++;
                GameStatistics.AddTotalNumberOfSatisfiedVisitors(npcsActive.Count);
                GameStatistics.AddTotalNumberOfDrivenAttractions(1);
            }else
            {
                GameStatistics.AddTotalNumberOfStoppedAttractions(1);
            }

            while (npcsActive.Count > 0)
            {
                GameObject npc = npcsActive[0];
                npcsActive.Remove(npc);
                NPC npcScript = npc.GetComponent<NPC>();

                npcScript.SetStatus(NPC.status.IDLE);
                if(aborted)
                {
                    npcScript.UpdateHappiness(game.penaltyUnsatisfiedRide);
                } else
                {
                    npcScript.UpdateHappiness(game.rewardSatisfiedRide);
                    Debug.Log("AttractionController::Done Attraction, giving visitors happiness");

                }
                if(timeLeft <= 0)
                {
                    npcScript.DoneAttraction();
                }
            }

            visitorMangager.CalculateHappiness();
        }
    }

    void StartAnimation()
    {
        if(isAnimation)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            if(anim)
            {
                anim.SetBool("active", true);
                anim.speed = 1.0f;
            }
        } else
        {
            gameObject.BroadcastMessage("Activate");
        }
    }

    void StopAnimation()
    {
        if(isAnimation)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            if(anim)
            {
                anim.SetBool("active", false);
               // anim.speed = 4.0f;
            }
        } else
        {
            gameObject.BroadcastMessage("Deactivate");
        }
    }

    void PlaySound(bool aborted)
    {
        if(aborted)
        {
            audioSource.PlayOneShot(failureSound, 0.8f);
        }
        else
        {
            audioSource.PlayOneShot(successSound, 0.3f);
        }
    }
}

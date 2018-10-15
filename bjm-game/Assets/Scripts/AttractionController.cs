using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class AttractionController : MonoBehaviour
{
    GameController game;

    public int id = 0;
    public string attractionName;

    public Transform entrancePosition;
    // Duration of one run
    public double duration = 120;
    public int npcAmount = 10;

    public bool running = false;
    double timeLeft;

    int nextUpdate = 1;
    bool isAnimation = false;

    public List<GameObject> npcsActive = new List<GameObject>();
    public List<GameObject> npcsWaiting = new List<GameObject>();

    AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip failureSound;

    HUDManager hud;

    public TextMeshPro waitingCountText;


    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<GameController>();
        waitingCountText = GetComponentInChildren<TextMeshPro>();
        timeLeft = duration;
        hud = FindObjectOfType<HUDManager>();
        if(gameObject.GetComponent<Animator>())
        {
            isAnimation = true;
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
        waitingCountText.text = npcsWaiting.Count.ToString("00");
#if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("Attraction started");

            StartAttraction();
        } else if(Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("Attraction stopped");
            hud.ShowAlert("Attraktion gestoppt");

            StopAttraction();
        }
#endif
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
                hud.ShowAlert("Keiner in der Warteschlange");
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

            while(npcsActive.Count > 0)
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

    public void Ansage()
    {

    }

    public void Special()
    {

    }

    public void Notstop()
    {
        //Todo: Spieler bestrafen, NPCs schlechte Laune verpassen
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

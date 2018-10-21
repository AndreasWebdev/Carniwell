using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    GameController game;


    public enum status
    {
        QUEUE,
        WALKING,
        ATTRACTION,
        IDLE
    }

    public int happiness = 100;
    public status currentStatus = status.IDLE;
    public Transform destination;
    public NavMeshAgent agent;
    public Animator anim;

    int nextUpdate = 1;
    AttractionController lastVisitedAttraction;

    public int remainingIdleTime = 0;
    private bool walkRandom = true;

    VisitorManager vm;

    public Renderer _shirtRenderer;
    MaterialPropertyBlock _propBlock;

    string npcName;

    Vector3 GetRandomLocation()
    {
        Vector2 actualPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 randomPos = actualPos + Random.insideUnitCircle * 10;
        return new Vector3(randomPos.x, transform.position.y, randomPos.y);
    }


    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();

    }


    private void Start()
    {
        game = FindObjectOfType<GameController>();
        happiness = Random.Range(80, 100);
        vm = FindObjectOfType<VisitorManager>();
        vm.allVisitors.Add(this);
        npcName = NameDatabase.instance.GetRandomName();
        Debug.Log("New NPC: " + npcName);
    }


    void ShowNPC()
    {
        // Disable NPC renderer/agent
        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();

        collider.enabled = true;
        render.enabled = true;
        _shirtRenderer.enabled = true;
        agent.enabled = true;
    }


    void HideNPC()
    {
        // Disable NPC renderer/agent
        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();

        collider.enabled = false;
        render.enabled = false;
        _shirtRenderer.enabled = false;
        agent.enabled = false;
    }


    void Update()
    {
        // If the next update is reached
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        // Create new path
        if (currentStatus == status.IDLE && remainingIdleTime == 0) {
            // 50:50 Chance calculator
            // 1 = attraction
            // 0 = random
            walkRandom = (Random.value > 0.55f);

            // Show NPC
            ShowNPC();
            currentStatus = status.WALKING;

            if (!walkRandom) {
                ParkManager parkManager = FindObjectOfType<ParkManager>();
                if (parkManager) {
                    List<AttractionController> attractions = parkManager.activeAttractions;
                    if (attractions.Count > 0) {
                        int attractionIndex = Random.Range(0, attractions.Count);

                        // Do not move to last attraction again
                        if (!lastVisitedAttraction || lastVisitedAttraction != attractions[attractionIndex]) {
                            destination = attractions[attractionIndex].entrancePosition;

                            lastVisitedAttraction = attractions[attractionIndex];

                            agent.SetDestination(new Vector3(destination.position.x, transform.position.y, destination.position.z));
                            anim.SetBool("moving", true);
                        } else {
                            currentStatus = status.IDLE;
                        }
                    } else {
                        currentStatus = status.IDLE;
                    }
                } else {
                    currentStatus = status.IDLE;
                }
            } else {
                remainingIdleTime = Random.Range(game.idleTimeMin, game.idleTimeMax); ;
                agent.SetDestination(GetRandomLocation());
                anim.SetBool("moving", true);
            }
        }

        if(currentStatus == status.WALKING)
        {
            if(!agent.pathPending)
            {
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        if(destination)
                        {
                            anim.SetBool("moving", false);

                            AttractionController attraction = lastVisitedAttraction.GetComponent<AttractionController>();
                            // Check if target is really an attraction
                            if (attraction)
                            {
                                attraction.AddNPCToQueue(gameObject);
                                remainingIdleTime = 0;

                                HideNPC();
                            }

                            destination = null;
                        }
                        else if (walkRandom && remainingIdleTime != 0)
                        {
                            currentStatus = status.IDLE;
                            anim.SetBool("moving", false);
                        }
                    }
                }
            }
        }
    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        if(currentStatus.Equals(status.QUEUE))
        {
            if (lastVisitedAttraction.npcsWaiting.Count <= lastVisitedAttraction.npcAmount)
            {
                UpdateHappiness(game.penaltyInQueue);
            }else if(lastVisitedAttraction.npcsWaiting.Count > lastVisitedAttraction.npcAmount)
            {
                UpdateHappiness(game.penaltyInQueue*2);
            }
        }

        if (currentStatus.Equals(status.ATTRACTION))
        {
            UpdateHappiness(game.rewardInAttraction);
        }

        if (walkRandom && currentStatus == status.IDLE && remainingIdleTime > 0)
        {
            --remainingIdleTime;
        }

        UpdateShirtColor();
        
#if UNITY_EDITOR
        gameObject.name = "NPC [" + currentStatus.ToString() + "]";
#endif
    }


    void UpdateShirtColor()
    {
        _shirtRenderer.GetPropertyBlock(_propBlock);
        
        if (happiness > game.mediumTopLimit)
        {
            _propBlock.SetColor("_Color", game.goodColor);
            
        }
        else if (happiness < game.mediumBottomLimit)
        {
            _propBlock.SetColor("_Color", game.badColor);
        }
        else
        {
            _propBlock.SetColor("_Color", game.mediumColor);
        }
        _shirtRenderer.SetPropertyBlock(_propBlock);
    }


    public status GetStatus()
    {
        return currentStatus;
    }


    public void SetStatus(status newStatus)
    {
        currentStatus = newStatus;
    }


    public int GetHappiness()
    {
        return happiness;
    }


    public void UpdateHappiness(int reward){
        happiness += reward;
        happiness = Mathf.Clamp(happiness, 0, 100);


        if(happiness <= 0)
        {
            // Dann bleibt er weiter hier
        }
    }

    public void DoneAttraction()
    {
        vm.AddSatisfiedVisitors(1);
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked on " + gameObject.name);
    }

    private void OnDestroy()
    {
        vm.allVisitors.Remove(this);
    }

    void OnTriggerEnter(Collider other)
    {
        AttractionController attraction = other.GetComponentInParent<AttractionController>();
        if(attraction != null)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
            collider.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        AttractionController attraction = other.GetComponentInParent<AttractionController>();
        if(attraction != null)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
            collider.enabled = true;
        }
    }
}


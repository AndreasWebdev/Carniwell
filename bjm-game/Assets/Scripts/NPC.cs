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

    public Material shirtMaterial;

    private int nextUpdate = 1;
    private AttractionController lastVisitedAttraction;

    public int remainingIdleTime = 0;
    private bool walkRandom = true;

    VisitorManager vm;

    public GameObject happinessPopupPrefab;

    Vector3 GetRandomLocation()
    {
        Vector2 actualPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 randomPos = actualPos + Random.insideUnitCircle * 10;
        return new Vector3(randomPos.x, transform.position.y, randomPos.y);
    }

    private void Start()
    {
        game = FindObjectOfType<GameController>();

        vm = FindObjectOfType<VisitorManager>();
        vm.allVisitors.Add(this);

        shirtMaterial.SetColor("_Color", game.goodColor);
    }

    void ShowNPC()
    {
        // Disable NPC renderer/agent
        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();

        collider.enabled = true;
        render.enabled = true;
        agent.enabled = true;
    }

    void HideNPC()
    {
        // Disable NPC renderer/agent
        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();

        collider.enabled = false;
        render.enabled = false;
        agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        if(happiness > game.mediumTopLimit)
        {
            shirtMaterial.SetColor("_Color", game.goodColor);
        }
        else if (happiness < game.mediumBottomLimit)
        {
            shirtMaterial.SetColor("_Color", game.badColor);
        }
        else
        {
            shirtMaterial.SetColor("_Color", game.mediumColor);
        }

        // Create new path
        if (currentStatus == status.IDLE && remainingIdleTime == 0) {
            // 50:50 Chance calculator
            // 1 = attraction
            // 0 = random
            walkRandom = (Random.value > 0.75f);

            // Show NPC
            ShowNPC();
            currentStatus = status.WALKING;

            if (!walkRandom) {
                ParkManager parkManager = GameObject.FindObjectOfType<ParkManager>();
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
            if(happiness > 0)
            {
                UpdateHappiness(game.penaltyInQueue);
            }
        }

        if(walkRandom && currentStatus == status.IDLE && remainingIdleTime > 0)
        {
            --remainingIdleTime;
        }
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
        if(reward > 0)
        {
            if(happinessPopupPrefab != null)
            {
                GameObject popup = (GameObject)Instantiate(happinessPopupPrefab);
                popup.transform.position = this.transform.position;
                popup.transform.parent = this.transform;
                popup.GetComponent<TMPro.TextMeshPro>().text = "+" + reward.ToString("N0");
            }
        }

        if(happiness <= 0)
        {
            // Dann bleibt er weiter hier
        }
    }

    public void DoneAttraction()
    {
        vm.AddSatisfiedVisitors(1);
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


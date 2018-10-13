using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {

    public enum status {
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


    private int nextUpdate = 1;
    private AttractionController lastVisitedAttraction;

    private int idleTime = 5;
    public int remainingIdleTime = 0;
    private bool walkRandom = true;

    Vector3 GetRandomLocation() {
        Vector2 actualPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 randomPos = actualPos + Random.insideUnitCircle * 10;
        return new Vector3(randomPos.x, transform.position.y, randomPos.y);
    }

    // Update is called once per frame
    void Update() {
        // If the next update is reached
        if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        // Create new path
        if (currentStatus == status.IDLE && remainingIdleTime == 0) {
            // 50:50 Chance calculator
            // 1 = attraction
            // 0 = random
            walkRandom = (Random.value > 0.75f);

            // Show NPC
            SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            render.enabled = true;
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
                remainingIdleTime = idleTime;
                agent.SetDestination(GetRandomLocation());
                anim.SetBool("moving", true);
            }
        }

        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    if (destination) {
                        anim.SetBool("moving", false);

                        AttractionController attraction = lastVisitedAttraction.GetComponent<AttractionController>();
                        // Check if target is really an attraction
                        if (attraction) {

                            attraction.AddNPCToQueue(gameObject);
                            remainingIdleTime = 0;

                            // Hide NPC
                            SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                            render.enabled = false;
                        }

                        destination = null;
                    } else if(walkRandom && currentStatus == status.WALKING && remainingIdleTime != 0) {
                        currentStatus = status.IDLE;
                        anim.SetBool("moving", false);
                    }
                }
            }
        }
    }

    // Update is called once per second
    void UpdateEverySecond() {
        if (currentStatus.Equals(status.QUEUE)) {
            if (happiness > 0) {
                AddHappiness(-1);
            }
            //} else if (currentStatus.Equals(status.ATTRACTION)) {
            //    if (happiness < 100) {
            //        ++happiness;
            //    }
        }

        if (walkRandom && currentStatus == status.IDLE && remainingIdleTime > 0) {
            --remainingIdleTime;
        }
    }

    public status GetStatus() {
        return currentStatus;
    }

    public void SetStatus(status newStatus) {
        currentStatus = newStatus;
    }

    public int GetHappiness() {
        return happiness;
    }

    public void AddHappiness(int reward) {
        happiness += reward;

        if (happiness <= 0) {
            // Remove NPC
        }
    }
}


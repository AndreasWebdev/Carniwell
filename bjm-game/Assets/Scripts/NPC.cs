using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {

    public enum status {
        QUEUE,
        WALKING,
        ATTRACTION
    }

    public int happiness = 100;
    public status currentStatus = status.WALKING;
    public Transform destination;
    public NavMeshAgent agent;
    public Animator anim;


    private int nextUpdate = 1;

    // Update is called once per frame
    void Update() {
        //agent.SetDestination(new Vector3(0f, 0.695f,10f));

        // If the next update is reached
        if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        if (!destination && currentStatus == status.WALKING) {
            AttractionController[] attractions = GameObject.FindObjectsOfType<AttractionController>();
            if (attractions.Length > 0) {
                int attractionIndex = Random.Range(0, attractions.Length);
                destination = attractions[attractionIndex].transform;

                agent.SetDestination(new Vector3(destination.position.x, transform.position.y, destination.position.z));
                anim.SetBool("moving", true);
            }
        }

        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    if (destination) {
                        anim.SetBool("moving", false);

                        AttractionController attraction = destination.gameObject.GetComponent<AttractionController>();
                        // Check if target is really an attraction
                        if (attraction) {

                            attraction.AddNPCToQueue(gameObject);

                            // Hide NPC
                            SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                            render.enabled = false;
                        }

                        destination = null;
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

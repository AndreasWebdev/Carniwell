using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Menu : MonoBehaviour {

    public enum status {
        WALKING,
        IDLE
    }
    
    public status currentStatus = status.IDLE;
    public Transform destination;
    public NavMeshAgent agent;
    public Animator anim;

    Vector2 actualPos;
    Vector2 randomPos;
    float agentVelocity = 0;
    float unreachableCheckTime = 5f;

    private int nextUpdate = 1;

    private int idleTime = 5;
    public int remainingIdleTime = 0;

    Vector3 GetRandomLocation() {
        actualPos = new Vector2(transform.position.x, transform.position.z);
        randomPos = actualPos + Random.insideUnitCircle * 10;
        Vector3 newPos = new Vector3();

        // Check if Position is reachable
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(new Vector3(actualPos.x, 5.2f, actualPos.y), path);

        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            newPos = GetRandomLocation();
        } else
        {
            newPos = randomPos;
        }

        return new Vector3(randomPos.x, 5.2f, randomPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        agentVelocity = agent.velocity.sqrMagnitude;
        if(agentVelocity < 0.05f && currentStatus == status.WALKING)
        {
            unreachableCheckTime -= Time.deltaTime;

            if(unreachableCheckTime < 0f)
            {
                agent.SetDestination(GetRandomLocation());
                unreachableCheckTime = 5f;
            }
        }

        Debug.DrawLine(new Vector3(actualPos.x, 1f, actualPos.y), new Vector3(randomPos.x, 5.2f, randomPos.y), Color.red);

        // If the next update is reached
        if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        // Create new path
        if (currentStatus == status.IDLE && remainingIdleTime == 0) {

            // Show NPC
            currentStatus = status.WALKING;

            remainingIdleTime = idleTime;
            agent.SetDestination(GetRandomLocation());
            anim.SetBool("moving", true);
        }

        if (currentStatus == status.WALKING) {
            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        currentStatus = status.IDLE;
                        anim.SetBool("moving", false);
                    }
                }
            }
        }
    }
    
    void UpdateEverySecond() {
        if (currentStatus == status.IDLE && remainingIdleTime > 0) {
            --remainingIdleTime;
        }
    }

    public status GetStatus() {
        return currentStatus;
    }

    public void SetStatus(status newStatus) {
        currentStatus = newStatus;
    }
}


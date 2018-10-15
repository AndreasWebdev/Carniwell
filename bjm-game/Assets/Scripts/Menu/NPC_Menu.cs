using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC_Menu : MonoBehaviour
{
    public enum status
    {
        WALKING,
        IDLE
    }
    
    public status currentStatus = status.IDLE;
    public Transform destination;
    public NavMeshAgent agent;
    public Animator anim;

    Vector2 actualPos;
    Vector2 randomPos;

    int nextUpdate = 1;

    int idleTime = 5;
    public int remainingIdleTime = 0;

    public float timeoutWalking = 2f;
    public float agentVelocity = 0f;


    Vector3 GetRandomLocation()
    {
        actualPos = new Vector2(transform.position.x, transform.position.z);
        randomPos = actualPos + Random.insideUnitCircle * 5;
        Vector3 newPos = new Vector3();

        // Check if Position is reachable
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(new Vector3(randomPos.x, 5.2f, randomPos.y), path);

        if(path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            newPos = GetRandomLocation();
        }
        else
        {
            newPos = randomPos;
        }

        return new Vector3(newPos.x, 5.2f, newPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, new Vector3(randomPos.x, 5.2f, randomPos.y));

        agentVelocity = agent.velocity.sqrMagnitude;

        if(agentVelocity < 0.2f && currentStatus == status.WALKING)
        {
            timeoutWalking -= Time.deltaTime;

            if(timeoutWalking < 0)
            {
                agent.SetDestination(GetRandomLocation());
                timeoutWalking = 2f;
            }
        }

        // If the next update is reached
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        // Create new path
        if(currentStatus == status.IDLE && remainingIdleTime == 0)
        {
            // Show NPC
            currentStatus = status.WALKING;

            remainingIdleTime = idleTime;
            agent.SetDestination(GetRandomLocation());
            anim.SetBool("moving", true);
        }

        if(currentStatus == status.WALKING)
        {
            if(!agent.pathPending)
            {
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        currentStatus = status.IDLE;
                        anim.SetBool("moving", false);
                    }
                }
            }
        }
    }
    
    void UpdateEverySecond()
    {
        if (currentStatus == status.IDLE && remainingIdleTime > 0)
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
}


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


    private int nextUpdate = 1;

    private int idleTime = 5;
    public int remainingIdleTime = 0;

    Vector3 GetRandomLocation() {
        Vector2 actualPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 randomPos = actualPos + Random.insideUnitCircle * 5;
        Vector3 newPos = new Vector3();

        // Check if Position is reachable
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(randomPos, path);

        if (path.status == NavMeshPathStatus.PathPartial)
        {
            newPos = GetRandomLocation();
            Debug.Log("Position Unreachable!");
        } else
        {
            newPos = randomPos;
        }

        return newPos;
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

    void OnDrawGizmosSelected()
    {

        var nav = GetComponent<NavMeshAgent>();
        if (nav == null || nav.path == null)
            return;

        var line = this.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            line.startWidth = 0.2f;
            line.startColor = Color.yellow;
        }

        var path = nav.path;

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }

    }
}


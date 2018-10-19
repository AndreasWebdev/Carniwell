using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParkLayouter : MonoBehaviour
{

    public int amountOfAttractions = 8;
    public float distanceToMiddle = 15;
    public Transform[] allBuildingSpots;
    int nextUpdate = 1;

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.blue;
        for(int i = 1; i < amountOfAttractions; i++)
        {
            float angle = (i * Mathf.PI * 2f / amountOfAttractions);
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * distanceToMiddle, 0, Mathf.Sin(angle) * distanceToMiddle);
            Gizmos.DrawCube(newPos, Vector3.one*5);

        }
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParkManager : MonoBehaviour
{
    public List<AttractionController> activeAttractions = new List<AttractionController>();


    void OnGameStarted()
    {
        StartCoroutine(DelayedFirstAttractrion());
    }

    IEnumerator DelayedFirstAttractrion()
    {
        yield return new WaitForSeconds(1);
        CreateNewAttraction();
        yield return new WaitForSeconds(2);
        CreateNewAttraction();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.C))
        {
            CreateNewAttraction();
        }
#endif
    }

    public void CreateNewAttraction()
    {
        //Finde eine Attraktion die noch nicht in der Liste ist
        Debug.Log("Trying to add new Attraction");
        AttractionDatabase db = FindObjectOfType<AttractionDatabase>();
        List<AttractionController> attrAvailable = new List<AttractionController>();
        for(int i= 0; i < db.attractionPrefabs.Count; i++)
        {
            if(!AttractionAlreadyInList(db.attractionPrefabs[i]))
            {
                attrAvailable.Add(db.attractionPrefabs[i]);
            }
        }
        
        if(attrAvailable.Count > 0)
        {
            AttractionController attrToBuild = attrAvailable[Random.Range(0, attrAvailable.Count)];
            AttractionBuildingSpot[] allBuildingSpots = FindObjectsOfType<AttractionBuildingSpot>();
            List<AttractionBuildingSpot> freeBuildingSpots = new List<AttractionBuildingSpot>();
            for(int i = 0; i < allBuildingSpots.Length; i++)
            {
                if(allBuildingSpots[i].myAttraction == null)
                {
                    freeBuildingSpots.Add(allBuildingSpots[i]);
                }
            }

            AttractionController createdAttraction = freeBuildingSpots[Random.Range(0,freeBuildingSpots.Count)].BuildAttraction(attrToBuild);
            AddAttraction(createdAttraction);

        }
        else
        {
            Debug.Log("no Available Attractions");
        }
    }

    public bool AttractionAlreadyInList(AttractionController _attraction)
    {
        //Suche ob genau diese Attraction (dieses gameObject) schon in Liste vorhanden ist.
        if(activeAttractions.Contains(_attraction))
        {
            return true;
        }

        //Gibt es schon eine Attraktion dieser Art?
        for(int i = 0; i < activeAttractions.Count; i++)
        {
            if(activeAttractions[i].id == _attraction.id)
            {
                return true;
            }
        }

        return false;
    }

    public void AddAttraction(AttractionController _attraction)
    {
        if (AttractionAlreadyInList(_attraction))
        {
            return;
        }

        //Füge Attraktion der Liste hinzu
        activeAttractions.Add(_attraction);
    }
}

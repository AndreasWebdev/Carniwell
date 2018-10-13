using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkManager : MonoBehaviour {

    public List<AttractionController> activeAttractions = new List<AttractionController>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateNewAttraction()
    {

    }

    public bool AttractionAlreadyInList(AttractionController _attraction)
    {
        //Suche ob genau diese Attraction (dieses gameObject) schon in Liste vorhanden ist.
        if (activeAttractions.Contains(_attraction)) return true;

        //Gibt es schon eine Attraktion dieser Art?
        for (int i = 0; i < activeAttractions.Count; i++)
        {
            if(activeAttractions[i] == _attraction)
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

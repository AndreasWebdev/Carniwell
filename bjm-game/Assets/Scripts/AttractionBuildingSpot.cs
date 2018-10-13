using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionBuildingSpot : MonoBehaviour {
    public GameObject buildingSitePrefab;
    public GameObject attractionBuildParticles;
    GameObject buildingSite;

    public AnimationCurve spawnCurve;
    public float spawnAnimationDuration = 2;
    public AttractionController myAttraction = null;
	// Use this for initialization
	void Start () {
        if (transform.GetChild(0) == null)
        {
            buildingSite = (GameObject)Instantiate(buildingSitePrefab);
            buildingSite.transform.position = this.transform.position;
            buildingSite.transform.parent = this.transform;
            Debug.Log("Instantiated building spot");
        }
        else
        {
            
            buildingSite = transform.GetChild(0).gameObject;
        }
	}
	


    public void BuildAttraction(AttractionController _attraction)
    {
        if (myAttraction != null) return; // Wenn bereits eine Attraktion steht, nicht erneut bauen.
        buildingSite.SetActive(false);
        GameObject attraction = (GameObject)Instantiate(_attraction.gameObject);

        float spawnHeight = 30f;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
        attraction.transform.position = spawnPosition;
        if(transform.position.x > 0)
        {
            Vector3 rot = attraction.transform.eulerAngles;

            attraction.transform.rotation = Quaternion.Euler(rot.x, rot.y+180, rot.z);
        }
        AttractionController ac = attraction.GetComponent<AttractionController>();
        myAttraction = ac;
        attraction.transform.parent = this.transform;
        //Moved die Attraktion auf Bodenhöhe
        StartCoroutine(MoveToPosition(attraction.transform, spawnPosition, transform.position,spawnAnimationDuration));

    }


    IEnumerator MoveToPosition(Transform obj, Vector3 origin, Vector3 target, float duration)
    {
        
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            float curvePercent = spawnCurve.Evaluate(percent);
            obj.position = Vector3.LerpUnclamped(origin, target, curvePercent);

            yield return null;
        }
       GameObject particles = Instantiate(attractionBuildParticles);
       particles.transform.position = this.transform.position + Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);
    }
}

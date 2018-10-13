using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionBuildingSpot : MonoBehaviour {
    public GameObject buildingSitePrefab;
    public GameObject attractionBuildParticles;
    GameObject buildingSite;

    public AnimationCurve spawnCurve;
    public float spawnAnimationDuration = 2;
    AttractionController myAttraction = null;
	// Use this for initialization
	void Start () {
        buildingSite = (GameObject)Instantiate(buildingSitePrefab);
        buildingSite.transform.position = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            BuildAttraction(FindObjectOfType<AttractionDatabase>().attractionPrefabs[0]);
        }
#endif
    }


    public void BuildAttraction(AttractionController _attraction)
    {
        if (myAttraction != null) return; // Wenn bereits eine Attraktion steht, nicht erneut bauen.

        GameObject attraction = (GameObject)Instantiate(_attraction.gameObject);

        float spawnHeight = 30f;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
        attraction.transform.position = spawnPosition;
        AttractionController ac = attraction.GetComponent<AttractionController>();
        myAttraction = ac;

        //Moved die Attraktion auf Bodenhöhe
        StartCoroutine(MoveToPosition(attraction.transform, spawnPosition, transform.position,spawnAnimationDuration));

    }


    IEnumerator MoveToPosition(Transform obj, Vector3 origin, Vector3 target, float duration)
    {
        buildingSite.SetActive(false);
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

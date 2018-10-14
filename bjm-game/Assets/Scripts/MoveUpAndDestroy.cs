using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDestroy : MonoBehaviour {
    public float moveUpDuration = 1.5f;
    public float moveUpDistance = 1;

    public AnimationCurve moveCurve;
	// Use this for initialization
	void Start () {
        Vector3 origin = transform.position;
        Vector3 target = transform.position + (Vector3.up * moveUpDistance);
        StartCoroutine(MoveToPosition(transform,origin,target,moveUpDuration));
	}
	
	

    IEnumerator MoveToPosition(Transform obj, Vector3 origin, Vector3 target, float duration)
    {

        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            float curvePercent = moveCurve.Evaluate(percent);
            Vector3 pos = obj.position;
            pos.y = Mathf.LerpUnclamped(origin.y, target.y, curvePercent);
            obj.position = pos;
            yield return null;
        }
        Destroy(gameObject);
        
    }
}

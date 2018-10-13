using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform playerTransfrom;
    public float cameraSmoothing = 3f;

    Vector3 camPosOffset;

    void Start ()
    {
        camPosOffset = transform.position - playerTransfrom.position;
    }
	
	void FixedUpdate ()
    {
        Vector3 targetCamPos = playerTransfrom.position + camPosOffset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, cameraSmoothing * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


    public float minX, maxX;
    public float minY, maxY;
    public float minZ, maxZ;
    public Transform playerTransfrom;
    public float cameraSmoothing = 3f;

    Vector3 camPosOffset;

    void Start ()
    {
        if (playerTransfrom == null)
        {
            playerTransfrom = FindObjectOfType<PlayerMovement>().transform;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
            playerTransfrom.position = hit.point;

        camPosOffset = transform.position - playerTransfrom.position;
        
    }
	
	void FixedUpdate ()
    {
        Vector3 targetCamPos = playerTransfrom.position + camPosOffset;
        
        Vector3 newPosClamped = new Vector3(Mathf.Clamp(targetCamPos.x, minX, maxX), Mathf.Clamp(targetCamPos.y, minY, maxY), Mathf.Clamp(targetCamPos.z, minZ, maxZ));

        transform.position = Vector3.Lerp(transform.position, newPosClamped, cameraSmoothing * Time.deltaTime);

        transform.position = newPosClamped;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;


public class CameraControl : MonoBehaviour
{
    GameController game;

    public float minX, maxX;
    public float minZ, maxZ;

    public float minYRot = -40, maxYRot = 40;

    public Transform playerTransfrom;
    public float cameraSmoothing = 3f;

    Vector3 camPosOffset;

    void Start ()
    {
        game = FindObjectOfType<GameController>();

#if UNITY_ANDROID
        if (GetComponent<PostProcessingBehaviour>() != null)
        {
            GetComponent<PostProcessingBehaviour>().enabled = false;
        }
#endif
        if (playerTransfrom == null)
        {
            playerTransfrom = FindObjectOfType<PlayerMovement>().transform;
        }

        transform.position = new Vector3(0, 2, -40);
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3();
    }

    public void OnGameStarted()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            playerTransfrom.position = hit.point;
        }

        camPosOffset = transform.position - playerTransfrom.position;
    }

    void FixedUpdate ()
    {
        if(game.gameState != GameController.state.RUNNING)
        {
            return;
        }

        //Calculating Position;
        Vector3 targetCamPos = playerTransfrom.position + camPosOffset;
        targetCamPos.y = transform.position.y;
        Vector3 newPosClamped = new Vector3(Mathf.Clamp(targetCamPos.x, minX, maxX), targetCamPos.y/*Mathf.Clamp(targetCamPos.y, minY, maxY)*/, Mathf.Clamp(targetCamPos.z, minZ, maxZ));

        Vector3 lookPos = playerTransfrom.position - transform.position;
        lookPos.y = Mathf.Clamp(lookPos.y, minYRot, maxYRot);
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * cameraSmoothing);

        //Applying Position
        transform.position = Vector3.Lerp(transform.position, newPosClamped, cameraSmoothing * Time.deltaTime);

        transform.position = newPosClamped;
    }
}

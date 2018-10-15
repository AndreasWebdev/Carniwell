using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PinchZoom : MonoBehaviour
{
    public Camera mainCamera;

    public float perspectiveZoomSpeed = 0.5f;
    public float orthoZoomSpeed = 0.5f;

    public float minXRot = 25, maxXRot = 80;
    public float minYPos = 4, maxYPos = 26;


    void Update()
    {
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Vector3 rot = mainCamera.transform.eulerAngles;
            Vector3 pos = mainCamera.transform.position;
            if (!mainCamera.orthographic)
            {
                rot.x += deltaMagnitudeDiff * perspectiveZoomSpeed;
                pos.y += deltaMagnitudeDiff * perspectiveZoomSpeed;
                //mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 0.1f, 179.9f);
            }

            rot.x = Mathf.Clamp(rot.x, minXRot, maxXRot);
            pos.y = Mathf.Clamp(pos.y, minYPos, maxYPos);

            mainCamera.transform.eulerAngles = rot;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,pos,Time.deltaTime *2);
        }
    }
}

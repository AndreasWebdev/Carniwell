using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OffscreenIndicator : MonoBehaviour
{

    public Transform indicator;
    public Transform target;

    [Header("Sprites")]
    public Image spriteHolder;
    [Space(20)]
    public Sprite Arrow;
    public Sprite Exclamation;

    void Update ()
    {
        if (target != null)
        {
            Indicate();
        }else
        {
            spriteHolder.enabled = false;
        }
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }
    public void ChangeSprite(Sprite sprite)
    {
        spriteHolder.sprite = sprite;
    }

    void Indicate()
    {
        Vector3 targetPosOnScreen = Camera.main.WorldToScreenPoint(target.position);

        if (onScreen(targetPosOnScreen))
        {
            indicator.position = targetPosOnScreen;
            indicator.eulerAngles = Vector3.zero;
            ChangeSprite(Exclamation);

            return;
        }
        else
        {
            ChangeSprite(Arrow);
        }

        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        float angle = Mathf.Atan2(targetPosOnScreen.y - center.y, targetPosOnScreen.x - center.x) * Mathf.Rad2Deg;

        float coef;
        if (Screen.width > Screen.height)
            coef = Screen.width / Screen.height;
        else
            coef = Screen.height / Screen.width;

        float degreeRange = 360f / (coef + 1);
        
        if (angle < 0) angle = angle + 360;
        int edgeLine;
        if (angle < degreeRange / 4f) edgeLine = 0;
        else if (angle < 180 - degreeRange / 4f) edgeLine = 1;
        else if (angle < 180 + degreeRange / 4f) edgeLine = 2;
        else if (angle < 360 - degreeRange / 4f) edgeLine = 3;
        else edgeLine = 0;

        indicator.eulerAngles = new Vector3(0, 0, angle);
        Vector3 pos = intersect(edgeLine, center, targetPosOnScreen);
        pos.x = Mathf.Clamp(pos.x,0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        indicator.position = pos;
    }

    bool onScreen(Vector2 input)
    {
        return !(input.x > Screen.width || input.x < 0 || input.y > Screen.height || input.y < 0);
    }

    Vector3 intersect(int edgeLine, Vector3 line2point1, Vector3 line2point2)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float[] A1 = { -screenHeight, 0, screenHeight, 0 };
        float[] B1 = { 0, -screenWidth, 0, screenWidth };
        float[] C1 = { -screenWidth * screenHeight, -screenWidth * screenHeight, 0, 0 };

        float A2 = line2point2.y - line2point1.y;
        float B2 = line2point1.x - line2point2.x;
        float C2 = A2 * line2point1.x + B2 * line2point1.y;

        float det = A1[edgeLine] * B2 - A2 * B1[edgeLine];

        //Debug.DrawLine(line2point1, line2point2,Color.red);
        //Debug.DrawLine(line2point1, new Vector3((B2 * C1[edgeLine] - B1[edgeLine] * C2) / det, (A1[edgeLine] * C2 - A2 * C1[edgeLine]) / det, 0));
        return new Vector3((B2 * C1[edgeLine] - B1[edgeLine] * C2) / det, (A1[edgeLine] * C2 - A2 * C1[edgeLine]) / det, 0);
    }


}

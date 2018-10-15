using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreeController : MonoBehaviour
{
    public Renderer _treeRenderer;


    public void UpdateTransparency(float transparency)
    {
        for(int materialIndex = 0; materialIndex < _treeRenderer.materials.Length; ++materialIndex)
        {
            Color startColor = _treeRenderer.materials[materialIndex].GetColor("_Color");
            Color endColor = startColor;
            endColor.a = Mathf.Clamp01(transparency);

            StartCoroutine(ChangeColor(_treeRenderer.materials[materialIndex], startColor, endColor, 0.2f));
        }
    }

    IEnumerator ChangeColor(Material material, Color startColor, Color endColor, float duration)
    {
        float t = 0;
        while(t < 1)
        {
            Color color = Color.Lerp(startColor, endColor, t);
            material.SetColor("_Color", color);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}

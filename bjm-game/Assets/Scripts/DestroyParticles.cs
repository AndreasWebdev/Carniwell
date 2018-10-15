using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyParticles : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);

        Destroy(gameObject);
    }
}

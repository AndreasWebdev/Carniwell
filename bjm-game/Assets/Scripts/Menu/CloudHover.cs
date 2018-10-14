using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHover : MonoBehaviour {
	void Update () {
        transform.position += Vector3.forward * 3f * Time.deltaTime;

        if(transform.position.z > 100)
        {
            transform.position -= new Vector3(0f, 0f, 150f);
        }
	}
}

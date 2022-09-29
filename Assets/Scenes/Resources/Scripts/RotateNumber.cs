using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNumber : MonoBehaviour
{

    // Numbers always face the camera
    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
    }
}

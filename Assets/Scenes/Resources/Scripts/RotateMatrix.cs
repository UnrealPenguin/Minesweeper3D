using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMatrix : MonoBehaviour
{
    Vector3 mFromPos = Vector3.zero;
    Vector3 mToPos = Vector3.zero;

    GameObject toRotate;
    private float rotationSpeed = 5f;
    void Start(){
        toRotate = GameObject.Find("Generator");
    }
    
    void OnMouseDrag(){
        float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
        float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

        toRotate.transform.Rotate(Vector3.down, xAxis, Space.World);
        toRotate.transform.Rotate(Vector3.right, yAxis, Space.World);
    }
}

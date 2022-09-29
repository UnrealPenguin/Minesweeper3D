using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{   
    Camera mainCam;
    void Start(){
        mainCam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {   
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");
        // zoom in, out or reset view
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            mainCam.transform.position = mainCam.transform.position + new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")*5);
        
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            mainCam.transform.position = mainCam.transform.position + new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")*5);

        // allows camera to pan
        if(Input.GetMouseButton(2)){
            mainCam.transform.position = mainCam.transform.position + new Vector3(xAxis/-5, 0, 0);
            mainCam.transform.position = mainCam.transform.position + new Vector3(0, yAxis/-5, 0);
        }
    }
}

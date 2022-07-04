using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraManager : MonoBehaviour
{

    public Camera activeCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activeCamera = GetComponentInParent<CameraManager>().mainCamera;
        transform.position = new Vector3(activeCamera.transform.position.x, transform.position.y, activeCamera.transform.position.z);

    }
}

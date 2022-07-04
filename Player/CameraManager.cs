using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public float panSpeed;
    public float rotateSpeed;
    public float rotateAmount;
    public Vector3[] firmCameraPositions;

    public float panBorderSpeed = 3;
    public float scrollSpeed = 10;
    public Vector3 panLimit;

    private Quaternion rotation;

    private float panDetect = 15f;
    [SerializeField] private float minHeightDifference = 400f;
    [SerializeField] private float maxHeightDifference = 600f;
    
    public Camera mainCamera;
    public Camera MinimapCamera;

    public LayerMask IgnoreLayer;

    private Vector3 forwardMove;
    private Vector3 lateralMove;

    float cursorPositionX;
    float cursorPositionY;

    void Start()
    {
        mainCamera.enabled = true;
        rotation = Camera.main.transform.rotation;
    }

    void Update()
    {
        SetCameraHeight();
        MoveCamera();
        RotateCamera();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = rotation;
        }

    }

    void MoveCamera()
    {
        float CamPosX = Camera.main.transform.position.x;
        float CamPosY = Camera.main.transform.position.y;//the lower the camera is, the slower the WASD will move the camera 
        float CamPosZ = Camera.main.transform.position.z;
        cursorPositionX = Input.mousePosition.x;
        cursorPositionY = Input.mousePosition.y;

        forwardMove = new Vector3(0, 0, 0);
        lateralMove = new Vector3(0, 0, 0);

        GetForwardMoveInput();
        GetLateralMoveInput();

        forwardMove.y = 0;
        Vector3 move = lateralMove + forwardMove;

        CamPosX = Mathf.Clamp(CamPosX + move.x, -panLimit.x, panLimit.x);
        CamPosZ = Mathf.Clamp(CamPosZ + move.z, -panLimit.y, panLimit.y);
        Vector3 newPos = new Vector3(CamPosX, CamPosY, CamPosZ);

        Camera.main.transform.position = newPos;
    }

    void GetForwardMoveInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            forwardMove = GetForwardMoveValue(panSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwardMove = -GetForwardMoveValue(panSpeed);
        }
        else if (cursorPositionY < Screen.height && cursorPositionY > Screen.height - panDetect)
        {
            forwardMove = GetForwardMoveValue(panBorderSpeed);
        }
        else if (cursorPositionY > 0 && cursorPositionY < panDetect)
        {
            forwardMove = -GetForwardMoveValue(panBorderSpeed);
        }
    }

    Vector3 GetForwardMoveValue(float panSpeed)
    {
        return Camera.main.transform.forward * panSpeed * Time.deltaTime;
    }

    void GetLateralMoveInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            lateralMove = -GetRightMoveValue(panSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            lateralMove = GetRightMoveValue(panSpeed);
        }
        else if (cursorPositionX > 0 && cursorPositionX < panDetect)
        {
            lateralMove = -GetRightMoveValue(panBorderSpeed);
        }
        else if (cursorPositionX < Screen.width && cursorPositionX > Screen.width - panDetect)
        {
            lateralMove = GetRightMoveValue(panBorderSpeed);
        }
    }

    Vector3 GetRightMoveValue(float panSpeed)
    {
        return Camera.main.transform.right * panSpeed * Time.deltaTime;
    }


    void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        if (Input.GetMouseButton(2))
        {
            
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }

        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }

        if (Input.GetMouseButtonUp(2))
        {
            float closestDistance = 1000f;
            Vector3 targetCameraPosition = firmCameraPositions[0];

            foreach (Vector3 cameraPosition in firmCameraPositions)
            {

                if (AngleBetween(new Vector2(origin.y, origin.z), new Vector2(cameraPosition.y, cameraPosition.z)) < closestDistance)//vector3.distance
                {
                    closestDistance = AngleBetween(origin, cameraPosition);
                    targetCameraPosition = cameraPosition;
                }
            }
            float newVertical = Camera.main.transform.eulerAngles.x;
            Camera.main.transform.eulerAngles = new Vector3(newVertical, targetCameraPosition.y, targetCameraPosition.z);
            MinimapCamera.transform.eulerAngles = new Vector3(90, targetCameraPosition.y, 0);
        }
    }

    float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector3.Distance(vectorA, vectorB);
        if (angle > 180)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    void SetCameraHeight()
    {
        float CamPosY = Camera.main.transform.position.y;
        float CamPosX = Camera.main.transform.position.x;
        float CamPosZ = Camera.main.transform.position.z;

        float[] potentialX = { 1f, 0.71f, 0f, -0.71f, -1f, -0.71f, 0f, 0.71f };
        float[] potentialZ = { 0f, 0.71f, 1f, 0.71f, 0f, -0.71f, -1f, -0.71f };
        RaycastHit hitAround;
        float maxAroundHeight = 0;
        for (int i = 0; i < 8; i++)
        {
            Vector3 RaycastPosition = new Vector3(Camera.main.transform.position.x + potentialX[i] * 400, 0, Camera.main.transform.position.z + potentialZ[i] * 400);
            Vector3 CameraPosition = new Vector3(Camera.main.transform.position.x, 900, Camera.main.transform.position.z);
            Debug.DrawLine(CameraPosition, RaycastPosition, Color.green, 0.4f);
            if (Physics.Linecast(CameraPosition, RaycastPosition, out hitAround, ~IgnoreLayer))
            {
                float aroundHeight = hitAround.point.y;
                Debug.Log("vyska terenu " + aroundHeight);
                if (aroundHeight > maxAroundHeight)
                {
                    maxAroundHeight = aroundHeight;
                }
            }
        }
        
        //CamPosY += (maxAroundHeight - CamPosY) * panSpeed * CamPosY * Time.deltaTime;
        if (CamPosY - maxAroundHeight < minHeightDifference)
        {
            CamPosY += (maxAroundHeight - CamPosY + 200) * Time.deltaTime;
        }

        if (CamPosY - maxAroundHeight > maxHeightDifference)
        {
            CamPosY -= (CamPosY - maxAroundHeight - 200) * Time.deltaTime;
        }
        
        Vector3 newPos = new Vector3(CamPosX, CamPosY, CamPosZ);

        Camera.main.transform.position = newPos;
    }

}

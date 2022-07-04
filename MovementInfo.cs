using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementInfo : MonoBehaviour
{
    Vector3 lastPosition = Vector3.zero;
    public float speed = 0;
    private NavMeshAgent agent;
    public Vector3 previousPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        previousPosition = transform.position;
    }

    void Update()
    {
    }

    public float GetSpeed()
    {
        speed = agent.velocity.magnitude;
        return speed;
    }

    public void GetPreviousPosition()
    {

    }

    public Vector3 GetRotation()
    {
        Vector3 orientationVector = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
        
        return orientationVector;
    }

}

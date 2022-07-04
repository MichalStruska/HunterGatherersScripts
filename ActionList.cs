using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionList : MonoBehaviour
{

    public bool following;
    public NavMeshAgent agent;
    public GameObject prey;
    public float agent_speed;
    public int unitNumber;

    public void Move(NavMeshAgent agent, Vector3 hitPoint, float agent_speed, int unitNumber)
    {
        Debug.Log("move1 " + hitPoint);
        agent.destination = hitPoint;
        agent.speed = agent_speed;
    }

}

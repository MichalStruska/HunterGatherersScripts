using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    private Vector3 startingPosition;
    Vector3 PrevPos;
    Vector3 NewPos;
    Vector3 ObjVelocity;
    private NavMeshAgent agent;
    private int walkSpeed = 5;
    public float awareDistance = 500f;
    public float huntFleeDistance = 1000f;
    Animator animator;
    AnimalTaskList animalTask;
    public Vector3 targetPoint;

    private void Start()
    {
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //animalTask = GetComponent<CheckTerrainTexture>().animalTask;
    }

    private void Update()
    {

    }

    public void Move(Vector3 goToPos, float speed)
    {
        agent.destination = goToPos; //hit.point;
        agent.speed = speed;
    }

}





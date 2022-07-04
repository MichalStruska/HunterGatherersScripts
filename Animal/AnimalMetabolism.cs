using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMetabolism : MonoBehaviour
{
    Vector3 NewPos;
    private NavMeshAgent agent;
    private float horseCoeffA;
    private float horseCoeffB;
    private float horseCoeffC;

    private float ponyCoeffA;
    private float ponyCoeffB;
    private float ponyCoeffC;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
    }

    void RemainingEnergy()
    {
        //GetComponent<AnimalInfo>().animalEnergy++;
    }

}





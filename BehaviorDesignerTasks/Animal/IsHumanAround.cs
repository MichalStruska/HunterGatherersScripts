using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class IsHumanAround : Conditional
{

    public SharedTransformList dangerousHumans;
    public GameObject[] humans;
    private Vector3 animalPosition;
    public float awareDistance;
    public float initialAwareDistance = 300;
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;
    public SharedFloat dangerState;
    public SharedFloat humanDistance;
    private float closestDistance;
    private GameObject CurrentHuman;

    public override void OnAwake()
    {
        awareDistance = initialAwareDistance;
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);        
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = transform.position;

        if (AreHumansClose())
        {
            awareDistance = 500;
            return TaskStatus.Success;
        }
        awareDistance = initialAwareDistance;
        return TaskStatus.Failure;
    }

    bool AreHumansClose()
    {
        closestDistance = awareDistance;
        dangerousHumans.Value.Clear();
        GetDangerousHumans();
        if (dangerousHumans.Value.Count > 0)
        {
            CalculateDangerState();
            humanDistance.Value = closestDistance;
            return true;
        }

        return false;
    }

    void GetDangerousHumans()
    {
        foreach (GameObject Human in currentGameObject.GetComponent<AnimalMemory>().humans)
        {
            CurrentHuman = Human;
            if (IsHumanClose() && IsHumanAlive())
            {
                dangerousHumans.Value.Add(CurrentHuman.transform);
                closestDistance = Mathf.Min(closestDistance, humanDistance.Value);
            }
        }
    }

    bool IsHumanAlive()
    {
        return CurrentHuman.activeSelf;
    }

    void CalculateDangerState()
    {
        dangerState.Value = 1 - (closestDistance / awareDistance);
    }

    bool IsHumanClose()
    {
        humanDistance.Value = Vector3.Distance(animalPosition, CurrentHuman.transform.position);
        return humanDistance.Value < awareDistance;
    }

    public override void OnReset()
    {
        dangerousHumans.Value.Clear();
        //dangerState.Value = 0;
    }
}

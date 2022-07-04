using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using System.Collections.Generic;

public class GetRoamTarget : Action
{
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;
    
    public SharedVector3 roamingTarget;
    public Vector3 animalPosition;

    public SharedFloat analysedCircleSize;
    public SharedFloat analysedCircleDistance;

    public List<GameObject> humans;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(currentGameObject);
        humans = currentGameObject.GetComponent<AnimalMemory>().humans;
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = currentGameObject.transform.position;
        
        GetNewRoute();
        
        return TaskStatus.Success;
    }

    public void GetNewRoute()
    {
        float newDirectionX = Random.Range(0f, 1f);
        float newDirectionZ = Random.Range(-1f, 1f);
        FindLowestRisk();
        

        //currentGameObject.GetComponent<AnimalAI>().Move(randomWalkTarget, walkSpeed);
    }

    public void FindLowestRisk()
    {
        float[] potentialX = { 1f, 0.71f, 0f, -0.71f, -1f, -0.71f, 0f, 0.71f };
        float[] potentialZ = { 0f, 0.71f, 1f, 0.71f, 0f, -0.71f, -1f, -0.71f };

        List<Vector3> bestDirections = new List<Vector3>();

        float minDangerScore = 10000f;
        float dangerScore;
        for (int i = 0; i < 8; i++)
        {
            Vector3 orientationVector = currentGameObject.GetComponent<MovementInfo>().GetRotation();

            float potentialAngle = AngleBetween(new Vector2(potentialX[i], potentialZ[i]), new Vector2(orientationVector.x, orientationVector.z));
            float absDistance = 0;
            float humanCounter = 0;
            float sumDistance = 10000f;
            for (int j = 0; j < humans.Count; j++)
            {
                Vector3 potentialPosition = animalPosition + new Vector3(potentialX[i] * 1000, 0, potentialZ[i] * 1000);
                potentialPosition = currentGameObject.GetComponent<GetTerrainPoint>().GetNewPoint(potentialPosition);
                Vector3 direction = (potentialPosition - animalPosition);
                Ray ray = new Ray(animalPosition, direction);
                float distanceCross = Vector3.Cross(ray.direction, humans[j].transform.position - ray.origin).magnitude;
                float distancePoint = Vector3.Distance(humans[j].transform.position, potentialPosition);
                if (distanceCross < analysedCircleSize.Value || distancePoint < analysedCircleSize.Value)
                {
                    humanCounter++;
                    sumDistance += distancePoint;
                    sumDistance += distanceCross;
                }

                dangerScore = humanCounter + (1 / sumDistance) + (Mathf.Sin(Mathf.PI * (potentialAngle/180f)))/100;
                if (dangerScore < minDangerScore)
                {
                    minDangerScore = dangerScore;
                    bestDirections = new List<Vector3>() { potentialPosition };
                }
                else if (dangerScore == minDangerScore)
                {
                    bestDirections.Add(potentialPosition);
                }
            }
        }

        int randomNumber = Random.Range(0, bestDirections.Count);

        roamingTarget.Value = bestDirections[randomNumber];
    }

    public float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        //if (angle > 180)
        //{
        //    angle = 360 - angle;
        //}
        return angle;
    }

    public override void OnReset()
    {
        //foundGrass.Value = false;
    }


}

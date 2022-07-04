using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class GetRunawayPoint : Action
{
    public SharedTransformList dangerousHumans;
    public SharedVector3 runAwayPoint;
    public float runAwaySpeed;
    public SharedGameObject targetGameObject;
    private NavMeshAgent agent;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {

        RunAway();

        return TaskStatus.Success;

    }

    private void RunAway()
    {

        float[] potentialX = { 1f, 0.71f, 0f, -0.71f, -1f, -0.71f, 0f, 0.71f };
        float[] potentialZ = { 0f, 0.71f, 1f, 0.71f, 0f, -0.71f, -1f, -0.71f };
        int maxIndex = 0;
        float maxValue = 0;
        for (int i = 0; i < 8; i++)
        {
            float absDistance = 0;
            
            for (int j = 0; j < dangerousHumans.Value.Count; j++)
            {
                currentGameObject.GetComponent<AnimalMemory>().AddHumanPosition(dangerousHumans.Value[j].position);
                Vector3 potentialPosition = transform.position + new Vector3(potentialX[i], 0, potentialZ[i]);
                absDistance += Mathf.Abs(Vector3.Distance(potentialPosition, dangerousHumans.Value[j].position));
            }

            if (i == 0)
            {
                maxValue = absDistance;
            }
            else if (i > 0 && absDistance > maxValue)
            {
                maxIndex = i;
                maxValue = absDistance;
            }

        }

        Vector3 runAwayDirection = new Vector3(potentialX[maxIndex], 0, potentialZ[maxIndex]);
        runAwayPoint.Value = GetComponent<GetTerrainPoint>().GetNewPoint(transform.position + (runAwayDirection * 40f));
        
        
        Debug.DrawLine(new Vector3(transform.position.x, 1f, transform.position.z), transform.position + (new Vector3(runAwayDirection.x, 1f, runAwayDirection.z) * 40f), Color.red, 2f);
        Debug.DrawLine(new Vector3(runAwayPoint.Value.x, 0.4f, runAwayPoint.Value.z), new Vector3(runAwayPoint.Value.x + 6, 0.4f, runAwayPoint.Value.z + 6), Color.blue, 2f);
        //currentGameObject.GetComponent<AnimalAI>().Move(runAwayPoint, runAwaySpeed);
    }

}

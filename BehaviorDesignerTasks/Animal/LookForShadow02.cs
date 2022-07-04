using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class LookForShadow02 : Action
{
    private int shadowDist;
    private GameObject sun;
    public SharedGameObject targetGameObject;
    public SharedVector3 shadowTargetPoint;
    private RaycastHit hit;
    private int walkSpeed = 15;
    private Vector3 treePosition;
    private float lookingAngle;
    private Vector3 animalLook1;
    private Vector3 animalPosition;
    private float lookingDist;
    public float treeSearchHeight = 10f;
    private float closestTree = 3000f;
    public SharedBool foundShadow = false;
    private Vector3 animalEyePosition;
    private int changeDirectionCounter;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        shadowDist = 5;
        sun = GameObject.Find("Sun");
        lookingAngle = -45f;
        foundShadow.Value = false;
        lookingDist = 1000;
        changeDirectionCounter = 399;
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = currentGameObject.transform.position;
        LookForTrees();
        if (foundShadow.Value)
        {

            //shadowTargetPoint.Value = hit.point;
            return TaskStatus.Success;
        }

        if (changeDirectionCounter > 400)
        {
            Vector3 randomWalkTarget = new Vector3(animalPosition.x + Random.Range(0f, 1f) * 500, 0, animalPosition.z + Random.Range(-1f, 1f) * 500);
            currentGameObject.GetComponent<AnimalAI>().Move(randomWalkTarget, walkSpeed);
            //currentGameObject.GetComponent<AnimalInfo>().animalTask = AnimalTaskList.Walking;
            changeDirectionCounter = 0;
        }
        
        changeDirectionCounter++;
        return TaskStatus.Failure;

    }

    public override void OnReset()
    {
        changeDirectionCounter = 999;
        //foundShadow.Value = false;
    }


    void FindShadow()
    {
        Vector3 rotatedVector1 = new Vector3(treePosition.x + shadowDist, 0, treePosition.z + 0);
        Vector3 rotatedVector2 = new Vector3(treePosition.x + 0, 0, treePosition.z + shadowDist);
        Vector3 rotatedVector3 = new Vector3(treePosition.x - shadowDist, 0, treePosition.z - 0);
        Vector3 rotatedVector4 = new Vector3(treePosition.x - 0, 0, treePosition.z - shadowDist);
        ShadowRaycast(rotatedVector1);
        ShadowRaycast(rotatedVector2);
        ShadowRaycast(rotatedVector3);
        ShadowRaycast(rotatedVector4);
        shadowDist += 5;

        if (shadowDist == 20)
        {
            shadowDist = 5;
        }
    }

    void TreeRaycast(Vector3 lookPos)
    {
        Debug.DrawRay(new Vector3(animalPosition.x, animalPosition.y + treeSearchHeight, animalPosition.z), new Vector3(lookPos.x * 1000, treeSearchHeight, lookPos.z * 1000), Color.blue, 0.5f);
        if (Physics.Raycast(new Vector3(animalPosition.x, animalPosition.y + treeSearchHeight, animalPosition.z), new Vector3(lookPos.x * 1000, treeSearchHeight, lookPos.z * 1000), out hit, 200))
        {    
            treePosition = hit.point; //hit.collider.transform.position;
            closestTree = Vector3.Distance(treePosition, animalPosition);
            FindShadow();
        }
    }

    void ShadowRaycast(Vector3 lookPos)
    {
        Vector3 targetShadow = new Vector3(lookPos.x, lookPos.y, lookPos.z);
        //Debug.DrawRay(targetShadow, sun.transform.position, Color.blue, 5f);

        if (Physics.Raycast(targetShadow, sun.transform.position, out hit, 4000))
        {
            if (hit.collider.tag == "Tree")
            {
                shadowTargetPoint.Value = lookPos;
                Debug.Log("nasel " + shadowTargetPoint.Value);
                currentGameObject.GetComponent<AnimalInfo>().animalTask = AnimalTaskList.GoingToShadow;
                foundShadow.Value = true;
            }
        }

    }

    void LookAround(float lookHeight)
    {
        animalEyePosition = new Vector3(animalPosition.x, animalPosition.y + 3, animalPosition.z);
        animalLook1 = new Vector3(transform.forward.x * lookingDist, lookHeight, transform.forward.z * lookingDist);
    }

    void LookForTrees()
    {
        LookAround(6f);
        Vector3 rotatedVector = Quaternion.AngleAxis(lookingAngle, Vector3.up) * animalLook1;
        TreeRaycast(rotatedVector);
        Vector3 rotatedVector2 = Quaternion.AngleAxis(-lookingAngle, Vector3.up) * animalLook1;
        TreeRaycast(rotatedVector2);

        lookingAngle += 1;
        if (lookingAngle == 90)
        {
            lookingAngle = -45;
        }
       
    }



}

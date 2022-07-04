using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class LookForGrass : Action
{

    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;

    private float lookingDistance;
    private int shadowDist;
    private float lookingAngle;
    private Vector3 animalPosition;
    private Vector3 animalEyePosition;
    public AnimalTaskList animalTask;
    private Vector3 animalLook1;
    private Vector3 animalLook2;
    private Vector3 animalLook3;
    private RaycastHit hit;
    private GameObject sun;
    public int posX;
    public int posZ;
    public float closestGrass = 3000f;
    public float humanZone = 200f;
    public Terrain currentTerrain;
    private float walkSpeed = 15f;
    public SharedBool foundGrass;
    public SharedVector3 grassPosition;
    private Terrain observedTerrain;

    private int changeDirectionCounter;
    
    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(currentGameObject);
        lookingDistance = 1f;
        lookingAngle = -45f;
        changeDirectionCounter = 999;
        foundGrass.Value = false;
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = currentGameObject.transform.position;
        animalTask = currentGameObject.GetComponent<AnimalInfo>().animalTask;
        observedTerrain = Terrain.activeTerrain;

        FindGrass();
        if (foundGrass.Value)
        {
            return TaskStatus.Success;
        }
      
        changeDirectionCounter++;
        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        //foundGrass.Value = false;
    }

    private void FindGrass()
    {
        LookAround(-0.2f);

        Vector3 rotatedVector = Quaternion.AngleAxis(lookingAngle, Vector3.up) * animalLook1;
        LookAtDirection(new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z));

        Debug.DrawRay(animalPosition, new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z), Color.green, 0.1f);

        //rotatedVector = Quaternion.AngleAxis(-lookingAngle, Vector3.up) * animalLook1;
        
        LookAtDirection(new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z));

        lookingDistance += 10f;
        if (lookingDistance > 300)
        {
            lookingDistance = 1f;
            lookingAngle += 10;
        }

        if (lookingAngle == 45)
        {
            lookingAngle = -45;
        }

    }

    void LookAround(float lookHeight)
    {
        animalEyePosition = new Vector3(animalPosition.x, animalPosition.y + 6, animalPosition.z);
        animalLook1 = new Vector3(currentGameObject.transform.forward.x * lookingDistance, lookHeight, currentGameObject.transform.forward.z * lookingDistance);
        //Debug.DrawRay(animalPosition, new Vector3(animalLook1.x, animalLook1.y, animalLook1.z), Color.red, 0.1f);
    }

    void LookAtDirection(Vector3 lookDir)
    {
        if (Physics.Raycast(animalEyePosition, lookDir, out hit, 400))
        {


            if (DropRay(hit.point))
            {
                grassPosition.Value = hit.point;
                currentGameObject.GetComponent<AnimalInfo>().animalTask = AnimalTaskList.GoingToGrass;
                foundGrass.Value = true;
            }

            else
            {
                foundGrass.Value = false;
            }

        }
        Debug.Log("LookAtDirection " + foundGrass.Value);
    }

    public bool DropRay(Vector3 rayPosition)
    {
        RaycastHit downHit;
        if (Physics.Raycast(new Vector3(rayPosition.x, 200, rayPosition.z), Vector3.down, out downHit, 400))
        {
            Debug.Log("collider " + downHit.collider.tag);
            if (downHit.collider.tag == "Grass")
            {
                return true;
            }
        }
        return false;
    }

    public void GetTerrainTexture()
    {
        ConvertPosition();
        float[,,] aMap = observedTerrain.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        if (aMap[0, 0, 1] == 1) // && IntersectsHuman(animalPosition, hit.point) == false
        {
            grassPosition.Value = hit.point;
            currentGameObject.GetComponent<AnimalInfo>().animalTask = AnimalTaskList.GoingToGrass;
            foundGrass.Value = true;
        }
    }

    void ConvertPosition()
    {
        Vector3 terrainPosition = hit.point - currentTerrain.transform.position;
        Debug.DrawLine(hit.point, new Vector3(hit.point.x + 5, hit.point.y, hit.point.z + 5), Color.red, 2f);

        Vector3 mapPosition = new Vector3(terrainPosition.x / currentTerrain.terrainData.size.x, 0, terrainPosition.z / currentTerrain.terrainData.size.z);

        float xCoord = mapPosition.x * currentTerrain.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * currentTerrain.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

}

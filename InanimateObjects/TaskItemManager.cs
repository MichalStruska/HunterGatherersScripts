using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskItemManager : MonoBehaviour
{
    public int taskTimeMultiplier = 6;
    public List<GameObject> taskingHumans = new List<GameObject>();

    public float timer = 0f;
    public float timeLimit = 20f;
    public bool taskCompleted;
    public int humanCounter;
    [SerializeField]
    public Positions[] positionShifts = new[] { new Positions(new Vector3(10f, 0f, 0f), true), new Positions(new Vector3(0f, 0f, 10f), true), new Positions(new Vector3(-10f, 0f, 0f), true), new Positions(new Vector3(0f, 0f, -10f), true) };

    void Start()
    {
        taskCompleted = false;
        humanCounter = 0;
    }

    void Update()
    {
        Debug.Log("je hotovo " + humanCounter);
        if (timer > timeLimit)
        {
            taskCompleted = true;
            DestroyObject();
        }
    }

    public void AddHuman(GameObject Human)
    {
        taskTimeMultiplier--;
        humanCounter++;
        taskingHumans.Add(Human);
        GetPositionShift(Human);
        foreach (GameObject hu in taskingHumans)
        {
            hu.GetComponent<HumanInfo>().taskTimeMultiplier = taskTimeMultiplier;
        }
    }

    public void RemoveHuman(GameObject Human)
    {
        taskTimeMultiplier--;
        humanCounter--;
        taskingHumans.Remove(Human);
        positionShifts[Human.GetComponent<MovementManager>().tuberPositionShiftNumber].availability = true;
        foreach (GameObject hu in taskingHumans)
        {
            hu.GetComponent<HumanInfo>().taskTimeMultiplier = taskTimeMultiplier;
        }
    }

    public void GetPositionShift(GameObject Human)
    {
        for (int i = 0; i < positionShifts.Length; i++)
        {
            if (positionShifts[i].availability == true)
            {
                AssignPosition(Human, i);
            }
        }        
    }

    public void AssignPosition(GameObject Human, int positionNumber)
    {
        Human.GetComponent<MovementManager>().tuberPositionShift = positionShifts[positionNumber].position;
        Human.GetComponent<MovementManager>().tuberPositionShiftNumber = positionNumber;
        positionShifts[positionNumber].availability = false;
        
    }

    public void WorkOnTask()
    {
        timer += 1f;
    }

    public bool IsTaskDone()
    {
        Debug.Log("je hotovo " + humanCounter);
        return humanCounter == 0;
    }

    public void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}

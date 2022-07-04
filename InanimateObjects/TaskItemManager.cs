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
    public Vector3[] positionShifts = new[] {new Vector3(100f,0f,0f), new Vector3(0f,0f,100f), new Vector3(-40f, 0f, 0f), new Vector3(0f, 0f, -40f) };

    void Start()
    {
        taskCompleted = false;
        humanCounter = 0;
    }

    void Update()
    {
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

        foreach (GameObject hu in taskingHumans)
        {
            hu.GetComponent<HumanInfo>().taskTimeMultiplier = taskTimeMultiplier;
        }
    }

    public void WorkOnTask()
    {
        timer += 1f;
    }

    public void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HutManager : MonoBehaviour
{
    public Positions[] positions;
    public int distanceFromCenter = 7;
    public int humanCounter;
    public int humanLimit;
    public GameObject Player;
    public int tuberCounter;
    public int maxTuberNumber;

    void Start()
    {
        positions = new[] { new Positions(new Vector3(0,0,distanceFromCenter), true), new Positions(new Vector3(distanceFromCenter,0,distanceFromCenter), true),
            new Positions(new Vector3(-distanceFromCenter, 0, distanceFromCenter), true),new Positions(new Vector3(0, 0,-distanceFromCenter), true),
            new Positions(new Vector3(-distanceFromCenter,0,-distanceFromCenter), true), new Positions(new Vector3(distanceFromCenter,0,-distanceFromCenter), true) };
    }

    void Update()
    {
        
    }

    public bool IsFreeSpace()
    {
        if (humanCounter < humanLimit)
        {
            return true;
        }
        return false;
    }

    public Vector3 AddHuman()
    {
        //int spaceCounter = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            Positions position = positions[i];
            Debug.Log("volna pozice " + position.availability + " " + position.position);
            if (position.availability)
            {
                humanCounter++;
                if (humanCounter == humanLimit)
                {
                    Player.GetComponent<HutPanelController>().SleepButton.interactable = false;
                }
                positions[i].availability = false;
                return position.position;
            }
        }
        return new Vector3(0,0,0);
    }

    public void SubstractHuman()
    {

    }
    
    public void AddTubers(int numberOfTubers)
    {
        tuberCounter += numberOfTubers;
        Debug.Log("pocet tuberu " + tuberCounter + " " + numberOfTubers);
        Player.GetComponent<HutPanelController>().UpdateTuberCounter(tuberCounter, maxTuberNumber);
    }
    
    public void RemoveTuber()
    {
        tuberCounter--;
        Player.GetComponent<HutPanelController>().UpdateTuberCounter(tuberCounter, maxTuberNumber);
    }

}

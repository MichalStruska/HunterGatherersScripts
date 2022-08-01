using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HutManager : MonoBehaviour
{
    [SerializeField]
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

    public Vector3 GetEmptyHutPosition()
    {
        //int spaceCounter = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            Positions position = positions[i];
            if (position.availability)
            {
                
                if (humanCounter == humanLimit)
                {
                    Player.GetComponent<HutPanelController>().SleepButton.interactable = false;
                }
                positions[i].availability = false;
                return position.position;
            }
        }
        return new Vector3(0, 0, 0);
    }

    public void AddHuman(GameObject human)
    {
        FindEmptyPositionAndOcuppy(human);
        humanCounter++;
        CheckHutCapacityAndDisableSleepButton();
        Player.GetComponent<HutPanelController>().RefreshHumanPanel(gameObject);
    }

    public void RemoveHuman()
    {
        humanCounter--;
    }

    public void FindEmptyPositionAndOcuppy(GameObject human)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Positions position = positions[i];
            if (position.availability)
            {
                OcuppyPosition(i, human);
                return;
            }
        }
    }

    public void CheckHutCapacityAndDisableSleepButton()
    {
        if (humanCounter == humanLimit)
        {
            Player.GetComponent<HutPanelController>().SleepButton.interactable = false;
        }
    }

    public void OcuppyPosition(int positionNumber, GameObject human)
    {
        positions[positionNumber].availability = false;
        positions[positionNumber].human = human;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanThumbnailsManager : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> humans;

    [SerializeField]
    public List<HumanButtonStruct> humanButtons;


    void Start()
    {

    }

    void Update()
    {
        Debug.Log("Create thumbnail " + humans.Count);
    }

    public void SetThumbnails()
    {
        humans = Player.GetComponent<InputManager>().units;
        for (int i = 0; i < humans.Count; i++)
        {
            CreateThumbnailForHuman(humans[i]);
        }
    }

    public void CreateThumbnailForHuman(GameObject human)
    {
        //GameObject human = humans[humanNumber];
        
        GameObject ThumbnailObject = new GameObject();
        Image img = ThumbnailObject.AddComponent<Image>();

        img.sprite = human.GetComponent<HumanGUIHolder>().ThumbnailObject.GetComponent<SpriteRenderer>().sprite;
        human.GetComponent<HumanGUIHolder>().HumanThumbnailGeneral = Instantiate(img, Vector3.zero, Quaternion.identity);
        Image thumbnail = human.GetComponent<HumanGUIHolder>().HumanThumbnailGeneral;

        HumanButtonStruct humanButtonPair = new HumanButtonStruct();
        humanButtonPair.human = human;
        humanButtonPair.button = ThumbnailObject;

        AddButtonToObject(ThumbnailObject, humanButtonPair, human);

        ThumbnailObject.transform.SetParent(gameObject.transform, false);

        humanButtons.Add(humanButtonPair);
        Debug.Log("Created thumbnail");

    }

    public void AddButtonToObject(GameObject ThumbnailObject, HumanButtonStruct humanButtonPair, GameObject human)
    {
        Button button = ThumbnailObject.AddComponent<Button>();
   
        button.onClick.AddListener(delegate { GetHuman(human); });
    }

    public void GetHuman(GameObject human)
    {
        Debug.Log("kliknul " + human.name);
        Player.GetComponent<CameraManager>().GoToObject(human);
    }

    public void DeleteThumbnail(GameObject deadHuman)
    {
        //humanButtons.RemoveAt(humanNumber);
        //humanButtons[humanNumber].button.transform.parent.gameObject.SetActive(false);
        //Debug.Log("Created mrtvoly " + humanButtons[humanNumber].human.name);

        for (int i = 0; i < humanButtons.Count; i++)
        {
            if (deadHuman == humanButtons[i].human)
            {
                humanButtons[i].button.SetActive(false);
                humanButtons.RemoveAt(i);
            }
        }

    }

}

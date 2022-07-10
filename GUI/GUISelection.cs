using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct GameObjectWithTaskLoadingBar
{
    public GameObject gameObject;
    public GameObject slider;
    public bool empty;

    public GameObjectWithTaskLoadingBar(GameObject gameObject, GameObject slider, bool empty)
    {
        this.gameObject = gameObject;
        this.slider = slider;
        this.empty = empty;
    }
}

public class GUISelection : MonoBehaviour
{

    public List<GameObject> humans;
    public Camera mainCamera;
    public Camera huntCamera;
    public Plane humanPlane;
    //public GameObject MyImage;
    public Image MyImage;
    public GameObject myCanvas;
    public Image SelectionImageTemplate;
    public List<Image> images;
    public List<Image> targetImages;
    public List<Image> activeTargetImages;
    public List<bool> activeTargetImagesBool;
    public List<Vector3> targetPositions;
    public List<GameObject> selectedUnits;
    public float indicatorHeight = 30f;

    public Image TargetImage;
    public Image TargetImageTemplate;
    public Image ImgPok;

    public GameObject TaskLoadingBarTemplate;

    private Camera activeCamera;

    public Vector3 targetPosition;
    private Vector3 lastPos;

    public float screenCoeffVert;
    public float screenCoeffHor;

    public GameObject Player;

    public Color showInfoColor;
    public Color hideInfoColor;

    public GameObjectWithTaskLoadingBar[] taskLoadingBars;

    void Start()
    {
        humans = Player.GetComponent<InputManager>().units;
        images = new List<Image>();
        targetImages = new List<Image>();
        targetPositions = new List<Vector3>();

        taskLoadingBars = new GameObjectWithTaskLoadingBar[humans.Count];

        for (int i = 0; i < humans.Count; i++)
        {
            CreateSelectionIndicator(SelectionImageTemplate, gameObject, images, humans[i]);
            CreateTargetIndicator(TargetImageTemplate, gameObject, targetImages, humans[i]);
            CreateTaskLoadingBar(TaskLoadingBarTemplate, gameObject, i);
            targetPositions.Add(Vector3.zero);
            activeTargetImages.Add(null);
            activeTargetImagesBool.Add(true);
        }

        foreach (GameObjectWithTaskLoadingBar g in taskLoadingBars)
        {
            Debug.Log("je prazdny " + g.empty);
        }

        screenCoeffVert = (float)Screen.height / 416f;
        screenCoeffHor = (float)Screen.width / 442f; //442f;
    }

    void Update()
    {
        if (huntCamera.enabled)
        {
            activeCamera = huntCamera;
        }
        else
        {
            activeCamera = mainCamera;
        }

        selectedUnits = Player.GetComponent<InputManager>().selectedUnits;
        foreach (GameObject hu in selectedUnits)
        {            
            IndicatorPosition(hu.GetComponent<HumanInfo>().selectionIndicator.rectTransform, new Vector3(hu.transform.position.x, hu.transform.position.y + indicatorHeight, hu.transform.position.z));
            SetIndicatorSize(hu.GetComponent<HumanInfo>().selectionIndicator.rectTransform, new Vector3(hu.transform.position.x, hu.transform.position.y + indicatorHeight, hu.transform.position.z), 11f, 11f);
            IndicatorPosition(hu.GetComponent<HumanInfo>().targetIndicator.rectTransform, hu.GetComponent<HumanInfo>().targetIndicatorPosition);
            SetIndicatorSize(hu.GetComponent<HumanInfo>().targetIndicator.rectTransform, hu.GetComponent<HumanInfo>().targetIndicatorPosition, 11f, 11f);
            
        }


        foreach (GameObjectWithTaskLoadingBar taskLoadingBarObject in taskLoadingBars)
        {
            if (IsTaskLoadingBarActive(taskLoadingBarObject) == false)
            {
                UpdateTaskLoadingBarPosition(taskLoadingBarObject);
                UpdateTaskLoadingBarValue(taskLoadingBarObject);
            }
        }

        IfTaskIsDoneDeactivateSlider();

    }

    public void IndicatorPosition(RectTransform rectTransform, Vector3 originalPosition)
    {
        Vector3 screenPos = activeCamera.WorldToScreenPoint(originalPosition);
        rectTransform.anchoredPosition = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }

    public void SetIndicatorSize(RectTransform rectTransform, Vector3 originalPosition, float originalWidth, float originalHeight)
    {
        Vector3 newVector = originalPosition - activeCamera.transform.position;
        Plane targetPlane = new Plane(newVector, originalPosition);

        float distCoeff = 130 / Mathf.Abs(targetPlane.GetDistanceToPoint(activeCamera.transform.position));

        //float newSizeVert = rectTransform.rect.width * screenCoeffHor * distCoeff;
        //float newSizeHor = rectTransform.rect.height * screenCoeffHor * distCoeff;

        float newSizeVert = originalHeight * screenCoeffHor * distCoeff;
        float newSizeHor = originalWidth * screenCoeffHor * distCoeff;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSizeVert);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSizeHor);

    }

    public void CreateTargetIndicator(Image img, GameObject parent, List<Image> imgList, GameObject human)
    {
        human.GetComponent<HumanInfo>().targetIndicator = Instantiate(img, Vector3.zero, Quaternion.identity);
        Image targetIndicator = human.GetComponent<HumanInfo>().targetIndicator;

        targetIndicator.transform.SetParent(parent.transform, false);
        targetIndicator.rectTransform.anchoredPosition = Vector3.zero;
        targetIndicator.enabled = false;
        imgList.Add(targetIndicator);
    }
    
    public void CreateSelectionIndicator(Image img, GameObject parent, List<Image> imgList, GameObject human)
    {
        human.GetComponent<HumanInfo>().selectionIndicator = Instantiate(img, Vector3.zero, Quaternion.identity);
        Image selectionIndicator = human.GetComponent<HumanInfo>().selectionIndicator;

        selectionIndicator.transform.SetParent(parent.transform, false);
        selectionIndicator.rectTransform.anchoredPosition = Vector3.zero;
        selectionIndicator.enabled = false;
        setIndicatorColor(selectionIndicator, hideInfoColor);
        imgList.Add(selectionIndicator);
    }
    
    public void CreateTaskLoadingBar(GameObject slider, GameObject parent, int barNumber)
    {
        GameObject loadingBar = Instantiate(slider, Vector3.zero, Quaternion.identity);

        loadingBar.transform.SetParent(parent.transform, false);
        loadingBar.transform.position = new Vector3(50f,50f,50f);
        loadingBar.SetActive(false);

        taskLoadingBars[barNumber].empty = true;
        taskLoadingBars[barNumber].slider = slider;
        taskLoadingBars[barNumber].gameObject = null;
    }

    public void UpdateTaskLoadingBarPosition(GameObjectWithTaskLoadingBar taskLoadingBarObject)
    {

        SetIndicatorSize(taskLoadingBarObject.slider.GetComponent<RectTransform>(), taskLoadingBarObject.gameObject.transform.position, 30f, 11f);
        SetBarPosition(taskLoadingBarObject.slider, taskLoadingBarObject.gameObject.transform.position);
   
    }

    public void UpdateTaskLoadingBarValue(GameObjectWithTaskLoadingBar taskLoadingBarObject)
    {
        taskLoadingBarObject.slider.GetComponent<Slider>().value = (taskLoadingBarObject.gameObject.GetComponent<TaskItemManager>().timer / taskLoadingBarObject.gameObject.GetComponent<TaskItemManager>().timeLimit) * 100;
        Debug.Log("bar value je " + taskLoadingBarObject.gameObject.GetComponent<TaskItemManager>().timer + " " + taskLoadingBarObject.slider.GetComponent<Slider>().value);
    }

    private bool IsTaskLoadingBarActive(GameObjectWithTaskLoadingBar taskLoadingBarObject)
    {
        return taskLoadingBarObject.empty;
    }

    public void SetBarPosition(GameObject loadingBar, Vector3 worldPosition)
    {
        worldPosition.y += 20f;
        Vector3 screenPos = activeCamera.WorldToScreenPoint(worldPosition);
        loadingBar.transform.position = screenPos;
    }

    public void SetupTaskLoadingBar(GameObject workingObject)
    {
        for(int i = 0; i < taskLoadingBars.Length; i++)
        {
            if (IsTaskLoadingBarActive(taskLoadingBars[i]))
            {
                taskLoadingBars[i].gameObject = workingObject;
                taskLoadingBars[i].empty = false;
                taskLoadingBars[i].slider.SetActive(true);
                return;
            }
        }
    }

    public void IfTaskIsDoneDeactivateSlider()
    {
        for (int i = 0; i < taskLoadingBars.Length; i++)
        {
            if (taskLoadingBars[i].gameObject.activeSelf == false || taskLoadingBars[i].gameObject.GetComponent<TaskItemManager>().IsTaskDone())
            {
                taskLoadingBars[i].gameObject = null;
                taskLoadingBars[i].empty = true;
                taskLoadingBars[i].slider.SetActive(false);
            }
        }
    }


    public void setIndicatorColor(Image selectionIndicator, Color indicatorColor)
    {
        selectionIndicator.color = indicatorColor;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Camera activeCamera;

    public Vector3 targetPosition;
    private Vector3 lastPos;

    public float screenCoeffVert;
    public float screenCoeffHor;

    public GameObject Player;

    public Color showInfoColor;
    public Color hideInfoColor;

    void Start()
    {
        humans = Player.GetComponent<InputManager>().units;
        images = new List<Image>();
        targetImages = new List<Image>();
        targetPositions = new List<Vector3>();

        foreach (GameObject hu in humans)
        {

            CreateSelectionIndicator(SelectionImageTemplate, gameObject, images, hu);
            CreateTargetIndicator(TargetImageTemplate, gameObject, targetImages, hu);
            targetPositions.Add(Vector3.zero);
            activeTargetImages.Add(null);
            activeTargetImagesBool.Add(true);

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
            IndicatorPosition(hu.GetComponent<HumanInfo>().selectionIndicator, new Vector3(hu.transform.position.x, hu.transform.position.y + indicatorHeight, hu.transform.position.z), 11f);
            IndicatorPosition(hu.GetComponent<HumanInfo>().targetIndicator, hu.GetComponent<HumanInfo>().targetIndicatorPosition, 11f);
        }

    }

    public void IndicatorPosition(Image img, Vector3 originalPosition, float originalSize)
    {
        Vector3 screenPos = activeCamera.WorldToScreenPoint(originalPosition);
        
        Vector3 camPoint = activeCamera.WorldToViewportPoint(originalPosition);
       
        
        Vector3 newVector = originalPosition - activeCamera.transform.position;
        Plane targetPlane = new Plane(newVector, originalPosition);

        float distCoeff = 130 / Mathf.Abs(targetPlane.GetDistanceToPoint(activeCamera.transform.position));

        img.rectTransform.anchoredPosition = new Vector3(screenPos.x, screenPos.y, screenPos.z); ;
        float newSizeVert = originalSize  * screenCoeffHor * distCoeff;
        float newSizeHor = originalSize * screenCoeffHor * distCoeff;
        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSizeVert);
        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSizeHor);
        
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

    public void setIndicatorColor(Image selectionIndicator, Color indicatorColor)
    {
        selectionIndicator.color = indicatorColor;
    }

}

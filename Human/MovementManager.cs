using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class MovementManager : MonoBehaviour
{
    public int unitNumber = 0;
    public float walkSpeed;
    public float runSpeed;

    public RaycastHit hit;
    public Vector3 originalHitPoint;
    private Vector3 updatedHitPoint;

    public NavMeshAgent agent;

    bool oneClick = false;
    float timerForDoubleClick;
    private bool orderRun = false;

    public ActionList actionList;
    
    Vector3 followPoint;
    public float changeHuntingDirectionLimit;
    GameObject prey;
    public HumanTaskList humanTask;
    public GameObject MainCanvas;

    public int humanNumber;
    private float targetPrecision = 5f;
    private Vector3 hutPosition;

    public TMP_Dropdown GaitDropdown;
    public Vector3 shiftedPoint;
    private float randomTargetShift = 15;

    public float idleTime;

    private GameObject Player;
    public GameObject TargetObject;
    public bool startRotating;

    private bool isDoubleClick;
    private float doubleClickDelayLimit = 0.4f;

    public LayerMask layerMask;

    private HumanTaskList potentialTask;
    private HumanGaitList potentialGait;
    public float locomotionSpeed;
    private int gaitInterfaceValue;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        actionList = FindObjectOfType<ActionList>();

        agent.updateRotation = false;
        GetComponent<HumanInfo>().humanTask = HumanTaskList.Idle;

        Player = GameObject.Find("Player");
        idleTime = 0f;

        humanNumber = Player.GetComponent<ObjectsController>().AddHuman();

        walkSpeed = GetComponent<HumanGaitInfo>().minWalkSpeed;
        runSpeed = GetComponent<HumanGaitInfo>().minRunSpeed;
    }

    void Update()
    {
        Debug.Log("lovi " + shiftedPoint);
        humanTask = GetComponent<HumanInfo>().humanTask;

        GetComponent<MovementInfo>().GetRotation();
        if (MenuPanelController.isGamePaused)
        {
            //do nothing
        }

        else if (GetComponent<HumanInfo>().isHunting && unitNumber == 0)
        {
            shiftedPoint = prey.transform.position;
            UpdateSizeOfHuntingTarget();
            //CheckIfUpdateHuntingDirection();
            GetPositionOfHunterAndPrey();
            SetupTargetIndicator(prey.transform.position);
            
            //GoThere(shiftedPoint);
        }
        //else if (startRotating)
        //{
        //    Vector3 direction = (TargetObject.transform.position - transform.position).normalized;
        //    Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1);
        //}
        else if (Input.GetMouseButtonDown(1) && GetComponent<HumanInfo>().isSelected)
        {
            RightClick();
        }
    }

    private void UpdateSizeOfHuntingTarget()
    {
        changeHuntingDirectionLimit = Vector3.Distance(transform.position, shiftedPoint) * .1f;
    }

    public void CheckIfUpdateHuntingDirection()
    {
        if (Vector3.Distance(transform.position, shiftedPoint) > Random.Range(changeHuntingDirectionLimit, changeHuntingDirectionLimit + 20f))
        {
            UpdateHuntingDirection();
        }
    }
    
    public void UpdateHuntingDirection()
    {
        shiftedPoint = prey.transform.position;
        actionList.Move(agent, shiftedPoint, locomotionSpeed, unitNumber);
        //agent.destination = followPoint;
    }
    
    public void GetPositionOfHunterAndPrey()
    {
        Player.GetComponent<InputManager>().followedHunterPos = transform.position;
        Player.GetComponent<InputManager>().followedPreyTransform = prey.transform;
    }

    public bool IsDoubleClick()
    {
        if (!oneClick)
        {
            oneClick = true;
            timerForDoubleClick = Time.time;
            return false;
        }
        else if (oneClick && (Time.time - timerForDoubleClick) < doubleClickDelayLimit)
        {
            oneClick = false;
            return true;
        }
        else if (oneClick && (Time.time - timerForDoubleClick) > doubleClickDelayLimit)
        {
            oneClick = true;
            timerForDoubleClick = Time.time;
            return false;
        }
        return false;
    }

    public void RightClick()
    {
        isDoubleClick = IsDoubleClick();
        if (isDoubleClick)
        {
            orderRun = true;
            //ShowGaitPanel();
            GaitDropdown.gameObject.SetActive(true);
            gaitInterfaceValue = 0;
            GetComponent<HumanInfo>().SetSpeedSlider();
            locomotionSpeed = runSpeed;
            potentialTask = HumanTaskList.Running;
            potentialGait = HumanGaitList.Running;
        }
        else
        {
            orderRun = false;
            gaitInterfaceValue = 1;
            locomotionSpeed = walkSpeed;
            potentialTask = HumanTaskList.Walking;
            potentialGait = HumanGaitList.Walking;
        }
        ProcessClick();
    }

    void ProcessClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10000, ~layerMask))
        {
            WhatWasClickedOn();
        }
    }

    private void WhatWasClickedOn()
    {
        if (humanNumber == 0)
        {
            SetupGaitInterface(gaitInterfaceValue);
        }
        if (DidHitGround())
        {
            originalHitPoint = hit.point;
            GroundWalk();
        }
        else if (DidHitTuber())
        {
            TargetObject = hit.collider.gameObject;
            int humanTuberNumber = TargetObject.GetComponent<TaskItemManager>().humanCounter;
            if (humanTuberNumber < 4)
            {
                GoDigTheTuber(humanTuberNumber);
            }
        }
        else if (DidHitHut())
        {
            //HideGaitPanel();
            SetUpTargetTask(HumanTargetTaskList.Idle);
            TargetObject = hit.collider.gameObject;
            hutPosition = hit.collider.transform.position;
            Player.GetComponent<HutPanelController>().ShowPanel();
        }
        else if (DidHitAnimal())
        {
            prey = hit.transform.gameObject;
            SetupGaitInterface(gaitInterfaceValue);
            if (unitNumber == 0)
            {
                GetPositionOfHunterAndPrey();
            }
            //InvokeRepeating("UpdateHuntingDirection", 2f, 2f);
            Player.GetComponent<PanelController>().quitHuntButton.gameObject.SetActive(true);
            GetComponent<HumanInfo>().humanTask = potentialTask;
            GetComponent<HumanInfo>().isHunting = true;
            shiftedPoint = prey.transform.position;
            UpdateSizeOfHuntingTarget();
            SetupTargetIndicator(shiftedPoint);

            GoThere(shiftedPoint);
            //actionList.Move(agent, followPoint, locomotionSpeed, unitNumber);

            Player.GetComponent<InputManager>().followedPrey = prey;

        }
        //SetupGaitInterface(gaitInterfaceValue);
    }

    public void GroundWalk()
    {
        SetUpLocomotion();
        SetUpTargetTask(HumanTargetTaskList.Idle);
        
        if (humanNumber == 0)
        {
            SetupGaitInterface(gaitInterfaceValue);
        }
        GetIndividualTarget(originalHitPoint);
        GoThere(shiftedPoint);        
    }

    private void GoDigTheTuber(int humanTuberNumber)
    {
        TargetObject.GetComponent<TaskItemManager>().AddHuman(gameObject);
        SetUpLocomotion();
        SetUpTargetTask(HumanTargetTaskList.Digging);
        Vector3 tuberPosition = hit.collider.transform.position;
        shiftedPoint = tuberPosition + TargetObject.GetComponent<TaskItemManager>().positionShifts[humanTuberNumber];
        GoThere(new Vector3(originalHitPoint.x, originalHitPoint.y + 16, originalHitPoint.z));
    }

    private bool DidHitGround()
    {
        if (hit.collider.tag == "Ground")
        {
            return true;
        }
        return false;
    }

    private bool DidHitTuber()
    {
        if (hit.collider.tag == "Tuber")
        {
            return true;
        }
        return false;
    }
    
    private bool DidHitHut()
    {
        if (hit.collider.tag == "Hut")
        {
            return true;
        }
        return false;
    }
    
    private bool DidHitAnimal()
    {
        if (hit.collider.tag == "Animal")
        {
            return true;
        }
        return false;
    }

    public void SetUpLocomotion()
    {
        GetComponent<HumanInfo>().humanTask = potentialTask;
        GetComponent<HumanGaitInfo>().gait = potentialGait;
    }

    public void SetUpTargetTask(HumanTargetTaskList targetTask)
    {
        GetComponent<HumanInfo>().humanTargetTask = targetTask;
    }

    public void SetupGaitInterface(int gaitInterfaceValue)
    {
        ShowGaitPanel();
        GaitDropdown.gameObject.SetActive(true);
        ChangeGaitDropdown(gaitInterfaceValue);
        GetComponent<HumanInfo>().SetSpeedSlider();
    }

    IEnumerator OrderDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0f));
        actionList.Move(agent, shiftedPoint, locomotionSpeed, unitNumber);
    }

    public void GetIndividualTarget(Vector3 hitPoint)
    {
        float rand_num01 = Random.Range(-5.0f, 5.0f);
        shiftedPoint = new Vector3(hitPoint.x + 5 * unitNumber, hitPoint.y, hitPoint.z + Random.Range(-randomTargetShift, randomTargetShift));
    }

    void LateUpdate()
    {
        //transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    
    public void GoThere(Vector3 targetIndicatorPosition)
    {
        SetupGaitInterface(gaitInterfaceValue);
        SetupTargetIndicator(targetIndicatorPosition);
        SetUpLocomotion();
        GetComponent<HumanInfo>().hasTarget = true;
        StartCoroutine(OrderDelay());
    }

    public void SetupTargetIndicator(Vector3 targetIndicatorPosition)
    {
        GetComponent<HumanInfo>().targetIndicatorPosition = targetIndicatorPosition;
        GetComponent<HumanInfo>().targetIndicator.enabled = true;
    }

    public void TurnOffTargetIndicator()
    {
        GetComponent<HumanInfo>().targetIndicator.enabled = false;
    }

    public void ChangeGaitDropdown(int gaitValue)
    {
        GaitDropdown.value = gaitValue;
    }

    public void GoToHut()
    {
        originalHitPoint = hutPosition;
        shiftedPoint = new Vector3(hutPosition.x -20, hutPosition.y, hutPosition.z - 20);

        GoThere(new Vector3(hutPosition.x, hutPosition.y + 16, hutPosition.z));
      
        GetComponent<HumanInfo>().humanTask = HumanTaskList.Walking;
    }

    public void GoToSleep()
    {
        GetComponent<HumanInfo>().humanTargetTask = HumanTargetTaskList.Sleeping;
    }
    
    public void GoToEat()
    {
        GetComponent<HumanInfo>().humanTargetTask = HumanTargetTaskList.Eating;
    }

    public void GoBringResources()
    {
        SetUpTargetTask(HumanTargetTaskList.BringingResources);
    }

    public void HideGaitPanel()
    {
        Player.GetComponent<PanelController>().HidePanel(Player.GetComponent<PanelController>().HumanGaitPanel);
    }
    
    public void ShowGaitPanel()
    {
        Player.GetComponent<PanelController>().ShowPanel(Player.GetComponent<PanelController>().HumanGaitPanel);
    }

    public void QuitHunt()
    {
        if (GetComponent<HumanInfo>().isHunting)
        {
            GetComponent<HumanInfo>().isHunting = false;
            originalHitPoint = transform.position + new Vector3(10, 0, 10);
            //shiftedPoint = transform.position + new Vector3(4,0,4);
            GroundWalk();
            //actionList.Move(agent, shiftedPoint, locomotionSpeed, unitNumber);
            //GetComponent<HumanInfo>().humanTargetTask = HumanTargetTaskList.Idle;
            //GoThere(shiftedPoint);
            //CancelInvoke("UpdateHuntingDirection");
        }
    }
}

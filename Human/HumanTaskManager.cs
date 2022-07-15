using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTaskManager : MonoBehaviour
{

    public GameObject DiggingStick;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void QuitDigging()
    {
        DiggingStick.SetActive(false);
    }

}

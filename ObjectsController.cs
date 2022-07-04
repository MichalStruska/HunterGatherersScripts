using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
    public int humanCntr = -1;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AddHuman()
    {
        humanCntr++;
        Debug.Log("novy human " + humanCntr);
        return humanCntr;
    }
}

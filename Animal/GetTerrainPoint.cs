using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTerrainPoint : MonoBehaviour
{
    public LayerMask IgnoreLayer;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Vector3 GetNewPoint(Vector3 originalTargetPoint)
    {
        RaycastHit hit;
        Vector3 newTargetPoint = new Vector3();
        if (Physics.Linecast(new Vector3(originalTargetPoint.x, 1000, originalTargetPoint.z), new Vector3(originalTargetPoint.x, -100, originalTargetPoint.z), out hit, ~IgnoreLayer))
        {
            newTargetPoint = hit.point;
        }
        return newTargetPoint;
    }
}

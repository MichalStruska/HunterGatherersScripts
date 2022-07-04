using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputMethods
{
    public static bool GetRayOfCursorPosition(LayerMask layerMask, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            return true;
        }
        return false;
    }

}

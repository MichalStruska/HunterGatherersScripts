using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector3Array : SharedVariable<Vector3[]>
    {
        public static implicit operator SharedVector3Array(Vector3[] value) { return new SharedVector3Array { mValue = value }; }
    }
}

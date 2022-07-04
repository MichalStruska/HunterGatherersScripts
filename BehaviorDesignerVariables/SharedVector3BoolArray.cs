using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector3BoolArray : SharedVariable<(Vector3 X, bool Y)[]>
    {
        public static implicit operator SharedVector3BoolArray((Vector3 X, bool Y)[] value) { return new SharedVector3BoolArray { mValue = value }; }
    }
}

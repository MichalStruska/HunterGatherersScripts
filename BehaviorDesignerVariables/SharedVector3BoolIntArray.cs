using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector3BoolIntArray : SharedVariable<(Vector3 X, bool Y, int Z)[]>
    {
        public static implicit operator SharedVector3BoolIntArray((Vector3 X, bool Y, int Z)[] value) { return new SharedVector3BoolIntArray { mValue = value }; }
    }
}

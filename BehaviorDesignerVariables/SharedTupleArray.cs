using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedTupleArray : SharedVariable<(Vector3 X, int Y)[]>
    {
        public static implicit operator SharedTupleArray((Vector3 X, int Y)[] value) { return new SharedTupleArray { mValue = value }; }
    }
}

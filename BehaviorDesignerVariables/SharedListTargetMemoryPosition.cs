using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedListMemoryPosition : SharedVariable<List<MemoryPosition>>
    {
        public static implicit operator SharedListMemoryPosition(List<MemoryPosition> value) { return new SharedListMemoryPosition { mValue = value }; }
    }
}

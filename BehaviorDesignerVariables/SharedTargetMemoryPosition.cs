using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedMemoryPosition : SharedVariable<MemoryPosition>
    {
        public static implicit operator SharedMemoryPosition(MemoryPosition value) { return new SharedMemoryPosition { mValue = value }; }
    }
}

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame.Runtime.SequenceFrame
{
    public class SequenceFrameSO : ScriptableObject
    {
        [ReadOnly] public List<string> WalkWithForwardPath = new List<string>();
        [ReadOnly] public List<string> WalkWithRLPath = new List<string>();
        [ReadOnly] public List<string> WalkWithBelowPath = new List<string>();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.interaction
{
    public class Condition : ScriptableObject
    {
        public string description;
        public bool satisfied;
        public int hash;
    }
}

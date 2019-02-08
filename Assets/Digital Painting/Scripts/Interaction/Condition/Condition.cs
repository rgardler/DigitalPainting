using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.interaction
{
    public class Condition : ScriptableObject
    {
        public string description;
        public int hash;

        public bool satisfied;        
    }
}

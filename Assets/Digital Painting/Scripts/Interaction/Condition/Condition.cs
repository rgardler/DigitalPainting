using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.interaction
{
    public class Condition : ScriptableObject
    {
        public string description;
        public bool _satisfied;
        public int hash;
        
        public bool Satisfied
        {
            get { return _satisfied;  }
            set { _satisfied = value; }
        }
    }
}

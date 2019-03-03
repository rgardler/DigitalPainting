using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class Condition : ScriptableObject
    {
        public string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public int _hash = int.MinValue;
        public int Hash
        {
            get
            {
                if (_hash == int.MinValue)
                {
                    _hash = Animator.StringToHash(Description);
                }
                return _hash;
            }
        }

        public bool _satisfied;

        virtual public bool Satisfied(BaseAgentController interactor, Interactable interactable)
        {
            return _satisfied;
        }

        /// <summary>
        /// Reset the condition to its default values.
        /// </summary>
        public void Reset()
        {
            _satisfied = false;
        }
    }
}

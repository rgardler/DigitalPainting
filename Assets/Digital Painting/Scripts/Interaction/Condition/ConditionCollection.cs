using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class ConditionCollection : ScriptableObject
    {
        public string description;
        public Condition[] requiredConditions = new Condition[0];

        public bool CheckAndReact(BaseAgentController interactor, Interactable interactable)
        {
            for(int i = 0; i < requiredConditions.Length; i++)
            {
                if (!AllConditions.CheckCondition(requiredConditions[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

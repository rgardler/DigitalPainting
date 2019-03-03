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

        /// <summary>
        /// Check to see if conditions for interaction are passed.
        /// </summary>
        /// <param name="interactor">The agent that may perform the interaction.</param>
        /// <param name="interactable">The Thing upon which the agent may perform the interaction.</param>
        /// <returns>Tue if all required conditions pass.</returns>
        public bool CheckValidInteraction(BaseAgentController interactor, Interactable interactable)
        {
            for(int i = 0; i < requiredConditions.Length; i++)
            {
                if (!AllConditions.CheckCondition(requiredConditions[i], interactor, interactable))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

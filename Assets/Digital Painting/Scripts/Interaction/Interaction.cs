using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class Interaction : ScriptableObject
    {
        public string Description;
        public PlayableAsset playableAsset;
        public ConditionCollection requiredConditions;
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

        /// <summary>
        /// Check to see if conditions for interaction are passed.
        /// </summary>
        /// <param name="interactor">The agent that may perform the interaction.</param>
        /// <param name="interactable">The Thing upon which the agent may perform the interaction.</param>
        /// <returns>Tue if all required conditions pass.</returns>
        public bool CheckValidInteraction(BaseAgentController interactor, Interactable interactable)
        {
            for (int i = 0; i < requiredConditions.conditions.Length; i++)
            {
                if (!requiredConditions.CheckCondition(requiredConditions.conditions[i], interactor, interactable))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reset the condition to its default values.
        /// </summary>
        public void Reset()
        {
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.interaction;

namespace wizardscode.interaction
{
    [CreateAssetMenu(fileName = "ConditionCollection", menuName = "Wizards Code/Interactions/Conditions/Collection", order = 1000)]
    public class ConditionCollection : ResettableScriptableObject
    {
        public Condition[] conditions = new Condition[0];

        public override void Reset()
        {
            if (conditions == null)
            {
                return;
            }

            for (int i = 0; i < conditions.Length; i++)
            {
                conditions[i].Reset();
            }
        }

        public bool CheckCondition (Condition required, BaseAgentController interactor, Interactable interactable)
        {
            bool satisfied = true;
            
            if (conditions != null && conditions[0] != null)
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i].Hash == required.Hash)
                    {
                        satisfied = satisfied && conditions[i].Satisfied(interactor, interactable);
                    }
                }
            }

            return satisfied;
        }
    }
}
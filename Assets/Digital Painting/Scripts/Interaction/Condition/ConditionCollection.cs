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
        public ReactionCollection reactionCollection;

        public bool CheckAndReact(BaseAgentController interactor, Interactable interactable)
        {
            for(int i = 0; i < requiredConditions.Length; i++)
            {
                if (!AllConditions.CheckCondition(requiredConditions[i]))
                {
                    return false;
                }
            }

            if(reactionCollection != null) {
                bool canReact = true;
                for (int i = 0; i < reactionCollection.reactions.Length; i++)
                {
                    AbilityTrigger abilityTrigger = reactionCollection.reactions[i] as AbilityTrigger;
                    if (abilityTrigger != null && !interactor.abilities.Contains(abilityTrigger.ability))
                    {
                        canReact = false;
                    }
                }

                if (canReact)
                {
                    reactionCollection.React(interactor, interactable);
                    return true;
                } else
                {
                    return false;
                }
            }

            return true;
        }
    }
}

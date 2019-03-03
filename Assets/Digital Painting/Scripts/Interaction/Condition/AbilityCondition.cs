using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.ability;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class AbilityCondition : Condition
    {

        public Ability ability;

        public override bool Satisfied(BaseAgentController interactor, Interactable interactable)
        {
            if (interactor.HasAbility(ability))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}

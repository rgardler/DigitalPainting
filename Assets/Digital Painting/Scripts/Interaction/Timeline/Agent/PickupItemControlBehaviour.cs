using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class PickupItemControlBehaviour : PlayableBehaviour
    {
        public Ability ability;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            LevitateItemSpell spell = ability as LevitateItemSpell;
            if (spell != null)
            {
                spell.Update();
            }
            else
            {
                if (ability == null)
                {
                    throw new MisconfiguredAbilityException("No Ability is specified.");
                }
                throw new MisconfiguredAbilityException(ability.name + " is not recognized as an ability with which to pickup items");
            }
        }
    }
}
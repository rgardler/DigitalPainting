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

        private bool isFirstFrame = true;
        private bool isComplete = false;
        private DigitalPaintingManager manager;
        private LevitateItemSpell spell;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {   
            if (isFirstFrame)
            {
                manager = playerData as DigitalPaintingManager;
                spell = ability as LevitateItemSpell;

                if (spell != null)
                {
                    spell.endTransform = manager.AgentWithFocus.equippedMountPoint;
                    spell.item = manager.AgentWithFocus.Interactable;
                    spell.Start();
                    isFirstFrame = false;
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

            if (spell.isActive)
            {
                spell.Update();
            }
            else if (!isComplete)
            {
                manager.AgentWithFocus.Using = manager.AgentWithFocus.Interactable;
                isComplete = true;
            }
        }
    }
}
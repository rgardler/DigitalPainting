using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.inventory;

namespace wizardscode.interaction
{
    public class DropItemControlBehaviour : PlayableBehaviour
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
                    spell.item = manager.AgentWithFocus.Using;
                    spell.endTransform = manager.AgentWithFocus.Interactable.transform;
                    spell.Start();
                    isFirstFrame = false;
                }
                else { 
                    if (ability == null)
                    {
                        throw new MisconfiguredAbilityException("No Ability is specified.");
                    }
                    throw new MisconfiguredAbilityException(ability.name + " is not recognized as an ability with which to drop items");
                }
            }

            if (spell.isActive)
            {
                spell.Update();
            }
            else if (!isComplete)
            {
                Interactable item = manager.AgentWithFocus.Using;
                manager.AgentWithFocus.Unequip(false);

                InventoryManager inventory = manager.AgentWithFocus.Interactable.GetComponent<InventoryManager>();
                if (inventory != null)
                {
                    inventory.AddItem(item);
                }
                isComplete = true;
            }
        }
    }
}
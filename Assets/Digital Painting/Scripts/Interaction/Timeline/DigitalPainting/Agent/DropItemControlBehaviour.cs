using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.inventory;

namespace wizardscode.interaction.agent
{
    public class DropItemControlBehaviour : BaseSpellPlayableBehaviour<LevitateItemSpell>
    {
        InventoryManager targetInventory;

        internal override void Initialize(Playable playable, FrameData info, object playerData)
        {
            base.Initialize(playable, info, playerData);

            spell.item = agent.Using;
            spell.endTransform = agent.Interactable.transform;
            targetInventory = agent.Interactable.GetComponent<InventoryManager>();
        }

        internal override void Complete(Playable playable, FrameData info, object playerData)
        {
            base.Complete(playable, info, playerData);

            Interactable item = agent.Using;
            agent.Unequip(false);

            if (targetInventory != null)
            {
                targetInventory.AddItem(item);
            }
        }
    }
}
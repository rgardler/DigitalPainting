using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.inventory;

namespace wizardscode.interaction.agent
{
    public class InventoryAddItemControlBehaviour : BaseAbilityPlayableBehaviour
    {
        internal override void DigitalPaintingFrame(Playable playable, FrameData info, object playerData)
        {
            if (!isComplete)
            {
                agent.Unequip();
                Complete(playable, info, playerData);
            }
        }
    }
}
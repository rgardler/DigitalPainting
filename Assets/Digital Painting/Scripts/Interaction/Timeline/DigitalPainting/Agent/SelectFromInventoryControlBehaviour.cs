using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction.agent
{
    public class SelectFromInventoryControlBehaviour : BaseAbilityPlayableBehaviour
    {
        internal override void DigitalPaintingFrame(Playable playable, FrameData info, object playerData)
        {
            if (agent.Using == null) {
                agent.Using = agent.Inventory.GetItem(0);
            }
            else
            {
                Complete(playable, info, playerData);
            }
        }
    }
}
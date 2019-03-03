using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction.agent
{
    public class SelectFromInventoryControlBehaviour : PlayableBehaviour
    {
        private bool isFirstFrame = true;
        private DigitalPaintingManager manager;
        private BaseAgentController agent;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (isFirstFrame)
            {
                manager = playerData as DigitalPaintingManager;
                agent = manager.AgentWithFocus;
                isFirstFrame = false;
            }

            if (agent.Using == null) {
                agent.Using = agent.Inventory.GetItem(0);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.inventory;

namespace wizardscode.interaction
{
    public class InventoryAddItemControlBehaviour : PlayableBehaviour
    {
        bool isStored = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!isStored)
            {
                DigitalPaintingManager manager = playerData as DigitalPaintingManager;

                BaseAgentController agent = manager.AgentWithFocus;
                Interactable item = agent.PointOfInterest.gameObject.GetComponent<Interactable>();
                InventoryManager inventory = agent.GetComponent<InventoryManager>();
                inventory.AddItem(item);
                item.gameObject.SetActive(false);
            }
        }
    }
}
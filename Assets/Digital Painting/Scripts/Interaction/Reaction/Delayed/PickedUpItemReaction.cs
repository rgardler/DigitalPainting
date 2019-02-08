using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.inventory;

namespace wizardscode.interaction
{
    public class PickedUpItemReaction : DelayedReaction
    {
        public InventoryItem item;

        private InventoryManager inventory;

        protected override void SpecificInit()
        {
            inventory = FindObjectOfType<InventoryManager>();
        }

        protected override void ImmediateReaction(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            inventory.AddItem(item);
        }
    }
}

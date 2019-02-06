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

        protected override void ImmediateReaction()
        {
            inventory.AddItem(item);
        }
    }
}

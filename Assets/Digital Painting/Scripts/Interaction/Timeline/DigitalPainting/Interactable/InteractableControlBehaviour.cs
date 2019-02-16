using UnityEngine.Playables;
using wizardscode.digitalpainting;

namespace wizardscode.interaction
{
    public class InteractableControlBehaviour : PlayableBehaviour
    {
        public bool activeState;

        private bool isFirstFrame = true;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (isFirstFrame)
            {
                DigitalPaintingManager manager = playerData as DigitalPaintingManager;
                manager.AgentWithFocus.Interactable.gameObject.SetActive(activeState);
                isFirstFrame = false;
            }
        }
    }
}
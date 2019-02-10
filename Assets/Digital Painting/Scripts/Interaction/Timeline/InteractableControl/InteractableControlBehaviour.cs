using UnityEngine.Playables;

namespace wizardscode.interaction
{
    public class InteractableControlBehaviour : PlayableBehaviour
    {
        public bool activeState;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Interactable interactable = playerData as Interactable;
            interactable.gameObject.SetActive(activeState);
        }
    }
}
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class GameObjectReaction : DelayedReaction
    {
        public GameObject gameObject;
        public bool activeState;

        protected override void ImmediateReaction(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            gameObject.SetActive(activeState);
        }
    }
}

using UnityEngine;
using System.Collections;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public abstract class DelayedReaction : Reaction
    {
        public float delay;
        protected WaitForSeconds wait;

        public new void Init()
        {
            wait = new WaitForSeconds(delay);

            SpecificInit();
        }

        public new void React(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            monoBehaviour.StartCoroutine(ReactCoroutine(monoBehaviour, interactor, interactable));
        }

        protected IEnumerator ReactCoroutine(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            yield return wait;
            ImmediateReaction(monoBehaviour, interactor, interactable);
        }
    }
}

using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public abstract class Reaction : ScriptableObject
    {
        public void Init()
        {
            SpecificInit();
        }

        protected virtual void SpecificInit()
        { }

        public void React(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            ImmediateReaction(monoBehaviour, interactor, interactable);
        }

        protected abstract void ImmediateReaction(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable);
    }
}

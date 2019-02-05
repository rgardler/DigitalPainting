using UnityEngine;

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

        public void React(MonoBehaviour monoBehaviour)
        {
            ImmediateReaction();
        }

        protected abstract void ImmediateReaction();
    }
}

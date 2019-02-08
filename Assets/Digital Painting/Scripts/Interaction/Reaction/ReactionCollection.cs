using System.Collections.Generic;
using UnityEngine;
using wizardscode.ability;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class ReactionCollection : MonoBehaviour
    {
        [Tooltip("The set of reactions that will occur if this collection is triggered.")]
        public Reaction[] reactions = new Reaction[0];

        private Interactable interactable;

        private void Start()
        {
            for (int i = 0; i < reactions.Length; i++)
            {
                DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

                if (delayedReaction)
                    delayedReaction.Init();
                else
                    reactions[i].Init();
            }
        }

        public void React(BaseAgentController interactor, Interactable interactable)
        {
            for (int i = 0; i < reactions.Length; i++)
            {
                DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

                if (delayedReaction)
                    delayedReaction.React(this, interactor, interactable);
                else
                    reactions[i].React(this, interactor, interactable);
            }
        }
    }
}

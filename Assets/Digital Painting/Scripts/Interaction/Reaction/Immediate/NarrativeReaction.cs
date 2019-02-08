using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    public class NarrativeReaction : Reaction
    {
        public string message;
        public Color textColor = Color.white;
        public float delay;

        private TextManager textManager;

        protected override void SpecificInit()
        {
            textManager = FindObjectOfType<TextManager>();
            if (textManager == null)
            {
                Debug.LogError("You have a TextReaction but there is no TextManager in the scene.");
            }
        }

        protected override void ImmediateReaction(MonoBehaviour monoBehaviour, BaseAgentController interactor, Interactable interactable)
        {
            textManager.DisplayMessage(message, textColor, delay);
        }
    }
}
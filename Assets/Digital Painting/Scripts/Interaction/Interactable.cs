using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.interaction
{
    public class Interactable : MonoBehaviour
    {
        public Transform interactionLocation;
        public ConditionCollection[] conditionCollections = new ConditionCollection[0];
        public ReactionCollection defaultReactionCollection;

        public void Interact()
        {
            if (conditionCollections.Length == 0)
            {
                defaultReactionCollection.React();
            }

            for (int i = 0; i < conditionCollections.Length; i++)
            {
                if (conditionCollections[i].CheckAndReact())
                {
                    return;
                }

                defaultReactionCollection.React();
            }
        }
    }
}

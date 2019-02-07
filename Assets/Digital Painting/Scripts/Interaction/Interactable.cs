using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.ability;

namespace wizardscode.interaction
{
    public class Interactable : MonoBehaviour
    {
        public Transform interactionLocation;
        public ConditionCollection[] conditionCollections = new ConditionCollection[0];
        public ReactionCollection defaultReactionCollection;
        
        /// <summary>
        /// Interact with the interactable using an ability.
        /// 
        /// The correct collection of reactions will be identified by checking
        /// the conditions for each collection. When the conditions are satisfied
        /// the ability and reactions will be executed.
        /// </summary>
        /// <param name="interactor">The GameObject using the ability to interact.</param>
        /// <param name="ability">The ability to perform at the indicated spot within the ReactionsCollection.</param>
        public void Interact(GameObject interactor = null, Ability ability = null)
        {
            if (ability != null && interactor != null)
            {
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("endTransform", interactor.transform);
                StartCoroutine(ability.TriggerAbility(this.gameObject, options));
            }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Indicates that a Thing is interactable and manages all the possible interactions with the thing.
/// </summary>
namespace wizardscode.environment.thing {
    public class Interactable : MonoBehaviour
    {
        [Tooltip("All the interactions that may be performed on this Thing.")]
        public List<AbstractInteraction> interactions = new List<AbstractInteraction>();

        private void Awake()
        {
            bool hasTrigger = false;
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                if (collider.isTrigger)
                {
                    hasTrigger = true;
                    break;
                }
            }
            if (!hasTrigger)
            {
                Debug.LogWarning("You have an `Interactable` component attached to '" + gameObject.name + "' but it does not have a trigger collider. One is required and thus the component has been disabled.");
                this.enabled = false;
            }
        }
    }
}

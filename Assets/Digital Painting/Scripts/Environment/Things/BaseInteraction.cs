using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment.thing
{
    [Serializable]
    public class BaseInteraction : MonoBehaviour
    {
        private InteractionManager manager;

        protected void Awake()
        {
            manager = GetComponent<InteractionManager>();
            if (manager == null)
            {
                Debug.LogWarning("You have attached an interaction of " + name + " to " +
                    gameObject.name + " but there is no `InteractionManager` component attached. " +
                    "Since one is require the interaction is being disabled.");
                enabled = false;
            }
        }

        protected void Start()
        {
            manager.Add(this);
        }

        /// <summary>
        /// Called whenever a game object moves within the interaction trigger zone for a Thing.
        /// </summary>
        /// <param name="interactor"></param>
        public virtual void Interact(GameObject interactor) {
            Debug.LogWarning("You appear to have enabled " + interactor.name + " to interact with " +
                this.name + " but have not implemented the `Interact` method on the Interaction");
        }
    }
}

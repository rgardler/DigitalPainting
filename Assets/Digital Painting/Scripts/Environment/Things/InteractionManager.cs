using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using wizardscode.digitalpainting.agent;

/// <summary>
/// Indicates that a Thing is interactable and manages all the possible interactions with the thing.
/// </summary>
namespace wizardscode.environment.thing {
    public class InteractionManager : MonoBehaviour
    {
        protected List<BaseInteraction> interactions = new List<BaseInteraction>();
        private BaseAgentController agent;

        private void Awake()
        {
            agent = gameObject.GetComponent<BaseAgentController>();
            if (agent == null)
            {
                agent = gameObject.GetComponentInParent<BaseAgentController>();
                if (agent == null)
                {
                    Debug.LogWarning("You have an `InteractionManager` component to '" + gameObject.name + "' but it does not have a `BaseAgentController` in the same GameObject or its parent. One is required and thus the component has been disabled.");
                }
            }
        }

        internal void Add(BaseInteraction interaction)
        {
            interactions.Add(interaction);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Triggered interaction zone for " + agent.name + " triggered by " + other.gameObject.name);          
        }
    }
}

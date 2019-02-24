using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction
{
    /// <summary>
    /// An InteractionPoint is a location from which an Agent can interact
    /// with a thing. 
    /// </summary>
    public class InteractionPoint : MonoBehaviour
    {
        Interactable interactable;

        private void Start()
        {
            interactable = GetComponentInParent<Interactable>();
            if (interactable == null)
            {
                Debug.LogError("Interactable component cannot find the Interactable Thing it is for.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            BaseAgentController agent = other.gameObject.GetComponentInParent<BaseAgentController>();
            if (agent != null)
            {
                agent.Interactable = interactable;
                interactable.Interact(agent);
            }
        }
    }
}

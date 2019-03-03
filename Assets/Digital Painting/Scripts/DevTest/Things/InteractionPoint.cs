using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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

            bool hasTrigger = false;
            Collider[] colliders = GetComponentsInParent<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isTrigger)
                {
                    hasTrigger = true;
                    break;
                }
            }
            if (!hasTrigger)
            {
                Debug.LogError(gameObject.name + " has the InteractionPoint component but elect `Createit does not have a collider that is set to trigger so it will not work correctly.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            BaseAgentController agent = other.gameObject.GetComponentInParent<BaseAgentController>();
            if (agent != null && interactable.playableDirector.state != PlayState.Playing)
            {
                agent.Interactable = interactable;
                interactable.Interact(agent);
            }
        }
    }
}

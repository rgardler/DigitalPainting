using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;

namespace wizardscode.agent.ai
{
    public class CollectorBehaviour : MonoBehaviour
    {
        AIAgentController agent;

        private void Awake()
        {
            agent = GetComponent<AIAgentController>();
            if (agent == null)
            {
                Debug.LogError(gameObject.name + " has a CollectorBehaviour component attached, but it does not have a AIAgentController attached, which is required. Disabling the CollectorBehaviour.");
                this.enabled = false;
            }
        }

        private void Update()
        {
            List<Collectable> objects = agent.WithinReach<Collectable>();
            if ( objects.Count > 0) {
                Debug.Log(gameObject.name + " is within reach of " + objects.Count + " collectable items.");
            }
        }
    }
}

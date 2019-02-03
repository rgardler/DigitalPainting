using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;

namespace wizardscode.agent.ai
{
    public class CollectorBehaviour : MonoBehaviour
    {
        [Tooltip("The range in which an object is considered worth diverting for, " +
            "that is if the agent comes within this range of an object it will change" +
            "its `ThingOfInterest` and move towards the collectable.")]
        public float collectableRange = 10;

        AIAgentController controller;
        float checkFrequency = 2f;
        float timeUntilNextCheck = 0;

        private void Awake()
        {
            controller = GetComponent<AIAgentController>();
            if (controller == null)
            {
                Debug.LogError(gameObject.name + " has a CollectorBehaviour component attached, but it does not have a AIAgentController attached, which is required. Disabling the CollectorBehaviour.");
                this.enabled = false;
            }
        }

        private void Update()
        {
            timeUntilNextCheck -= Time.deltaTime;
            if (timeUntilNextCheck <= 0)
            {
                List<Collectable> objects = controller.WithinRange<Collectable>(collectableRange);
                if (objects.Count > 0)
                {
                    Debug.Log(gameObject.name + " is within range of " + objects.Count + " collectable items.");
                    Thing home = controller.home.GetComponent<Thing>();
                    if (controller.nextThings.Count == 0 && controller.ThingOfInterest != home)
                    {
                        controller.nextThings.Add(home);
                    }
                    else if (controller.nextThings.Count == 0 || controller.nextThings[0] != home)
                    {
                        controller.nextThings.Insert(0, home);
                    }
                }
                timeUntilNextCheck = checkFrequency;
            }
        }
    }
}

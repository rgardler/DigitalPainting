using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;
using wizardscode.interaction;

namespace wizardscode.agent.ai
{
    public class CollectorBehaviour : MonoBehaviour
    {
        [Tooltip("The range in which an object is considered worth diverting for, " +
            "that is if the agent comes within this range of an object it will change " +
            "its `ThingOfInterest` and move towards the collectable.")]
        public float collectableRange = 10;
        [Tooltip("The number of items an agent should seek to collect. The agent will " +
            "keep returning and collecting more items until either there are no items left " +
            "or they have this many in store.")]
        public int optimalNumberToCollect = 2;

        AIAgentController controller;
        float checkFrequency = 2f;
        float timeUntilNextCheck = 0;
        int numberScheduledToCollect = 0;

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
            if (numberScheduledToCollect >= optimalNumberToCollect)
            {
                return;
            }

            timeUntilNextCheck -= Time.deltaTime;
            if (timeUntilNextCheck <= 0)
            {
                List<Thing> objects = controller.WithinRange<Thing>(collectableRange);
                if (objects.Count > 0)
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        if (objects[i].GetComponentInParent<Interactable>() == null)
                        {
                            break;
                        }

                        if (!controller.nextThings.Contains(objects[i])) {
                            controller.nextThings.Insert(0, objects[i]);
                            numberScheduledToCollect++;

                            Thing home = controller.home.GetComponent<Thing>();
                            if (controller.nextThings.Count == 0 && controller.PointOfInterest != home)
                            {
                                controller.nextThings.Add(home);
                            }
                            else if (controller.nextThings.Count == 0 || controller.nextThings[0] != home)
                            {
                                controller.nextThings.Insert(1, home);
                            }
                        }
                    }
                }
                timeUntilNextCheck = checkFrequency;
            }
        }
    }
}

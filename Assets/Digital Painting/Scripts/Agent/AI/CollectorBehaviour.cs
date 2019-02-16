using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;
using wizardscode.interaction;
using wizardscode.inventory;

namespace wizardscode.agent.ai
{
    /// <summary>
    /// The CollectorBehaviour has an agent search for and, when found, collect items of
    /// interest. The default behaviour will have them collect an optimal number of items before
    /// returning to a place they can store items longer term, such as their home. 
    /// </summary>
    public class CollectorBehaviour : MonoBehaviour
    {
        [Tooltip("The range in which an object is considered worth diverting for, " +
            "that is if the agent comes within this range of an object it will change " +
            "its `ThingOfInterest` and move towards the collectable.")]
        public float collectableRange = 10;
        [Tooltip("The number of items an agent should seek to carry. The agent will " +
            "not return items to long term storage until they have this many in personal storage.")]
        public int optimalNumberToCarry = 1;
        [Tooltip("The number of items an agent should seek to collect. The agent will " +
            "keep returning and collecting more items until either there are no items left " +
            "or they have this many in their personal store + any long term storage.")]
        public int optimalNumberToCollect = 2;

        AIAgentController agent;
        InventoryManager personalInventory;
        float checkFrequency = 2f;
        float timeUntilNextCheck = 0;
        int numberScheduledToCollect = 0;

        private void Awake()
        {
            agent = GetComponent<AIAgentController>();
            if (agent == null)
            {
                Debug.LogError(gameObject.name + " has a CollectorBehaviour component attached, but it does not have a AIAgentController attached, which is required.");
            }

            personalInventory = agent.GetComponent<InventoryManager>();
            if (personalInventory == null)
            {
                Debug.LogError(gameObject.name + " has a CollectorBehaviour but does not have an InventoryManage, which is required.");
            }
        }

        private void Update()
        {
            // If we are carrying more than we need, return some to base
            if (personalInventory.Count > optimalNumberToCarry)
            {
                if (agent.PointOfInterest != agent.home)
                {
                    if (agent.nextThings.Count == 0)
                    {
                        agent.nextThings.Add(agent.home);
                    }
                    else if (!GameObject.ReferenceEquals(agent.nextThings[0], agent.home))
                    {
                        agent.nextThings.Insert(0, agent.home);
                    }
                }
            }

            // if we already know about enough to get us to the optimal level we ignore this
            if (personalInventory.Count + numberScheduledToCollect >= optimalNumberToCollect + optimalNumberToCarry)
            {
                return;
            }

            // if we see any items to collect add them to the list
            timeUntilNextCheck -= Time.deltaTime;
            if (timeUntilNextCheck <= 0)
            {
                List<Thing> objects = agent.WithinRange<Thing>(collectableRange);
                if (objects.Count > 0)
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        if (objects[i].GetComponentInParent<Interactable>() == null)
                        {
                            continue;
                        }

                        if (!agent.nextThings.Contains(objects[i])) {
                            agent.nextThings.Insert(0, objects[i]);
                            numberScheduledToCollect++;
                        }
                    }
                }
                timeUntilNextCheck = checkFrequency;
            }
        }
    }
}

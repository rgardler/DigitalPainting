using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public class ThingsManager : MonoBehaviour
    {
        [Header("World Data")]
        [Tooltip("The worlds collection of interesting things.")]
        public List<Thing> allTheThings;

        // Use this for initialization
        void Awake()
        {
            if (allTheThings == null)
            {
                allTheThings = new List<Thing>();
            }

            // Ensure all Things in the world are available in our collection
            Thing[] worldThings = FindObjectsOfType<Thing>();
            foreach (Thing thing in worldThings) 
            {
                if (thing.isPOI)
                {
                    allTheThings.Add(thing);
                }
            }            
        }
    }
}

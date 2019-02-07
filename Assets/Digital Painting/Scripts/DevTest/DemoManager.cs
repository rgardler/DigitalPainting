using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;

namespace wizardscode.devtest
{
    public class DemoManager : MonoBehaviour
    {
        [Tooltip("Prefab for Demo UI")]
        public GameObject demoUI;
        [Tooltip("First POI to head to.")]
        public Thing firstPOI;

        DigitalPaintingManager manager;

        private void Awake()
        {
            manager = FindObjectOfType<DigitalPaintingManager>();
        }

        void Start()
        {
            // Place the demo UI in the scene	
            GameObject.Instantiate(demoUI);
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject go = new GameObject("EventSystem");
                go.AddComponent<EventSystem>();
                go.AddComponent<StandaloneInputModule>();
            }

            ((AIAgentController)manager.AgentWithFocus.GetComponent<BaseAgentController>()).PointOfInterest = firstPOI;
        }
    }
}

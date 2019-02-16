using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;

namespace wizardscode.devtest
{
    public class DemoManager : PlayTestManager
    {
        [Tooltip("First POI to head to.")]
        public Thing firstPOI;

        protected override void Start()
        {
            base.Start();

            ((AIAgentController)manager.AgentWithFocus.GetComponent<BaseAgentController>()).PointOfInterest = firstPOI;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.interaction;

namespace wizardscode.interaction
{
    [CreateAssetMenu(fileName = "AllInteractions", menuName = "Wizards Code/Interactions/All Interactions", order = 990)]
    public class AllInteractions : ResettableScriptableObject
    {
        public Interaction[] interactions;

        private static AllInteractions instance;

        private const string loadPath = "AllInteractions";

        public static AllInteractions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AllInteractions>();
                }
                if (instance == null)
                {
                    instance = Resources.Load<AllInteractions>(loadPath);
                }
                if (instance == null)
                {
                    Debug.LogError("AllInteractionss has not been created yet. Go to Assets > Create > Wizards Code > Interactions > All Interactions. The resulting object must be stored in Resources/AllInteractions and you will need to add at least one interaction to it in the inspector.");
                }
                return instance;
            }
            set { instance = value; }
        }

        public override void Reset()
        {
            if (interactions == null)
            {
                return;
            }

            for (int i = 0; i < interactions.Length; i++)
            {
                interactions[i].Reset();
            }
        }
    }
}
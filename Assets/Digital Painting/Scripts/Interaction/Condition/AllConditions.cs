﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;
using wizardscode.interaction;

namespace wizardscode.interaction
{
    [CreateAssetMenu(fileName = "AllConditions", menuName = "Wizards Code/Interactions/All Conditions", order = 1000)]
    public class AllConditions : ResettableScriptableObject
    {
        public Condition[] conditions;

        private static AllConditions instance;

        private const string loadPath = "AllConditions";

        public static AllConditions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AllConditions>();
                }
                if (instance == null)
                {
                    instance = Resources.Load<AllConditions>(loadPath);
                }
                if (instance == null)
                {
                    Debug.LogError("AllConditions has not been created yet. Go to Assets > Create > Wizards Code > Interactions > Conditions. The resulting object must be stored in Resources/AllConditions and you will need to add at least one condition to it in the inspector.");
                }
                return instance;
            }
            set { instance = value; }
        }

        public override void Reset()
        {
            if (conditions == null)
            {
                return;
            }

            for (int i = 0; i < conditions.Length; i++)
            {
                conditions[i].Reset();
            }
        }

        public static bool CheckCondition (Condition required, BaseAgentController interactor, Interactable interactable)
        {
            bool satisfied = true;
            Condition[] allConditions = Instance.conditions;

            if (allConditions != null && allConditions[0] != null)
            {
                for (int i = 0; i < allConditions.Length; i++)
                {
                    if (allConditions[i].Hash == required.Hash)
                    {
                        satisfied = satisfied && allConditions[i].Satisfied(interactor, interactable);
                    }
                }
            }

            return satisfied;
        }
    }
}
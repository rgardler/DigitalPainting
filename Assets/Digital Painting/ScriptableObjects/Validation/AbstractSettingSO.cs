﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using wizardscode.utility;

namespace wizardscode.validation
{
    /// <summary>
    /// Implementations of the AbstractSettingsSO define a desired setting for a 
    /// single value. These are used in the validation system to find the optimal
    /// settings when different plugins demand different settings.
    /// </summary>
    public abstract class AbstractSettingSO : ScriptableObject
    {

        [Tooltip("A human readable name for this setting.")]
        public string SettingName;

        [Tooltip("Is a null value allowable? Set to true if setting can left unconfigured.")]
        public bool Nullable = false;

        [Tooltip("If the suggested value is a prefab should a copy of the object be added to the scene.")]
        public bool AddToScene = false;

        [Tooltip("The name of the class containing the property or field to set. For example, `QualitySettings`.")]
        public string valueClassName;

        [Tooltip("The name of the property or field to set. For example, `shadowDistance`.")]
        public string valueName;

        [SerializeField]
        private ValidationResultCollection validationCollection;

        private ValidationResultCollection ValidationCollection
        {
            get
            {
                if (validationCollection == null)
                {
                    validationCollection = new ValidationResultCollection(); ;
                }
                return validationCollection;
            }
        }

        /// <summary>
        /// A human readable name for the default test.
        /// </summary>
        public virtual string TestName
        {
            get { return "Default Setting Test Suite (replace this name by overriding the TestName getter in you *SettingSO)"; }
        }

        /// <summary>
        /// Test to see if the setting is valid or not. 
        /// </summary>
        /// <returns>A ValidationResult. This will have an impact of "OK" if the setting is set to an acceptable value.</returns>
        public abstract ValidationResult Validate(Type validationTest);

        /// <summary>
        /// Test to see if the setting is valid or not. It's not necessary to test for null values here, 
        /// that is done automatically.
        /// </summary>
        /// <returns>True or false depending on whether the setting is correctly set (true) or not (false)</returns>
        internal abstract ValidationResult ValidateSetting(Type validationTest);

        /// <summary>
        /// This method will be executed when the user clicks a button to automatically fix the setting.
        /// In some cases this will simply set the recommended setting, in other cases this will take the
        /// user to the setting to fix it themselves.
        /// </summary>
        public abstract void Fix();

        private ValidationResult GetResult(string testName, string message, string reportingTest)
        {
            ValidationResult result = ValidationCollection.GetOrCreate(SettingName + " - " + testName, reportingTest);
            result.Message = message;
            result.impact = ValidationResult.Level.Warning;
            AddDefaultFixCallback(result);
            return result;
        }

        internal void AddDefaultFixCallback(ValidationResult result)
        {
            ResolutionCallback callback = AddDefaultCallback();
            result.AddCallback(callback);
        }

        private ResolutionCallback AddDefaultCallback()
        {
            return new ResolutionCallback(Fix, "Automatically Resolve");
        }

        internal ValidationResult GetErrorResult(string testName, string message, string reportingTest)
        {
            ValidationResult result = GetResult(testName, message, reportingTest);
            result.impact = ValidationResult.Level.Error;
            return result;
        }

        internal ValidationResult GetWarningResult(string testName, string message, string reportingTest)
        {
            ValidationResult result = GetResult(testName, message, reportingTest);
            result.impact = ValidationResult.Level.Warning;
            return result;
        }

        internal ValidationResult GetPassResult(string testName, string reportingTest)
        {
            ValidationResult result = GetResult(testName, "Looks good.", reportingTest);
            result.impact = ValidationResult.Level.OK;
            return result;
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using wizardscode.extension;
using wizardscode.plugin;
using wizardscode.utility;

namespace wizardscode.validation
{
    /// <summary>
    /// A test that can be executed in order to validate that the DigitalPainting system and/or its plugins are setup correctly.
    /// Each ValidationTest tests one specific requirement.
    /// </summary>
    public abstract class ValidationTest<T> where T : AbstractPluginManager
    {
        private AbstractPluginManager m_manager;

        private AbstractPluginManager Manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = GameObject.FindObjectOfType<T>(); ;
                }
                return m_manager;
            }
        }

        internal abstract string ProfileType { get; }

        public abstract ValidationTest<T> Instance { get; }

        public ValidationResultCollection Execute(Type validationTest)
        {
            ValidationResultCollection localCollection = new ValidationResultCollection();
            ValidationResult result;
            
            // Is plugin enabled, If not offer an easy way to enable it
            if (Manager == null)
            {
                result = localCollection.GetOrCreate(validationTest.Name);
                result.Message = "FIXME: Plugin type is not enabled (click ignore if you don't want to use it)";
                result.impact = ValidationResult.Level.Warning;
                // FIXME: offer a way to enable the plugin
                result.Callback = null;
                localCollection.AddOrUpdate(result);

                return localCollection;
            }

            // Is a plugin profile provided?
            if(Manager.Profile == null)
            {
                result = localCollection.GetOrCreate(Manager.GetType().Name.BreakCamelCase() + " - Missing Profile");
                result.Message = "You need to provide a plugin profile for " + Manager.GetType().Name.BreakCamelCase();
                result.Test = validationTest;
                result.impact = ValidationResult.Level.Error;
                result.Callback = null;
                localCollection.AddOrUpdate(result);

                return localCollection;
            }

            // FIXME: Check that the profile is of the correct type
            if (Manager.Profile.GetType().Name != ProfileType) {
                return localCollection;
            }

            // Get all the SettingSO fields
            IEnumerable<FieldInfo> fields = Manager.Profile.GetType().GetFields()
                .Where(field => field.FieldType.IsSubclassOf(typeof(AbstractSettingSO)));

            // Validate them individually
            foreach (FieldInfo field in fields)
            {
                object instance = field.GetValue(Manager.Profile);
                if (instance == null)
                {
                    result = localCollection.GetOrCreate(field.Name);
                    result.Message = "Must provide a Setting Scriptable Object";
                    result.impact = ValidationResult.Level.Error;
                }
                else
                {
                    localCollection.Remove(field.Name);

                    Type type = field.FieldType;
                    result = (ValidationResult)type.InvokeMember("Validate",
                        BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                        null, instance, new object[] { validationTest });
                }
                localCollection.AddOrUpdate(result);
            }

            // FIXME: ensure all tests are moved to the new model
            // localCollection.AddOrUpdateAll(ExecuteOriginal());
            return localCollection;
        }
    }
}

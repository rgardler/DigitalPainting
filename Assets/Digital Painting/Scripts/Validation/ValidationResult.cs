﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using wizardscode.plugin;

namespace wizardscode.validation
{
    /// <summary>
    /// A ValidationResult captures the results of a validation test.
    /// These can be used to help the designer improve on their scene.
    /// </summary>
    public class ValidationResult
    {
        public enum Level { OK, Warning, Error, Untested }

        public int id;
        public string name;
        public AbstractPluginManager PluginManager;
        public Level impact;
        public ResolutionCallback Callback;

        private string m_message;

        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// Create a Validation object in an untested state.
        /// </summary>
        /// <param name="name">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        internal ValidationResult(string name) : this(name, Level.Untested, null)
        {
        }

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="name">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        internal ValidationResult(string name, Level impact) : this(name, impact, null)
        {
        }

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="name">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        /// <param name="callbackToFix">A callback method that will allow the result to be corrected if possible.</param>
        internal ValidationResult(string name, Level impact, ProfileCallback callbackToFix)
        {
            this.id = name.GetHashCode();
            this.name = name;
            this.impact = impact;
            this.Callback = new ResolutionCallback(callbackToFix);
        }
    }

    public delegate void ProfileCallback();

    /// <summary>
    /// ResolutionCallback is a method that can be called to resolve a problem.
    /// </summary>
    public class ResolutionCallback
    {
        public ProfileCallback ProfileCallback; // A callback that attempts to resolve the problem automatically
        public string Label;

        /// <summary>
        /// Create a ResolutionCallback in which the label is automatically generated from the method name.
        /// CamelCase names will be broken into their individual words for the label.
        /// </summary>
        /// <param name="callback"></param>
        public ResolutionCallback(ProfileCallback callback)
        {
            ProfileCallback = callback;
            if (callback != null)
            {
                Label = Regex.Replace(ProfileCallback.Method.Name, "(\\B[A-Z])", " $1");
            }
            else
            {
                Label = "No Fix Available";
            }
        }

        /// <summary>
        /// Create a ResolutionCallback in which the label is defined.
        /// </summary>
        /// <param name="callback"></param>
        public ResolutionCallback(ProfileCallback callback, string label)
        {
            ProfileCallback = callback;
            Label = label;
        }
    }
}
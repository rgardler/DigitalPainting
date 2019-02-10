using System;
using System.Runtime.Serialization;

namespace wizardscode.interaction
{
    [Serializable]
    public class MisconfiguredBehaviourException : ApplicationException
    {
        #region privates
        #endregion privates

        #region properties
        #endregion properties

        public MisconfiguredBehaviourException(string sMessage,
            Exception innerException)
            : base(sMessage, innerException) { }
        public MisconfiguredBehaviourException(string sMessage)
            : base(sMessage) { }
        public MisconfiguredBehaviourException() { }

        #region Serializeable Code
        public MisconfiguredBehaviourException(
           SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        #endregion Serializeable Code
    }
}
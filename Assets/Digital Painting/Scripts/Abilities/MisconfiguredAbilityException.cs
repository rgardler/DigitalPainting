using System;
using System.Runtime.Serialization;

namespace wizardscode.ability
{
    [Serializable]
    public class MisconfiguredAbilityException : ApplicationException
    {
        #region privates
        #endregion privates

        #region properties
        #endregion properties

        public MisconfiguredAbilityException(string sMessage,
            Exception innerException)
            : base(sMessage, innerException) { }
        public MisconfiguredAbilityException(string sMessage)
            : base(sMessage) { }
        public MisconfiguredAbilityException() { }

        #region Serializeable Code
        public MisconfiguredAbilityException(
           SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        #endregion Serializeable Code
    }
}
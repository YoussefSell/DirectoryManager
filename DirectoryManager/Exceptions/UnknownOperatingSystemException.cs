namespace System.IO.Expand
{
    using System;

    /// <summary>
    /// Exception thrown if the operation system is not defined 
    /// </summary>
    [Serializable]
    [Diagnostics.DebuggerStepThrough]
    public class UnknownOperatingSystemException : Exception
    {
        /// <summary>
        /// parameterless constructor
        /// </summary>
        public UnknownOperatingSystemException()
            : base() { }

        /// <summary>
        /// constructor with the exception message
        /// </summary>
        /// <param name="msg"></param>
        public UnknownOperatingSystemException(string msg)
            : base(msg) { }

        /// <summary>
        /// constructor with SerializationInfo and StreamingContext
        /// </summary>
        /// <param name="info">the SerializationInfo</param>
        /// <param name="context">the StreamingContext</param>
        protected UnknownOperatingSystemException(
            Runtime.Serialization.SerializationInfo info,
            Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

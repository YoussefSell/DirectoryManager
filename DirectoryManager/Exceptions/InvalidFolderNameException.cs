namespace System.IO.Expand
{
    using System;

    /// <summary>
    /// Exception thrown when the given folder name is invalid
    /// </summary>
    [Serializable]
    [Diagnostics.DebuggerStepThrough]
    public class InvalidFolderNameException : Exception
    {
        /// <summary>
        /// parameterless constructor
        /// </summary>
        public InvalidFolderNameException()
            : base() { }

        /// <summary>
        /// constructor with the exception message
        /// </summary>
        public InvalidFolderNameException(string message)
            : base(message) { }

        /// <summary>
        /// constructor with SerializationInfo and StreamingContext
        /// </summary>
        /// <param name="info">the SerializationInfo</param>
        /// <param name="context">the StreamingContext</param>
        protected InvalidFolderNameException(
          Runtime.Serialization.SerializationInfo info,
          Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

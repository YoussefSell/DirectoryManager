namespace System.IO.Expand
{
    /// <summary>
    /// class for the DirectoryContentChanged Event Arguments
    /// </summary>
    [Diagnostics.DebuggerStepThrough]
    public class DirectoryContentChangedEventArgs : EventArgs
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="fullPath">the full path to the affected file or directory</param>
        /// <param name="name">the name of the affected file or directory</param>
        /// <param name="OldfullPath">the Old full path to the affected file or directory, only populated in case of Rename change type, otherwise is set to null</param>
        /// <param name="Oldname">the Old name of the affected file or directory, only populated in case of Rename change type, otherwise is set to null</param>
        /// <param name="changeType">the type of the changed</param>
        public DirectoryContentChangedEventArgs(ChangeType changeType, string fullPath, string name, string OldfullPath = null, string Oldname= null)
        {
            ChangeType = changeType;
            Name = name;
            FullPath = fullPath;
        }

        /// <summary>
        /// Gets the type of directory event that occurred.
        /// </summary>
        public ChangeType ChangeType { get; }
        
        /// <summary>
        /// Gets the fully qualified path of the affected file or directory.
        /// </summary>
        public string FullPath { get; }

        /// <summary>
        /// Gets the name of the affected file or directory.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Old path of the affected file or directory.
        /// only populated in case of Rename change type, otherwise is set to null.
        /// </summary>
        public string OldFullPath { get; }

        /// <summary>
        /// Old name of the affected file or directory.
        /// only populated in case of Rename change type, otherwise is set to null.
        /// </summary>
        public string OldName { get; }
    }
}

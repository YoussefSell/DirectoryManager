namespace System.IO.Expand
{
    /// <summary>
    /// OS type
    /// </summary>
    enum OS
    {
        Windows,
        MacOS,
        Linux,
        Unknown
    }

    /// <summary>
    /// Directory ChangeType
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// a new file or folder has been created inside the monitored directory
        /// </summary>
        Created = 1,

        /// <summary>
        /// a new file or folder has been Deleted inside the monitored directory
        /// </summary>
        Deleted = 2,

        /// <summary>
        /// a new file or folder has been Changed inside the monitored directory
        /// </summary>
        Changed = 4,

        /// <summary>
        /// a new file or folder has been Renamed inside the monitored directory
        /// </summary>
        Renamed = 8,

        /// <summary>
        /// all changes has been made
        /// </summary>
        All = 15
    }

    /// <summary>
    /// Comparison Options
    /// </summary>
    public enum CompareOptions
    {
        /// <summary>
        /// the default compare option
        /// </summary>
        Default,

        /// <summary>
        /// compare with name
        /// </summary>
        Name,

        /// <summary>
        /// compare with fullName
        /// </summary>
        FullName,

        /// <summary>
        /// compare with DateOfCreation
        /// </summary>
        DateOfCreation,

        /// <summary>
        /// compare with Size
        /// </summary>
        Size
    }

    /// <summary>
    /// output options
    /// </summary>
    public enum OutputOptions
    {
        /// <summary>
        /// the default output option
        /// </summary>
        Default,

        /// <summary>
        /// get the matched Result of the comparison
        /// </summary>
        Matching,

        /// <summary>
        /// get the non-matching Result of the comparison
        /// </summary>
        NonMatching
    }

    /// <summary>
    /// Rename Options
    /// </summary>
    public enum RenameOptions
    {
        /// <summary>
        /// the default Rename option
        /// </summary>
        Default = 0,

        /// <summary>
        /// Rename by adding Incremental Numbers To Beginning of the string
        /// </summary>
        AddIncrementalNumbersToBeginning,

        /// <summary>
        /// Rename by adding Incremental Numbers To the end of the string
        /// </summary>
        AddIncrementalNumbersToEnd,

        /// <summary>
        /// Rename by Replacing Matched Regex Pattern,
        /// </summary>
        ReplaceMatchedRegexPattern,

        /// <summary>
        /// Rename by removing matched regex pattern
        /// </summary>
        RemoveMatchedRegexPattern,

        /// <summary>
        /// Rename by adding Random letters to the end of the string
        /// </summary>
        AddRandomLettersToEnd,

        /// <summary>
        /// Rename by adding Generating random names
        /// </summary>
        GenerateRandomName,

        /// <summary>
        /// Rename by using a unique name
        /// </summary>
        UseUniqueName,
    }

    /// <summary>
    /// search Options
    /// </summary>
    public enum SearchOptions
    {
        /// <summary>
        /// the default search option
        /// </summary>
        Default,

        /// <summary>
        /// search with name option
        /// </summary>
        Name,

        /// <summary>
        /// search with Regex option
        /// </summary>
        Regex
    }
}

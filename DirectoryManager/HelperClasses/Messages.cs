namespace System.IO.Expand
{
    [Diagnostics.DebuggerStepThrough]
    static class Messages
    {
        public static string DirectoryNull { get; } = "provided directoryInfo is null";
        public static string DirectoriesNull { get; } = "provided directories are null";
        public static string DirectoriesNotExist { get; } = "the specified folder is not exist, provide a valid path";
        public static string DirectoriesMustHaveOneDir { get; } = "the list of directories must contain only One Directory";

        public static string InvalidSearchKeyNull { get; } = "the given Search Key is null";
        public static string InvalidName { get; } = "the given name is null or empty";
        public static string InvalidDestPath { get; } = "the given destination path is invalid, is null or empty";
        public static string InvalidSeparator { get; } = "Invalid separator, separator cannot contain any of the following characters: \\ / : * ? \" < > | or any Character";
        public static string InvalidFolderName { get; } = "A folder name cannot contain any of the following characters: \\ / : * ? \" < > | ";
        public static string InvalidRegexPattern { get; } = "the given regex pattern is null or empty";

        public static string CannotOpenFolderViewer { get; } = "Unknown Operating System can't open in Folder Viewer";
        public static string CannotReturnSpecifiedPath { get; } = "Unknown Operating System can't return the specified path";
    }
}

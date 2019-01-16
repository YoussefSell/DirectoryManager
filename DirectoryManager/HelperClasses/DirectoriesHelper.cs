namespace System.IO.Expand
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    [Diagnostics.DebuggerStepThrough]
    internal class DirectoriesHelper
    {
        #region Compare Methods

        /// <summary>
        /// compare the two folders with their names
        /// </summary>
        /// <param name="currentDirectory">the current directory instant</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put option</param>
        /// <returns>list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static IEnumerable<DirectoryInfo> CompareWithName(DirectoryInfo currentDirectory,
            DirectoryInfo directoryToCompaire, OutputOptions outputOptions)
        {
            //compare base on the output option
            switch (outputOptions)
            {
                case OutputOptions.NonMatching:
                    return CompareForNonMatching<DirectoryInfoCompareWithName>(currentDirectory, directoryToCompaire);
                case OutputOptions.Matching:
                default:
                    return CompareForMatching<DirectoryInfoCompareWithName>(currentDirectory, directoryToCompaire);
            }
        }

        /// <summary>
        /// compare the two folders with their full names
        /// </summary>
        /// <param name="currentDirectory">the current directory instant</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put option</param>
        /// <returns>list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static IEnumerable<DirectoryInfo> CompareWithFullName(DirectoryInfo currentDirectory,
            DirectoryInfo directoryToCompaire, OutputOptions outputOptions)
        {
            //compare base on the output option
            switch (outputOptions)
            {
                case OutputOptions.NonMatching:
                    return CompareForNonMatching<DirectoryInfoCompareWithFullName>(currentDirectory, directoryToCompaire);
                case OutputOptions.Matching:
                default:
                    return CompareForMatching<DirectoryInfoCompareWithFullName>(currentDirectory, directoryToCompaire);
            }
        }

        /// <summary>
        /// compare the two folders with their Date of creation
        /// </summary>
        /// <param name="currentDirectory">the current directory instant</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put option</param>
        /// <returns>list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static IEnumerable<DirectoryInfo> ComparewithDateOfCreation(DirectoryInfo currentDirectory,
            DirectoryInfo directoryToCompaire, OutputOptions outputOptions)
        {
            //compare base on the output option
            switch (outputOptions)
            {
                case OutputOptions.NonMatching:
                    return CompareForNonMatching<DirectoryInfoCompareWithDateOfCreation>(currentDirectory, directoryToCompaire);
                case OutputOptions.Matching:
                default:
                    return CompareForMatching<DirectoryInfoCompareWithDateOfCreation>(currentDirectory, directoryToCompaire);
            }
        }

        /// <summary>
        /// compare the two folders with their Total Size
        /// </summary>
        /// <param name="currentDirectory">the current directory instant</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put option</param>
        /// <returns>list of results found</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        internal static IEnumerable<DirectoryInfo> ComparewithTotalSize(DirectoryInfo currentDirectory,
            DirectoryInfo directoryToCompaire, OutputOptions outputOptions)
        {
            //compare base on the output option
            switch (outputOptions)
            {
                case OutputOptions.NonMatching:
                    return CompareForNonMatching<DirectoryInfoCompareWithSize>(currentDirectory, directoryToCompaire);
                case OutputOptions.Matching:
                default:
                    return CompareForMatching<DirectoryInfoCompareWithSize>(currentDirectory, directoryToCompaire);
            }
        }

        /// <summary>
        /// compare the two folders with the specified Compare option and return the Non Matching results option
        /// </summary>
        /// <param name="currentDirectory">the current directory instant</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <returns>list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static IEnumerable<DirectoryInfo> CompareForNonMatching<T>
            (DirectoryInfo currentDirectory, DirectoryInfo directoryToCompaire)
            where T : class, IEqualityComparer<DirectoryInfo>, new()
        {
            return currentDirectory.EnumerateDirectories()
                        .Except(directoryToCompaire.EnumerateDirectories(), new T());
        }

        /// <summary>
        /// compare the two folders with the specified Compare option and return the Matching results option
        /// </summary>
        /// <param name="currentDirectory">the current directory instant</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <returns>list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static IEnumerable<DirectoryInfo> CompareForMatching<T>
            (DirectoryInfo currentDirectory, DirectoryInfo directoryToCompaire)
            where T : class, IEqualityComparer<DirectoryInfo>, new()
        {
            return currentDirectory.EnumerateDirectories()
                        .Intersect(directoryToCompaire.EnumerateDirectories(), new T());
        }

        #endregion

        #region Search

        /// <summary>
        /// search in the current directory instant using the name
        /// </summary>
        /// <param name="CurrentDirectory">the current directory instant</param>
        /// <param name="dirName">the name to look for</param>
        /// <returns>the list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static IEnumerable<DirectoryInfo> SearchWithName(DirectoryInfo CurrentDirectory, string dirName)
        {
            return CurrentDirectory.EnumerateDirectories()
                .Where(dir => dir.Name.ToLower().Contains((dirName ?? "").ToLower()));
        }

        /// <summary>
        /// search in current directory instant Using <see cref="Regex"/>
        /// </summary>
        /// <param name="CurrentDirectory">the current directory instant</param>
        /// <param name="searchPattern">the Regex pattern</param>
        /// <returns>list of results found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        internal static IEnumerable<DirectoryInfo> SearchWithRegex(DirectoryInfo CurrentDirectory, string searchPattern)
        {
            return CurrentDirectory.EnumerateDirectories()
                .Where(item => Tools.IsMatch(item.Name, searchPattern ?? "", RegexOptions.IgnoreCase));
        }

        #endregion

        #region Rename Methods

        /// <summary>
        /// Method used to rename one directory by Using a Unique Name, passing a collection of directories
        /// </summary>
        /// <param name="directories">list of directories must contains only one directory</param>
        /// <param name="newName">the new name of the directory</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static void RenameByUsingUniqueName(IEnumerable<DirectoryInfo> directories, string newName)
        {
            //we store the count value so that we don't call to it twice
            var dirCount = directories?.Count();

            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            if (dirCount <= 0 || dirCount > 1)
                throw new ArgumentException(Messages.DirectoriesMustHaveOneDir);

            directories.First().Rename(newName);
        }

        /// <summary>
        /// Method used to rename a set of directories by Adding Incremental Numbers To Beginning separated with a separator
        /// </summary>
        /// <param name="directories">list of directories</param>
        /// <param name="separator">the separator</param>
        /// <param name="StartFrom">start adding from</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static void RenameByAddingIncrementalNumbersToBeginning(
            IEnumerable<DirectoryInfo> directories, char separator, int StartFrom)
        {
            if (!Tools.IsValidSeparator(separator))
                throw new ArgumentException(Messages.InvalidSeparator);

            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            string sep = separator == '\0' ? "" : separator.ToString();

            foreach (var dir in directories)
            {
                dir.Rename($"{StartFrom++.ToString()}{sep}{dir.Name}");
            }
        }

        /// <summary>
        /// Method used to rename a set of directories by Adding Incremental Numbers To End separated with a separator
        /// </summary>
        /// <param name="directories">list of directories</param>
        /// <param name="separator">the separator</param>
        /// <param name="StartFrom">start adding from</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static void RenameByAddingIncrementalNumbersToEnd(
            IEnumerable<DirectoryInfo> directories, char separator, int StartFrom)
        {
            if (!Tools.IsValidSeparator(separator))
                throw new ArgumentException(Messages.InvalidSeparator);

            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            string sep = separator == '\0' ? "" : separator.ToString();

            foreach (var dir in directories)
            {
                dir.Rename($"{dir.Name}{sep}{StartFrom++.ToString()}");
            }
        }

        /// <summary>
        /// Method used to rename a set of directories by Adding Random Letters To End separated with a separator
        /// </summary>
        /// <param name="directories">list of directories</param>
        /// <param name="separator">the separator</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException">if there is a Security problem when accessing the folder</exception>
        internal static void RenameByAddingRandomLettersToEnd(
            IEnumerable<DirectoryInfo> directories, char separator)
        {
            if (!Tools.IsValidSeparator(separator))
                throw new ArgumentException(Messages.InvalidSeparator);

            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            string sep = separator == '\0' ? "" : separator.ToString();

            foreach (var dir in directories)
            {
                dir.Rename($"{dir.Name}{sep}{RandomGenerator.RandomString()}");
            }
        }

        /// <summary>
        /// Method used to rename a set of directories by Generating Random Names
        /// </summary>
        /// <param name="directories">list of directories</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException">if there is a Security problem when accessing the folder</exception>
        internal static void RenameByGeneratingRandomNames(
            IEnumerable<DirectoryInfo> directories)
        {
            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            foreach (var dir in directories)
            {
                dir.GenerateRandomName();
            }
        }

        /// <summary>
        /// Method used to rename a set of directories by Removing Matched Regex Pattern
        /// </summary>
        /// <param name="directories">list of directories</param>
        /// <param name="pattern">the Regex pattern</param>
        /// <param name="regexOptions">the RegexOptions</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static void RenameByRemovingMatchedRegexPattern(
            IEnumerable<DirectoryInfo> directories, string pattern, RegexOptions regexOptions)
        {
            if (!pattern.IsValid())
                throw new ArgumentException(Messages.InvalidRegexPattern);

            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            foreach (var dir in directories)
            {
                var newName = dir.Name.ReplaceWithEmptyString(pattern, regexOptions);

                if (newName != dir.Name)
                    dir.Rename(newName);
            }
        }

        /// <summary>
        /// Method used to rename a set of directories by replace Matched Regex Pattern
        /// </summary>
        /// <param name="directories">list of directories</param>
        /// <param name="pattern">the Regex pattern</param>
        /// <param name="replaceWith">the string to replace the pattern with</param>
        /// <param name="regexOptions">the RegexOptions</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidFolderNameException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="Security.SecurityException"></exception>
        internal static void RenameByReplacingMatchedRegexPattern(IEnumerable<DirectoryInfo> directories,
            string pattern, string replaceWith, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (!pattern.IsValid())
                throw new ArgumentException(Messages.InvalidRegexPattern);

            if (directories is null)
                throw new ArgumentException(Messages.DirectoriesNull);

            foreach (var dir in directories)
            {
                var newName = dir.Name.ReplaceWithValue(replaceWith, pattern, regexOptions);

                if (newName != dir.Name)
                    dir.Rename(newName);
            }
        }

        #endregion

        #region General Methods

        /// <summary>
        /// return the full path to the desktop folder
        /// </summary>
        /// <returns>desktop path</returns>
        /// <exception cref="UnknownOperatingSystemException">if unable to detect the current operation system</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
        internal static string GetDesktopPath()
        {
            switch (Tools.GetOperationSystem())
            {
                case OS.Windows:
                    return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                case OS.MacOS:
                    return Environment.GetEnvironmentVariable("$HOME");
                case OS.Linux:
                    return Environment.GetEnvironmentVariable("$HOME");
                case OS.Unknown:
                default:
                    throw new UnknownOperatingSystemException(Messages.CannotReturnSpecifiedPath);
            }
        }

        #endregion
    }
}

namespace System.IO.Expand
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// class for adding more functionality to <see cref="DirectoryInfo"/> objects
    /// </summary>
    [DebuggerStepThrough]
    public static class DirectoryInfoHelper
    {
        #region Compare Methods

        /// <summary>
        /// compare the content of the current directory to the given directory content
        /// using the given compare Option
        /// Note that only Directory are compared the files are ignored
        /// </summary>
        /// <param name="CurrentDirectory">the current instant directory</param>
        /// <param name="directoryToCompare">the directory to compare to</param>
        /// <param name="outputOptions">the out put type</param>
        /// <param name="compareOptions">the compare option, has a default value set to CompareOptions.Name</param>
        /// <returns>the list of found directories</returns>
        /// <exception cref="DirectoryNotFoundException">Current Directory -or- Directory to compare cannot be found.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static IEnumerable<DirectoryInfo> Compare(
            this DirectoryInfo CurrentDirectory,
            DirectoryInfo directoryToCompare,
            OutputOptions outputOptions, 
            CompareOptions compareOptions = CompareOptions.Name)
        {
            if (!CurrentDirectory.Exists || !directoryToCompare.Exists)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            switch (compareOptions)
            {
                case CompareOptions.FullName:
                    return DirectoriesHelper.CompareWithFullName(CurrentDirectory, directoryToCompare, outputOptions);
                case CompareOptions.DateOfCreation:
                    return DirectoriesHelper.ComparewithDateOfCreation(CurrentDirectory, directoryToCompare, outputOptions);
                case CompareOptions.Size:
                    return DirectoriesHelper.ComparewithTotalSize(CurrentDirectory, directoryToCompare, outputOptions);
                case CompareOptions.Name:
                default: return DirectoriesHelper.CompareWithName(CurrentDirectory, directoryToCompare, outputOptions);
            }
        }

        /// <summary>
        /// compare the content of the current directory to the given directory content
        /// using the given compare Options
        /// Note that only Directory are compared the files are ignored
        /// </summary>
        /// <param name="CurrentDirectory">the current instant directory</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put type</param>
        /// <param name="compareOptions">the compare options</param>
        /// <returns>the list of found directories</returns>
        /// <exception cref="DirectoryNotFoundException">Current Directory -or- Directory to compare cannot be found.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static IEnumerable<DirectoryInfo> Compare(
            this DirectoryInfo CurrentDirectory, 
            DirectoryInfo directoryToCompaire, 
            OutputOptions outputOptions, 
            CompareOptions[] compareOptions)
        {
            throw new NotImplementedException("this function is not implemented yet, if you have any idea on how we should implement it, feel free to contact us");
        }

        /// <summary>
        /// compare the content of the current directory to the given directory content
        /// using the given compare Option
        /// Note that only Directory are compared the files are ignored
        /// </summary>
        /// <param name="CurrentDirectory">the current instant directory</param>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put type</param>
        /// <param name="compareOptions">the compare option, has a default value set to CompareOptions.Name</param>
        /// <returns>the list of found directories</returns>
        /// <exception cref="DirectoryNotFoundException">Current Directory -or- Directory to compare cannot be found.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public async static Task<IEnumerable<DirectoryInfo>> CompareAsync(
            this DirectoryInfo CurrentDirectory,
            DirectoryInfo directoryToCompaire,
            OutputOptions outputOptions,
            CompareOptions compareOptions = CompareOptions.Name)
        {
            return await Task.Run(() => CurrentDirectory.Compare(directoryToCompaire, outputOptions, compareOptions));
        }

        #endregion

        #region Search Methods

        /// <summary>
        /// search in the sub directories of the current instant
        /// </summary>
        /// <param name="CurrentDirectory">the current Directory to search in</param>
        /// <param name="searchKey">the search keyword</param>
        /// <param name="searchOptions">search option</param>
        /// <returns>list of results found</returns>
        /// <exception cref="DirectoryNotFoundException">the current directory cannot be found</exception>
        /// <exception cref="RegexMatchTimeoutException">A time-out occurred.</exception>
        /// <exception cref="ArgumentException">if the search key is null</exception>
        public static IEnumerable<DirectoryInfo> Search(
            this DirectoryInfo CurrentDirectory,
            string searchKey,
            SearchOptions searchOptions)
        {
            if (!CurrentDirectory.Exists)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            //search base on the search option
            switch (searchOptions)
            {
                case SearchOptions.Regex:
                    return DirectoriesHelper.SearchWithRegex(CurrentDirectory, searchKey);
                case SearchOptions.Name:
                default: return DirectoriesHelper.SearchWithName(CurrentDirectory, searchKey);
            }
        }

        /// <summary>
        /// search in the sub directories of the current instant
        /// </summary>
        /// <param name="CurrentDirectory">the current Directory to search in</param>
        /// <param name="searchKey">the search keyword</param>
        /// <param name="searchOptions">search option</param>
        /// <exception cref="DirectoryNotFoundException">the current directory cannot be found</exception>
        /// <exception cref="RegexMatchTimeoutException">A time-out occurred.</exception>
        /// <exception cref="ArgumentException">if the search key is null</exception>
        public async static Task<IEnumerable<DirectoryInfo>> SearchAsync(
            this DirectoryInfo CurrentDirectory,
            string searchKey,
            SearchOptions searchOptions)
        {
            return await Task.Run(() => CurrentDirectory.Search(searchKey, searchOptions));
        }

        #endregion

        #region Move Methods

        /// <summary>
        /// move a directory and it content to the new path leaving the directory with the same name
        /// </summary>
        /// <param name="CurrentDirectory">Directory to Move</param>
        /// <param name="distDir">the destination folder</param>
        /// <exception cref="DirectoryNotFoundException">id The Current or destination directory cannot be found.</exception>
        /// <exception cref="ArgumentNullException">distDir is null.</exception>
        /// <exception cref="ArgumentException">distDir is an empty string (''").</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void Move(this DirectoryInfo CurrentDirectory, string distDir)
        {
            if (!CurrentDirectory.Exists)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            var distPath = Path.Combine(distDir, CurrentDirectory.Name);
            CurrentDirectory.MoveTo(distPath);
        }

        /// <summary>
        /// move the folder to the desktop folder
        /// </summary>
        /// <param name="CurrentDirectory">directory to move</param>
        /// <exception cref="DirectoryNotFoundException">The Current directory cannot be found.</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void MoveToDesktop(this DirectoryInfo CurrentDirectory)
        {
            CurrentDirectory.Move(DirectoriesHelper.GetDesktopPath());
        }

        #endregion

        #region Copy Methods

        /// <summary>
        /// copy the current directory to a given location
        /// </summary>
        /// <param name="CurrentDirectory">the current directory instant</param>
        /// <param name="destDirName">the destination where the directory should be copied</param>
        /// <param name="copySubDirs">true to copy sub directory</param>
        /// <param name="overwriteFiles">true to overWrite existing files, by default set to false</param>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException"> The caller does not have the required permission.</exception>
        /// <exception cref="NotSupportedException">destDirName contains a colon (:) in the middle of the string.</exception>
        /// <exception cref="ArgumentNullException">if one of the parameters is null</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception>
        /// <exception cref="ArgumentException">if the destDirName is empty</exception>
        /// <exception cref="IOException">An error occurs, or the destination file already exists and overwrite is false.</exception>
        public static void Copy(this DirectoryInfo CurrentDirectory,
            string destDirName,
            bool copySubDirs = true,
            bool overwriteFiles = false)
        {
            if (!destDirName.IsValid())
                throw new ArgumentException(Messages.InvalidDestPath);

            destDirName = Path.Combine(destDirName, CurrentDirectory.Name);

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = CurrentDirectory.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                DirectoryInfo[] dirs = CurrentDirectory.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    Copy(subdir, destDirName, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Copy the directory to the desktop folder
        /// </summary>
        /// <param name="CurrentDirectory">the current directory instant</param>
        /// <param name="copySubDirs">true to copy sub directory</param>
        /// <param name="overwriteFiles">true to overWrite existing files, by default set to false</param>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException"> The caller does not have the required permission.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception>
        /// <exception cref="IOException">An error occurs, or the destination file already exists and overwrite is false.</exception>
        public static void CopyToDesktop(this DirectoryInfo CurrentDirectory,
            bool copySubDirs = true,
            bool overwriteFiles = false)
        {
            CurrentDirectory.Copy(DirectoriesHelper.GetDesktopPath());
        }

        #endregion

        #region Rename Methods

        /// <summary>
        /// rename the Current Directory to the new given name
        /// </summary>
        /// <param name="CurrentDirectory">the current directory instant</param>
        /// <param name="newName">the new name</param>
        /// <exception cref="InvalidFolderNameException">if the name contains invalid characters</exception>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="ArgumentException">if the newName is null, empty or whitespace</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void Rename(this DirectoryInfo CurrentDirectory, string newName)
        {
            if (!CurrentDirectory.Exists)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            if (!newName.IsValid())
                throw new ArgumentException(Messages.InvalidName);

            if (!Tools.IsValidFolderName(newName))
                throw new InvalidFolderNameException(Messages.InvalidFolderName);

            var theNewName = Path.Combine(CurrentDirectory.Parent.FullName, newName);
            CurrentDirectory.MoveTo(theNewName);
        }

        /// <summary>
        /// generate a random folder name
        /// </summary>
        /// <param name="directory">the directory to generate the name for</param>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void GenerateRandomName(this DirectoryInfo directory)
        {
            directory.Rename(RandomGenerator.RandomFolderName());
        }

        #endregion

        #region General Methods

        /// <summary>
        /// get the total size of the Current Directory
        /// </summary>
        /// <param name="CurrentDirectory">the current directory</param>
        /// <returns>the size in bytes</returns>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public static long GetTotalSize(this DirectoryInfo CurrentDirectory)
        {
            if (!CurrentDirectory.Exists)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            // get the size of all files in the current directory
            var currentSize = CurrentDirectory.GetFiles().Sum(f => f.Length);

            // get the size of all files in all subdirectories
            var subDirSize = CurrentDirectory.GetDirectories().Sum(d => d.GetTotalSize());

            // return the sum of the two results
            return currentSize + subDirSize;
        }

        /// <summary>
        /// get the total size of the Current Directory
        /// </summary>
        /// <param name="CurrentDirectory">the current directory</param>
        /// <returns>the size in bytes</returns>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public async static Task<long> GetTotalSizeAsync(this DirectoryInfo CurrentDirectory)
        {
            return await Task.Run(() => CurrentDirectory.GetTotalSize());
        }

        /// <summary>
        /// Launch the folder viewer for the Current folder
        /// </summary>
        /// <param name="currentDirectory">The Current Folder instant</param>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="UnknownOperatingSystemException">if unable to detect the current operation system</exception>
        public static void LaunchFolderView(this DirectoryInfo currentDirectory)
        {
            if (!currentDirectory.Exists)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            using (Process Process = new Process())
            {
                //get the operation system to run the specified command for it
                var os = Tools.GetOperationSystem();
                switch (os)
                {
                    case OS.Windows:
                        Process.StartInfo = new ProcessStartInfo
                        {
                            FileName = "explorer",
                            Arguments = $"/root,{currentDirectory.FullName}",
                            UseShellExecute = true
                        };
                        break;
                    case OS.MacOS:
                        Process.StartInfo = new ProcessStartInfo
                        {
                            FileName = "open",
                            Arguments = $"-R {currentDirectory.FullName}",
                            UseShellExecute = true
                        };
                        break;
                    case OS.Linux:
                        Process.StartInfo = new ProcessStartInfo()
                        {
                            FileName = "file://",
                            Arguments = currentDirectory.FullName,
                        };
                        break;
                    case OS.Unknown:
                        throw new UnknownOperatingSystemException(Messages.CannotOpenFolderViewer);
                }

                Process.Start();
            }
        }

        #endregion
    }
}

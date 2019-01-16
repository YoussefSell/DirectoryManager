namespace System.IO.Expand
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public partial class DirectoryManager
    {
        #region Private Fields

        private bool _enableContentChangedWatcher = false;
        private FileSystemWatcher _watcher;

        #endregion

        #region Private Methods

        private void CreateFileWatcher()
        {
            _enableContentChangedWatcher = true;
            _watcher = _watcher ?? new FileSystemWatcher();

            _watcher.Path = DirectoryInfoInstant.FullName;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
            _watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.LastAccess |
                                    NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;

            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnChanged);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            DirectoryContentChanged?.Invoke(this, new DirectoryContentChangedEventArgs(
                (ChangeType)e.ChangeType,
                e.FullPath,
                e.Name));
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            DirectoryContentChanged?.Invoke(this, new DirectoryContentChangedEventArgs(
                (ChangeType)e.ChangeType,
                e.FullPath,
                e.Name,
                e.OldFullPath,
                e.OldName));
        } 

        private int GetTotalSubDirs()
        {
            return DirectoryInfoInstant.GetDirectories().Length;
        }

        private int GetTotalSubFiles()
        {
            return DirectoryInfoInstant.GetFiles().Length;
        }

        #endregion

        #region DirectoryInfo Methods Implementations

        /// <summary>
        /// Creates a subdirectory or subdirectories on the specified path. The specified
        /// path can be relative to this instance of the System.IO.DirectoryInfo class.
        /// </summary>
        /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming
        /// Convention (UNC) name.</param>
        /// <returns>The last directory specified in path.</returns>
        /// <exception cref="ArgumentException">path does not specify a valid file path or contains invalid DirectoryInfo characters.</exception>
        /// <exception cref="ArgumentNullException">path is null.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"> The subdirectory cannot be created. -or- A file or directory already has the
        /// name specified by path.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters,
        /// and file names must be less than 260 characters. The specified path, file name,
        /// or both are too long.</exception>
        /// <exception cref="NotSupportedException">path contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        public DirectoryManager CreateSubdirectory(string path)
        {
            return DirectoryInfoInstant
                .CreateSubdirectory(path);
        }

        /// <summary>
        /// Deletes this System.IO.DirectoryInfo if it is empty.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">The directory contains a read-only file.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory described by this System.IO.DirectoryInfo object does not exist
        /// or could not be found.</exception>
        /// <exception cref="IOException">The directory is not empty. 
        /// -or- The directory is the application's current working 
        /// -or- There is an open handle on the directory, and the operating system
        /// is Windows XP or earlier. This open handle can result from enumerating directories.
        /// For more information, see ~/docs/standard/IO/how-to-enumerate-directories-and-files.md.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public void Delete()
        {
            DirectoryInfoInstant.Delete();
        }

        /// <summary>
        /// Deletes this instance of a System.IO.DirectoryInfo, specifying whether to delete
        /// subdirectories and files.
        /// </summary>
        /// <param name="recursive">true to delete this directory, its subdirectories, and all files; otherwise, false.</param>
        /// <exception cref="UnauthorizedAccessException">The directory contains a read-only file.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory described by this System.IO.DirectoryInfo object does not exist
        /// or could not be found.</exception>
        /// <exception cref="IOException">The directory is not empty. 
        /// -or- The directory is the application's current working 
        /// -or- There is an open handle on the directory, and the operating system
        /// is Windows XP or earlier. This open handle can result from enumerating directories.
        /// For more information, see ~/docs/standard/IO/how-to-enumerate-directories-and-files.md.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public void Delete(bool recursive)
        {
            DirectoryInfoInstant.Delete(recursive);
        }

        /// <summary>
        /// Returns an enumerable collection of directory information in the current directory.
        /// </summary>
        /// <returns>An enumerable collection of directories in the current directory.</returns>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<DirectoryManager> EnumerateDirectories()
        {
            return DirectoryInfoInstant
                .EnumerateDirectories()
                .ToDirectoryManager();
        }

        /// <summary>
        /// Returns an enumerable collection of directory information that matches a specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories. This parameter can
        /// contain a combination of valid literal path and wildCard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which
        /// returns all files.</param>
        /// <returns>An enumerable collection of directories that matches searchPattern.</returns>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<DirectoryManager> EnumerateDirectories(string searchPattern)
        {
            return DirectoryInfoInstant
                .EnumerateDirectories(searchPattern)
                .ToDirectoryManager();
        }

        /// <summary>
        /// Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories. This parameter can
        /// contain a combination of valid literal path and wildCard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which
        /// returns all files.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should
        /// include only the current directory or all subdirectories. The default value is
        /// System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of directories that matches searchPattern and searchOption.</returns>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">searchOption is not a valid System.IO.SearchOption value.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<DirectoryManager> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfoInstant
                .EnumerateDirectories(searchPattern, searchOption)
                .ToDirectoryManager();
        }

        /// <summary>
        /// Returns an enumerable collection of file information in the current directory.
        /// </summary>
        /// <returns>An enumerable collection of the files in the current directory.</returns>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<FileInfo> EnumerateFiles()
        {
            return DirectoryInfoInstant.EnumerateFiles();
        }

        /// <summary>
        /// Returns an enumerable collection of file information that matches a search pattern.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain
        /// a combination of valid literal path and wildCard (* and ?) characters, but it
        /// doesn't support regular expressions. The default pattern is "*", which returns
        /// all files.</param>
        /// <returns>An enumerable collection of files that matches searchPattern.</returns>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="Security.SecurityException"> The caller does not have the required permission.</exception>
        public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
        {
            return DirectoryInfoInstant.EnumerateFiles(searchPattern);
        }

        /// <summary>
        ///  Returns an enumerable collection of file information that matches a specified
        ///  search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain
        /// a combination of valid literal path and wildCard (* and ?) characters, but it
        /// doesn't support regular expressions. The default pattern is "*", which returns
        /// all files.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should
        /// include only the current directory or all subdirectories. The default value is
        /// System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of files that matches searchPattern and searchOption.</returns>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">searchOption is not a valid System.IO.SearchOption value.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfoInstant.EnumerateFiles(searchPattern, searchOption);
        }

        /// <summary>
        /// Returns an enumerable collection of file system information in the current directory.
        /// </summary>
        /// <returns>An enumerable collection of file system information in the current directory.</returns>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
        {
            return DirectoryInfoInstant.EnumerateFileSystemInfos();
        }

        /// <summary>
        /// Returns an enumerable collection of file system information that matches a specified search pattern.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories. This parameter can
        /// contain a combination of valid literal path and wildcard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which
        /// returns all files.</param>
        /// <returns>An enumerable collection of file system information objects that matches searchPattern.</returns>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            return DirectoryInfoInstant.EnumerateFileSystemInfos(searchPattern);
        }

        /// <summary>
        /// Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories. This parameter can
        /// contain a combination of valid literal path and wildcard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should
        /// include only the current directory or all subdirectories. The default value is System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of file system information objects that matches searchPattern and searchOption.</returns>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> searchOption is not a valid System.IO.SearchOption value.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfoInstant.EnumerateFileSystemInfos(searchPattern, searchOption);
        }

        /// <summary>
        /// Returns an array of directories in the current System.IO.DirectoryInfo matching
        /// the given search criteria and using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories. This parameter can
        /// contain a combination of valid literal path and wildcard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which
        /// returns all files.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should
        /// include only the current directory or all subdirectories.</param>
        /// <returns>An array of type DirectoryInfo matching searchPattern.</returns>
        /// <exception cref="ArgumentException">searchPattern contains one or more invalid characters defined 
        /// by the System.IO.Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">searchOption is not a valid System.IO.SearchOption value.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public DirectoryManager[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfoInstant
                .GetDirectories(searchPattern, searchOption)
                .ToDirectoryManager()
                .ToArray();
        }

        /// <summary>
        /// Returns an array of directories in the current System.IO.DirectoryInfo matching the given search criteria.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories. This parameter can
        /// contain a combination of valid literal path and wildcard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which
        /// returns all files.</param>
        /// <returns>An array of type DirectoryInfo matching searchPattern.</returns>
        /// <exception cref="ArgumentException">searchPattern contains one or more invalid characters defined
        /// by the System.IO.Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public DirectoryManager[] GetDirectories(string searchPattern)
        {
            return DirectoryInfoInstant
                .GetDirectories(searchPattern)
                .ToDirectoryManager()
                .ToArray();
        }

        /// <summary>
        /// Returns the subdirectories of the current directory.
        /// </summary>
        /// <returns>An array of System.IO.DirectoryInfo objects.</returns>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public DirectoryManager[] GetDirectories()
        {
            return DirectoryInfoInstant
                .GetDirectories()
                .ToDirectoryManager()
                .ToArray();
        }

        /// <summary>
        /// Returns a file list from the current directory matching the given search pattern
        /// and using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain
        /// a combination of valid literal path and wildcard (* and ?) characters, but it
        /// doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should
        /// include only the current directory or all subdirectories.</param>
        /// <returns>An array of type System.IO.FileInfo.</returns>
        /// <exception cref="ArgumentException">searchPattern contains one or more invalid characters 
        /// defined by the System.IO.Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentOutOfRangeException">searchOption is not a valid System.IO.SearchOption value.</exception>
        public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfoInstant
                .GetFiles();
        }

        /// <summary>
        /// Returns a file list from the current directory.
        /// </summary>
        /// <returns>An array of type System.IO.FileInfo.</returns>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public FileInfo[] GetFiles()
        {
            return DirectoryInfoInstant
                .GetFiles();
        }

        /// <summary>
        /// Returns a file list from the current directory matching the given search pattern.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain
        /// a combination of valid literal path and wildcard (* and ?) characters, but it
        /// doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
        /// <returns>An array of type System.IO.FileInfo.</returns>
        /// <exception cref="ArgumentException">searchPattern contains one or more invalid characters defined 
        /// by the System.IO.Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public FileInfo[] GetFiles(string searchPattern)
        {
            return DirectoryInfoInstant
                .GetFiles(searchPattern);
        }

        /// <summary>
        /// Returns an array of strongly typed System.IO.FileSystemInfo entries representing
        /// all the files and subdirectories in a directory.
        /// </summary>
        /// <returns>An array of strongly typed System.IO.FileSystemInfo entries.</returns>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public FileSystemInfo[] GetFileSystemInfos()
        {
            return DirectoryInfoInstant
                .GetFileSystemInfos();
        }

        /// <summary>
        ///  Retrieves an array of strongly typed System.IO.FileSystemInfo objects representing
        ///  the files and subdirectories that match the specified search criteria.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories and files. This parameter
        /// can contain a combination of valid literal path and wildcard (* and ?) characters,
        /// but it doesn't support regular expressions. The default pattern is "*", which returns all files.</param>
        /// <returns>An array of strongly typed FileSystemInfo objects matching the search criteria.</returns>
        /// <exception cref="ArgumentException">searchPattern contains one or more invalid characters 
        /// defined by the System.IO.Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException"> searchPattern is null.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            return DirectoryInfoInstant
                .GetFileSystemInfos(searchPattern);
        }

        /// <summary>
        /// Retrieves an array of System.IO.FileSystemInfo objects that represent the files
        /// and subdirectories matching the specified search criteria.
        /// </summary>
        /// <param name="searchPattern">The search string to match against the names of directories and files. This
        /// parameter can contain a combination of valid literal path and wildcard (* and
        /// ?) characters, but it doesn't support regular expressions. The default pattern
        /// is "*", which returns all files.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should
        /// include only the current directory or all subdirectories. The default value is
        /// System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>An array of file system entries that match the search criteria.</returns>
        /// <exception cref="ArgumentException">searchPattern contains one or more invalid characters 
        /// defined by the System.IO.Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">searchOption is not a valid System.IO.SearchOption value.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfoInstant
                .GetFileSystemInfos(searchPattern, searchOption);
        }

        #endregion

        #region Compare Methods

        /// <summary>
        /// compare the content of the current directory to the given directory content
        /// using the given compare Option
        /// Note that only Directory are compared the files are ignored
        /// </summary>
        /// <param name="directoryToCompare">the directory to compare to</param>
        /// <param name="outputOptions">the out put type</param>
        /// <param name="compareOptions">the compare option, has a default value set to CompareOptions.Name</param>
        /// <returns>the list of found directories</returns>
        /// <exception cref="DirectoryNotFoundException">Current Directory -or- Directory to compare cannot be found.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<DirectoryManager> Compare(
            DirectoryManager directoryToCompare,
            OutputOptions outputOptions,
            CompareOptions compareOptions = CompareOptions.Name)
        {
            return DirectoryInfoInstant
                .Compare(directoryToCompare.DirectoryInfoInstant, outputOptions, compareOptions)
                .ToDirectoryManager();
        }

        /// <summary>
        /// compare the content of the current directory to the given directory content
        /// using the given compare Option
        /// Note that only Directory are compared the files are ignored
        /// </summary>
        /// <param name="directoryToCompaire">the directory to compare to</param>
        /// <param name="outputOptions">the out put type</param>
        /// <param name="compareOptions">the compare option, has a default value set to CompareOptions.Name</param>
        /// <returns>the list of found directories</returns>
        /// <exception cref="DirectoryNotFoundException">Current Directory -or- Directory to compare cannot be found.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public async Task<IEnumerable<DirectoryManager>> CompareAsync(
            DirectoryManager directoryToCompaire,
            OutputOptions outputOptions,
            CompareOptions compareOptions = CompareOptions.Name)
        {
            return await Task.Run(()
                => Compare(directoryToCompaire.DirectoryInfoInstant, outputOptions, compareOptions));
        }

        #endregion

        #region Search Methods

        /// <summary>
        /// search in the sub directories of the current instant
        /// </summary>
        /// <param name="searchKey">the search keyword</param>
        /// <param name="searchOptions">search option</param>
        /// <returns>list of results found</returns>
        /// <exception cref="DirectoryNotFoundException">the current directory cannot be found</exception>
        /// <exception cref="RegexMatchTimeoutException">A time-out occurred.</exception>
        /// <exception cref="ArgumentException">if the search key is null</exception>
        public IEnumerable<DirectoryManager> Search(string searchKey, SearchOptions searchOptions)
        {
            return DirectoryInfoInstant
                .Search(searchKey, searchOptions)
                .ToDirectoryManager();
        }

        /// <summary>
        /// search in the sub directories of the current instant
        /// </summary>
        /// <param name="searchKey">the search keyword</param>
        /// <param name="searchOptions">search option</param>
        /// <exception cref="DirectoryNotFoundException">the current directory cannot be found</exception>
        /// <exception cref="RegexMatchTimeoutException">A time-out occurred.</exception>
        /// <exception cref="ArgumentException">if the search key is null</exception>
        public async Task<IEnumerable<DirectoryManager>> SearchAsync(string searchKey, SearchOptions searchOptions)
        {
            return await Task.Run(() => Search(searchKey, searchOptions));
        }

        #endregion

        #region Move Methods

        /// <summary>
        /// move a directory and it content to the new path leaving the directory with the same name
        /// </summary>
        /// <example>
        /// let say you have a folder in your Desktop called Folder1 "C:\...\Desktop\Folder1"
        /// and you want to move it to the Document Folder so your 'distDir' will be "C:\...\Document"
        /// so you only going to splice the destination folder, for that the new path of the folder will be
        /// "C:\...\Document\Folder1"
        /// </example>
        /// <param name="distDir">the destination folder</param>
        /// <exception cref="DirectoryNotFoundException">id The Current or destination directory cannot be found.</exception>
        /// <exception cref="ArgumentNullException">distDir is null.</exception>
        /// <exception cref="ArgumentException">distDir is an empty string (''").</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public void Move(string distDir)
        {
            var distPath = Path.Combine(distDir, Name);
            DirectoryInfoInstant.MoveTo(distPath);
        }

        /// <summary>
        /// move the folder to the desktop folder
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">The Current directory cannot be found.</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public void MoveToDesktop()
        {
            Move(GetDesktopPath());
        }

        #endregion

        #region Copy Methods

        /// <summary>
        /// copy the current directory to a given location
        /// </summary>
        /// <param name="destDirName">the destination where the directory should be copied</param>
        /// <param name="copySubDirs">true to copy sub directory, by default set to true</param>
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
        public void Copy(string destDirName, bool copySubDirs = true, bool overwriteFiles = false)
        {
            if (!destDirName.IsValid())
                throw new ArgumentException(Messages.InvalidDestPath);

            destDirName = Path.Combine(destDirName, Name);

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwriteFiles);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                DirectoryManager[] dirs = GetDirectories();
                foreach (DirectoryManager subdir in dirs)
                {
                    subdir.Copy(destDirName, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Copy the directory to the desktop folder
        /// </summary>
        /// <param name="copySubDirs">true to copy sub directory, by default set to true</param>
        /// <param name="overwriteFiles">true to overWrite existing files, by default set to false</param>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException"> The caller does not have the required permission.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception>
        /// <exception cref="IOException">An error occurs, or the destination file already exists and overwrite is false.</exception>
        public void CopyToDesktop(bool copySubDirs = true, bool overwriteFiles = false)
        {
            Copy(GetDesktopPath(), copySubDirs, overwriteFiles);
        }

        #endregion

        #region Rename Methods

        /// <summary>
        /// rename the Current Directory to the new given name
        /// </summary>
        /// <param name="newName">the new name</param>
        /// <exception cref="InvalidFolderNameException">if the name contains invalid characters</exception>
        /// <exception cref="ArgumentException">if the newName is null, empty or whitespace</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public void Rename(string newName)
        {
            if (!newName.IsValid())
                throw new ArgumentException(Messages.InvalidName);

            if (!Tools.IsValidFolderName(newName))
                throw new InvalidFolderNameException(Messages.InvalidFolderName);

            var theNewName = Path.Combine(Parent.FullName, newName);
            DirectoryInfoInstant.MoveTo(theNewName);
        }

        /// <summary>
        /// generate a random folder name
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public void GenerateRandomName()
        {
            Rename(RandomGenerator.RandomFolderName());
        }

        #endregion

        #region General Methods

        /// <summary>
        /// get the total size of the Current Directory
        /// </summary>
        /// <returns>the size in bytes</returns>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public async Task<long> GetTotalSizeAsync()
        {
            return await DirectoryInfoInstant.GetTotalSizeAsync();
        }

        /// <summary>
        /// Launch the folder viewer for the Current folder
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="UnknownOperatingSystemException">if unable to detect the current operation system</exception>
        public void LaunchFolderView()
        {
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
                            Arguments = $"/root,{FullName}",
                            UseShellExecute = true
                        };
                        break;
                    case OS.MacOS:
                        Process.StartInfo = new ProcessStartInfo
                        {
                            FileName = "open",
                            Arguments = $"-R {FullName}",
                            UseShellExecute = true
                        };
                        break;
                    case OS.Linux:
                        Process.StartInfo = new ProcessStartInfo()
                        {
                            FileName = "file://",
                            Arguments = FullName,
                        };
                        break;
                    case OS.Unknown:
                        throw new UnknownOperatingSystemException(Messages.CannotOpenFolderViewer);
                }

                Process.Start();
            }
        }

        /// <summary>
        /// return the full path to the desktop folder
        /// </summary>
        /// <returns>desktop path</returns>
        /// <exception cref="UnknownOperatingSystemException">if unable to detect the current operation system</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
        public static string GetDesktopPath()
        {
            return DirectoriesHelper.GetDesktopPath();
        }

        #endregion

        #region public Static Methods

        #region Move Methods

        /// <summary>
        /// move the list of directories to the new locations leaving the directories with the same name
        /// if the destination directory not exist it will be created
        /// </summary>
        /// <param name="lstDirToMove">list of the directories to be moved</param>
        /// <param name="distFolder">the destination folder</param>
        /// <exception cref="DirectoryNotFoundException">id The Current directory cannot be found.</exception>
        /// <exception cref="ArgumentNullException">distDir is null.</exception>
        /// <exception cref="ArgumentException">distDir is an empty string (''").</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void MoveAll(IEnumerable<DirectoryManager> lstDirToMove, string distFolder)
        {
            if (!Directory.Exists(distFolder))
                Directory.CreateDirectory(distFolder);

            foreach (var item in lstDirToMove)
            {
                item.Move(distFolder);
            }
        }

        /// <summary>
        /// move the list of directories to the new locations leaving the directories with the same name
        /// if the destination directory not exist it will be created
        /// </summary>
        /// <param name="lstDirToMove">list of the directories to be moved</param>
        /// <param name="distFolder">the destination folder</param>
        /// <exception cref="DirectoryNotFoundException">id The Current directory cannot be found.</exception>
        /// <exception cref="ArgumentNullException">distDir is null.</exception>
        /// <exception cref="ArgumentException">distDir is an empty string (''").</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void MoveAll(IEnumerable<DirectoryInfo> lstDirToMove, string distFolder)
        {
            if (!Directory.Exists(distFolder))
                Directory.CreateDirectory(distFolder);

            foreach (var item in lstDirToMove)
            {
                item.Move(distFolder);
            }
        }

        /// <summary>
        /// move the list of directories to the new locations leaving the directories with the same name
        /// </summary>
        /// <param name="lstDirToMove">list of the directories to be moved</param>
        /// <param name="distFolder">the destination folder</param>
        /// <exception cref="DirectoryNotFoundException">id The Current directory cannot be found.</exception>
        /// <exception cref="ArgumentNullException">distDir is null.</exception>
        /// <exception cref="ArgumentException">distDir is an empty string (''").</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static async void MoveAllAsync(IEnumerable<DirectoryManager> lstDirToMove, string distFolder)
        {
            await Task.Run(() => MoveAll(lstDirToMove, distFolder));
        }

        /// <summary>
        /// move the list of directories to the new locations leaving the directories with the same name
        /// </summary>
        /// <param name="lstDirToMove">list of the directories to be moved</param>
        /// <param name="distFolder">the destination folder</param>
        /// <exception cref="DirectoryNotFoundException">id The Current directory cannot be found.</exception>
        /// <exception cref="ArgumentNullException">distDir is null.</exception>
        /// <exception cref="ArgumentException">distDir is an empty string (''").</exception>
        /// <exception cref="IOException"> An attempt was made to move a directory to a different volume.
        /// -or- destDirName already exists. 
        /// -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static async void MoveAllAsync(IEnumerable<DirectoryInfo> lstDirToMove, string distFolder)
        {
            await Task.Run(() => MoveAll(lstDirToMove, distFolder));
        }

        #endregion

        #region Copy Methods

        /// <summary>
        /// copy the list directories to the given location
        /// </summary>
        /// <param name="directories">list of directories to copy</param>
        /// <param name="destFolder">the destination where the directories should be copied</param>
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
        public static void CopyAll(IEnumerable<DirectoryManager> directories,
            string destFolder,
            bool copySubDirs = true,
            bool overwriteFiles = false)
        {
            foreach (var dir in directories)
            {
                dir.Copy(destFolder, copySubDirs, overwriteFiles);
            }
        }

        /// <summary>
        /// copy the list directories to the given location
        /// </summary>
        /// <param name="directories">list of directories to copy</param>
        /// <param name="destFolder">the destination where the directories should be copied</param>
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
        public static void CopyAll(IEnumerable<DirectoryInfo> directories,
            string destFolder,
            bool copySubDirs = true,
            bool overwriteFiles = false)
        {
            foreach (var dir in directories)
            {
                dir.Copy(destFolder, copySubDirs, overwriteFiles);
            }
        }

        /// <summary>
        /// copy the list directories to the given location
        /// </summary>
        /// <param name="directories">list of directories to copy</param>
        /// <param name="destFolder">the destination where the directories should be copied</param>
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
        public static async void CopyAllAsync(IEnumerable<DirectoryManager> directories,
            string destFolder,
            bool copySubDirs = true,
            bool overwriteFiles = false)
        {
            await Task.Run(() => CopyAll(directories, destFolder, copySubDirs, overwriteFiles));
        }

        /// <summary>
        /// copy the list directories to the given location
        /// </summary>
        /// <param name="directories">list of directories to copy</param>
        /// <param name="destFolder">the destination where the directories should be copied</param>
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
        public static async void CopyAllAsync(IList<DirectoryInfo> directories,
            string destFolder,
            bool copySubDirs = true,
            bool overwriteFiles = false)
        {
            await Task.Run(() => CopyAll(directories, destFolder, copySubDirs, overwriteFiles));
        }

        #endregion

        #region Rename Methods

        /// <summary>
        /// Method used to rename a set of directories based on a Rename option
        /// </summary>
        /// <param name="directories">the list of directories to rename</param>
        /// <param name="options">the option for renaming, by default is set to 'RenameOptions.Default'</param>
        /// <param name="separator">separator is used when renaming by adding numbers and letters, by default is set to '-'</param>
        /// <param name="startFrom">startFrom is used when renaming by adding numbers, by default is set to '1'</param>
        /// <param name="pattern">pattern is used when renaming with Regex, by default is set to empty string</param>
        /// <param name="Replacewith">ReplaceWith is used when renaming by replacing a regex pattern, by default is set to empty string</param>
        /// <param name="regexOptions">the regex option, by default is set to 'RegexOptions.IgnoreCase'</param>
        /// <exception cref="InvalidFolderNameException">if the name contains invalid characters</exception>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="ArgumentException">if the newName is null, empty or whitespace</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void RenameAll(IEnumerable<DirectoryManager> directories,
            RenameOptions options = RenameOptions.Default,
            char separator = '-',
            string pattern = "",
            int startFrom = 1,
            string Replacewith = "",
            RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            RenameAll(directories.ToDirectoryInfo(),
                options, separator, pattern, startFrom, Replacewith, regexOptions);
        }

        /// <summary>
        /// Method used to rename a set of directories based on a Rename option
        /// </summary>
        /// <param name="directories">the list of directories to rename</param>
        /// <param name="options">the option for renaming, by default is set to 'RenameOptions.Default'</param>
        /// <param name="separator">separator is used when renaming by adding numbers and letters, by default is set to '-'</param>
        /// <param name="startFrom">startFrom is used when renaming by adding numbers, by default is set to '1'</param>
        /// <param name="pattern">pattern is used when renaming with Regex, by default is set to empty string</param>
        /// <param name="Replacewith">ReplaceWith is used when renaming by replacing a regex pattern, by default is set to empty string</param>
        /// <param name="regexOptions">the regex option, by default is set to 'RegexOptions.IgnoreCase'</param>
        /// <exception cref="InvalidFolderNameException">if the name contains invalid characters</exception>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="ArgumentException">if the newName is null, empty or whitespace</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static void RenameAll(IEnumerable<DirectoryInfo> directories,
            RenameOptions options = RenameOptions.Default,
            char separator = '-',
            string pattern = "",
            int startFrom = 1,
            string Replacewith = "",
            RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            switch (options)
            {
                case RenameOptions.UseUniqueName:
                    DirectoriesHelper.RenameByUsingUniqueName(directories, Replacewith);
                    break;
                case RenameOptions.GenerateRandomName:
                    DirectoriesHelper.RenameByGeneratingRandomNames(directories);
                    break;
                case RenameOptions.AddRandomLettersToEnd:
                    DirectoriesHelper.RenameByAddingRandomLettersToEnd(directories, separator);
                    break;
                case RenameOptions.RemoveMatchedRegexPattern:
                    DirectoriesHelper.RenameByRemovingMatchedRegexPattern(directories, pattern, regexOptions);
                    break;
                case RenameOptions.ReplaceMatchedRegexPattern:
                    DirectoriesHelper.RenameByReplacingMatchedRegexPattern(directories, pattern, Replacewith);
                    break;
                case RenameOptions.AddIncrementalNumbersToEnd:
                    DirectoriesHelper.RenameByAddingIncrementalNumbersToEnd(directories, separator, startFrom);
                    break;
                case RenameOptions.AddIncrementalNumbersToBeginning:
                    DirectoriesHelper.RenameByAddingIncrementalNumbersToBeginning(directories, separator, startFrom);
                    break;
                case RenameOptions.Default:
                default:
                    DirectoriesHelper.RenameByAddingIncrementalNumbersToBeginning(directories, '-', 1);
                    break;
            }
        }

        /// <summary>
        /// Method used to rename a set of directories based on a Rename option
        /// </summary>
        /// <param name="directories">the list of directories to rename</param>
        /// <param name="options">the option for renaming, by default is set to 'RenameOptions.Default'</param>
        /// <param name="separator">separator is used when renaming by adding numbers and letters, by default is set to '-'</param>
        /// <param name="startFrom">startFrom is used when renaming by adding numbers, by default is set to '1'</param>
        /// <param name="pattern">pattern is used when renaming with Regex, by default is set to empty string</param>
        /// <param name="Replacewith">ReplaceWith is used when renaming by replacing a regex pattern, by default is set to empty string</param>
        /// <param name="regexOptions">the regex option, by default is set to 'RegexOptions.IgnoreCase'</param>
        /// <exception cref="InvalidFolderNameException">if the name contains invalid characters</exception>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="ArgumentException">if the newName is null, empty or whitespace</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public static async void RenameAllAsync(IEnumerable<DirectoryManager> directories,
            RenameOptions options = RenameOptions.Default,
            char separator = '-',
            string pattern = "",
            int startFrom = 1,
            string Replacewith = "",
            RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            await Task.Run(() => { RenameAll(directories, options, separator, pattern, startFrom, Replacewith, regexOptions); });
        }

        /// <summary>
        /// Method used to rename a set of directories based on a Rename option
        /// </summary>
        /// <param name="directories">the list of directories to rename</param>
        /// <param name="options">the option for renaming, by default is set to 'RenameOptions.Default'</param>
        /// <param name="separator">separator is used when renaming by adding numbers and letters, by default is set to '-'</param>
        /// <param name="startFrom">startFrom is used when renaming by adding numbers, by default is set to '1'</param>
        /// <param name="pattern">pattern is used when renaming with Regex, by default is set to empty string</param>
        /// <param name="Replacewith">ReplaceWith is used when renaming by replacing a regex pattern, by default is set to empty string</param>
        /// <param name="regexOptions">the regex option, by default is set to 'RegexOptions.IgnoreCase'</param>
        /// <exception cref="InvalidFolderNameException">if the name contains invalid characters</exception>
        /// <exception cref="DirectoryNotFoundException">if the Renamed Directory not exist</exception>
        /// <exception cref="ArgumentException">if the newName is null, empty or whitespace</exception>
        /// <exception cref="IOException">An attempt was made to move a directory to a different volume. 
        /// -or- destDirName already exists. -or- You are not authorized to access this path. 
        /// -or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public async static void RenameAllAsync(IEnumerable<DirectoryInfo> directories,
            RenameOptions options = RenameOptions.Default,
            char separator = '-',
            string pattern = "",
            int startFrom = 1,
            string Replacewith = "",
            RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            await Task.Run(() =>
            { RenameAll(directories, options, separator, pattern, startFrom, Replacewith, regexOptions); });
        }

        #endregion

        #endregion
    }
}

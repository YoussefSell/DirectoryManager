namespace System.IO.Expand
{
    /// <summary>
    /// a call that expose more functionality on the Directory Info Object
    /// </summary>
    [Diagnostics.DebuggerStepThrough]
    public partial class DirectoryManager : IDisposable
    {
        #region Public Props

        /// <summary>
        /// get or set the directory instant
        /// </summary>
        public DirectoryInfo DirectoryInfoInstant { get; private set; }

        /// <summary>
        /// Enable Content Changed Watcher to raise event when the content of directory changed, by default set to false
        /// </summary>
        public bool EnableContentChangedWatcher
        {
            get => _enableContentChangedWatcher;
            set
            {
                if (value)
                    CreateFileWatcher();
                else
                    Dispose();
            }
        }

        /// <summary>
        /// Gets the name of the managed directory instance.
        /// </summary>
        public string Name { get => DirectoryInfoInstant.Name; }

        /// <summary>
        ///  Represents the fully qualified path of the directory or file.
        /// </summary>
        /// <exception cref="PathTooLongException">The fully qualified path is 260 or more characters.</exception>
        public string FullName { get => DirectoryInfoInstant.FullName; }

        /// <summary>
        /// Gets a value indicating whether the directory exists or not.
        /// </summary>
        public bool Exists { get => Directory.Exists(FullName); }

        /// <summary>
        /// Gets the parent directory of a specified subdirectory.
        /// this may return null in case the path is null or the path is a root
        /// </summary>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        public DirectoryInfo Parent { get => DirectoryInfoInstant.Parent; }

        /// <summary>
        /// Gets the root portion of the directory.
        /// </summary>
        /// <exception cref="Security.SecurityException"></exception>
        public DirectoryInfo Root { get => DirectoryInfoInstant.Root; }

        /// <summary>
        /// get the total size of the directory
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">if the Directory not exist</exception>
        /// <exception cref="Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public long TotalSize { get => DirectoryInfoInstant.GetTotalSize(); private set { } }

        /// <summary>
        /// get the total count of the sub directories
        /// </summary>
        public int TotalDirectories { get => GetTotalSubDirs(); }

        /// <summary>
        /// get the total count of files this directory contains
        /// </summary>
        public int TotalFiles { get => GetTotalSubFiles(); }

        /// <summary>
        ///  Gets or sets the attributes for the current file or directory.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentException"> The caller attempts to set an invalid file attribute. 
        /// -or- The user attempts to set an attribute value but doesn't have write permission.</exception>
        /// <exception cref="Security.SecurityException">The caller doesn't have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid.</exception>
        public FileAttributes Attributes
        {
            get => DirectoryInfoInstant.Attributes;
            set => DirectoryInfoInstant.Attributes = value;
        }

        /// <summary>
        /// Gets or sets the time when the current file or directory was last written to.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        public DateTime LastWriteTime
        {
            get => DirectoryInfoInstant.LastWriteTime;
            set => DirectoryInfoInstant.LastWriteTime = value;
        }

        /// <summary>
        /// Gets or sets the time, in coordinated universal time (UTC), when the current
        /// file or directory was last written to.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The caller attempts to set an invalid creation time.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        public DateTime LastWriteTimeUtc
        {
            get => DirectoryInfoInstant.LastWriteTimeUtc;
            set => DirectoryInfoInstant.LastWriteTimeUtc = value;
        }

        /// <summary>
        ///  Gets or sets the time the current file or directory was last accessed.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        public DateTime LastAccessTime
        {
            get => DirectoryInfoInstant.LastAccessTime;
            set => DirectoryInfoInstant.LastAccessTime = value;
        }

        /// <summary>
        ///  Gets or sets the time, in coordinated universal time (UTC), that the current
        ///  file or directory was last accessed.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        public DateTime LastAccessTimeUtc
        {
            get => DirectoryInfoInstant.LastAccessTimeUtc;
            set => DirectoryInfoInstant.LastAccessTimeUtc = value;
        }

        /// <summary>
        /// Gets or sets the creation time of the current directory.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The caller attempts to set an invalid creation time.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
        public DateTime CreationTime
        {
            get => DirectoryInfoInstant.CreationTime;
            set => DirectoryInfoInstant.CreationTime = value;
        }

        /// <summary>
        /// Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.
        /// </summary>
        /// <exception cref="IOException">System.IO.FileSystemInfo.Refresh cannot initialize the data.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
        /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid.</exception>
        public DateTime CreationTimeUtc
        {
            get => DirectoryInfoInstant.CreationTimeUtc;
            set => DirectoryInfoInstant.CreationTimeUtc = value;
        }

        #endregion

        #region Events

        /// <summary>
        /// event for monitoring the Directory content, the event raised when the content of the directory changed
        /// </summary>
        public event EventHandler<DirectoryContentChangedEventArgs> DirectoryContentChanged;

        #endregion

        #region Constructor, Destructor, Dispose Method and implicit conversion 

        /// <summary>
        /// Initializes a new instance of the System.IO.Expand.DirectoryManager class on the specified path.
        /// the path must be exist in order to instantiate the class or an exception will be thrown
        /// </summary>
        /// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
        /// <param name="createIfNotExist">a boolean flag indicating whether to create the directory 
        /// if it not exist or not, if the directory not exist and flag set to false an exception will be raised.
        /// by default it set to false.</param>
        /// <exception cref="ArgumentException">the provided path contains invalid characters</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">the specified path not exist.</exception>
        /// <exception cref="ArgumentNullException">the provided path is null.</exception>
        /// <exception cref="Security.SecurityException">the provided path is null.</exception>
        public DirectoryManager(string path, bool createIfNotExist = false)
        {
            if (!Directory.Exists(path) && !createIfNotExist)
                throw new DirectoryNotFoundException(Messages.DirectoriesNotExist);

            if (!Directory.Exists(path) && createIfNotExist)
            {
                DirectoryInfoInstant = Directory.CreateDirectory(path);
                return;
            }

            DirectoryInfoInstant = new DirectoryInfo(path);
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.Expand.DirectoryManager class on the specified path.
        /// the path must be exist in order to instantiate the class or an exception will be thrown
        /// </summary>
        /// <param name="directoryInfo">A string specifying the path on which to create the DirectoryInfo.</param>
        /// <param name="createIfNotExist">a boolean flag indicating whether to create the directory 
        /// if it not exist or not, if the directory not exist and flag set to false an exception will be raised.
        /// by default it set to false.</param>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">the directoryInfo path not exist.</exception>
        /// <exception cref="ArgumentNullException">the provided directoryInfo is null.</exception>
        /// <exception cref="Security.SecurityException">the provided path is null.</exception>
        public DirectoryManager(DirectoryInfo directoryInfo, bool createIfNotExist = false)
            : this(directoryInfo.FullName, createIfNotExist) { }

        /// <summary>
        /// the Destructor
        /// </summary>
        ~DirectoryManager()
        {
            _enableContentChangedWatcher = false;
            _watcher?.Dispose();
            _watcher = null;
        }

        /// <summary>
        /// Releases used resources
        /// </summary>
        public void Dispose()
        {
            _enableContentChangedWatcher = false;
            _watcher?.Dispose();
            _watcher = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// implicit conversion from DirectoryManager to DirectoryInfo
        /// </summary>
        /// <param name="directoryManager"></param>
        public static implicit operator DirectoryInfo(DirectoryManager directoryManager)
        {
            return directoryManager.DirectoryInfoInstant;
        }

        /// <summary>
        /// implicit conversion from DirectoryInfo to DirectoryManager
        /// </summary>
        /// <param name="directoryInfo"></param>
        public static implicit operator DirectoryManager(DirectoryInfo directoryInfo)
        {
            return new DirectoryManager(directoryInfo);
        }

        #endregion
    }
}

namespace System.IO.Expand
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    /// <summary>
    /// simple Tools Class
    /// </summary>
    [Diagnostics.DebuggerStepThrough]
    static class Tools
    {
        /// <summary>
        /// get the operation system currently running
        /// </summary>
        /// <returns>the current OS</returns>
        internal static OS GetOperationSystem()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isLunix = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isWindows)
                return OS.Windows;

            if (isMac)
                return OS.MacOS;

            if (isLunix)
                return OS.Linux;

            return OS.Unknown;
        }

        /// <summary>
        /// replace the string with a empty string ""
        /// </summary>
        /// <param name="value">the value to replace</param>
        /// <param name="pattern">the regex pattern</param>
        /// <param name="regexOptions">the regex option</param>
        /// <returns>the new string</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        internal static string ReplaceWithEmptyString(this string value, string pattern, RegexOptions regexOptions)
        {
            return value.ReplaceWithValue("", pattern, regexOptions);
        }

        /// <summary>
        /// replace the string with a given value
        /// </summary>
        /// <param name="value">the value to replace</param>
        /// <param name="replaceWith">the value to replace with</param>
        /// <param name="pattern">the regex pattern</param>
        /// <param name="regexOptions">the regex option</param>
        /// <returns>the new string</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        internal static string ReplaceWithValue(this string value, string replaceWith, string pattern, RegexOptions regexOptions)
        {
            return Regex.Replace(value, pattern, replaceWith, regexOptions);
        }

        /// <summary>
        /// Method for Converting IEnumerable of DirectoryInfo to IEnumerable of DirectoryManager
        /// </summary>
        /// <param name="directoryInfos">list of Directory Info</param>
        /// <returns>an IEnumerable of DirectoryManager</returns>
        public static IEnumerable<DirectoryManager> ToDirectoryManager(this IEnumerable<DirectoryInfo> directoryInfos)
        {
            return directoryInfos.Select(d => (DirectoryManager)d);
        }

        /// <summary>
        /// Method for Converting IEnumerable of DirectoryManager to IEnumerable of DirectoryInfo
        /// </summary>
        /// <param name="directoryManager">list of Directory Manager</param>
        /// <returns>an IEnumerable of DirectoryManager</returns>
        public static IEnumerable<DirectoryInfo> ToDirectoryInfo(this IEnumerable<DirectoryManager> directoryManager)
        {
            return directoryManager.Select(d => (DirectoryInfo)d);
        }

        #region Validation

        /// <summary>
        /// check if the given input is a match with the given pattern
        /// </summary>
        /// <param name="input">input to validate</param>
        /// <param name="pattern">the regex pattern</param>
        /// <returns>true if Match</returns>
        internal static bool IsMatch(string input, string pattern)
        {
            if (Regex.IsMatch(input, pattern))
                return true;

            return false;
        }

        /// <summary>
        /// check if the given input is a valid with the given pattern
        /// </summary>
        /// <param name="input">input to validate</param>
        /// <param name="pattern">the regex pattern</param>
        /// <param name="regexOptions">the regex Options</param>
        /// <returns>true if match</returns>
        internal static bool IsMatch(string input, string pattern, RegexOptions regexOptions)
        {
            if (Regex.IsMatch(input, pattern, regexOptions))
                return true;

            return false;
        }

        /// <summary>
        /// check if the given folder name is a valid folder name
        /// </summary>
        /// <param name="name">the name to check</param>
        /// <returns>true if valid, false if not</returns>
        internal static bool IsValidFolderName(string name)
        {
            return !IsMatch(name, @"([<>\?\*\\\""/\|])+");
        }

        /// <summary>
        /// check if the given separator is valid
        /// </summary>
        /// <param name="separator">the separator to check</param>
        /// <returns>true if valid, false if not</returns>
        internal static bool IsValidSeparator(char separator)
        {
            return !IsMatch(separator.ToString(), @"(\w)+|([<>\?\*\\\""/\|])+");
        }

        /// <summary>
        /// validate the string, check if is not null or empty or whiteSpace
        /// </summary>
        /// <param name="val">the string to validate</param>
        /// <returns>true if valid</returns>
        internal static bool IsValid(this string val)
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val)) return false;

            return true;
        }

        #endregion
    }
}

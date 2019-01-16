namespace System.IO.Expand
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// This implementation defines a very simple comparison  
    /// between two DirectoryInfo objects. It only compares the FullName  
    /// of the files being compared.
    /// </summary>
    [Diagnostics.DebuggerStepThrough]
    class DirectoryInfoCompareWithFullName : IEqualityComparer<DirectoryInfo>
    {
        /// <summary>
        /// check if the two objects are equals
        /// </summary>
        /// <param name="dirToLeft">the directory to the left of the comparison</param>
        /// <param name="dirToRight">the directory to the right of the comparison</param>
        /// <returns>true if equals</returns>
        public bool Equals(DirectoryInfo dirToLeft, DirectoryInfo dirToRight)
        {
            return dirToLeft.FullName == dirToRight.FullName;
        }

        /// <summary>
        /// Return a hash that reflects the comparison criteria.According to the
        /// rules for IEqualityComparer of T, if Equals is true, then the hash codes must  
        /// also be equal. Because equality as defined here is a simple value equality, not  
        /// reference identity, it is possible that two or more objects will produce the same  
        /// hash code.  
        /// </summary>
        /// <param name="dir">the directory to get the has code for</param>
        /// <returns>the hash code</returns>
        public int GetHashCode(DirectoryInfo dir)
        {
            return $"{dir.FullName}".GetHashCode();
        }
    }
}

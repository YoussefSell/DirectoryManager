namespace System.IO.Expand
{
    using System.Text;

    /// <summary>
    /// class used to generate random values
    /// </summary>
    [Diagnostics.DebuggerStepThrough]
    static class RandomGenerator
    {
        /// <summary>
        /// Generate a random string with a given size  
        /// </summary>
        /// <param name="size">the size of the string</param>
        /// <param name="UpperCase">use upper case or not</param>
        /// <returns>a random string</returns>
        public static string RandomString(int size = 4, bool UpperCase = true)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (UpperCase)
                return builder.ToString().ToUpper();

            return builder.ToString();
        }

        /// <summary>
        /// generate a random Folder Name
        /// </summary>
        /// <returns>folder name</returns>
        public static string RandomFolderName()
        {
            return Path.GetRandomFileName();
        }
    }
}

namespace MinimalApiStartupProject.Infrastructures.StringExtensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Method to update first letter of string to lower
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstLetterToLower(this string input)
        {
            return $"{input.Substring(0, 1).ToLower()}{input.Substring(1)}";
        }
    }
}
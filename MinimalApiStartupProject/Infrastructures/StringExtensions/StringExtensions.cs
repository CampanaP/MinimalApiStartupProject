namespace $safeprojectname$.Infrastructures.StringExtensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToLower(this string input)
        {
            return $"{input.Substring(0, 1).ToLower()}{input.Substring(1)}";
        }
    }
}
namespace $safeprojectname$.Modules.Sql.ExtensionMethods
{
	public static class StringExtensionMethod
	{
		public static string FirstLetterToLower(this string input)
		{
			return $"{input.Substring(0, 1).ToLower()}{input.Substring(1)}";
		}
	}
}
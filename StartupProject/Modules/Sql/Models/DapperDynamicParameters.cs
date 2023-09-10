using Dapper;
using System.Reflection;
using $safeprojectname$.Modules.Sql.ExtensionMethods;

namespace $safeprojectname$.Modules.Sql.Models
{
	public class DapperDynamicParameters : DynamicParameters
	{
		public DapperDynamicParameters(object parameters)
		{
			Dictionary<string, object?> dictionary = new Dictionary<string, object?>();

			PropertyInfo[]? properties = parameters.GetType().GetProperties();
			if (properties is null || !properties.Any())
			{
				return;
			}

			foreach (PropertyInfo property in properties)
			{
				dictionary.Add($"@{property.Name.FirstLetterToLower()}", property.GetValue(parameters));
			}

			AddDynamicParams(dictionary);
		}
	}
}
using System.Diagnostics.CodeAnalysis;

namespace MinimalApiStartupProject.Modules.Api.Models
{
	public class RequestHeader
	{
		public required string Name { get; set; }

		public required string Value { get; set; }

		[SetsRequiredMembers]
		public RequestHeader(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
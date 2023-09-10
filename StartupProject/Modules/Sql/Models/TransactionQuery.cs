using Dapper;

namespace $safeprojectname$.Modules.Sql.Models
{
	public class TransactionQuery
	{
		public required string Query { get; set; }

		public DynamicParameters? Parameters { get; set; }
	}
}
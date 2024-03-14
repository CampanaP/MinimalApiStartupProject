using Dapper;
using $safeprojectname$.Modules.Sql.Models;

namespace $safeprojectname$.Modules.Sql.Interfaces.Repositories
{
	public interface ISqlRepository
	{
		Task<IEnumerable<T>> ExecuteQuery<T>(string connectionString, string query, DynamicParameters? parameters = null);

		Task<T?> ExecuteFirstQuery<T>(string connectionString, string query, DynamicParameters? parameters = null);

		Task<int> ExecuteCountQuery(string connectionString, string query, DynamicParameters? parameters = null);

		Task ExecuteNonQuery(string connectionString, string query, DynamicParameters? parameters = null);

		Task ExecuteTransaction(string connectionString, List<TransactionQuery> transactionQueries);
	}
}
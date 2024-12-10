using Dapper;
using MinimalApiStartupProject.Modules.Sql.Models;

namespace MinimalApiStartupProject.Modules.Sql.Interfaces.Repositories
{
	public interface ISqlRepository
	{
		Task<IEnumerable<T>> ExecuteQueryAsync<T>(string connectionString, string query, DynamicParameters? parameters = null);

		Task<T?> ExecuteFirstQueryAsync<T>(string connectionString, string query, DynamicParameters? parameters = null);

		Task<int> ExecuteCountQueryAsync(string connectionString, string query, DynamicParameters? parameters = null);

		Task ExecuteNonQueryAsync(string connectionString, string query, DynamicParameters? parameters = null);

		Task ExecuteTransactionAsync(string connectionString, List<TransactionQuery> transactionQueries);
	}
}
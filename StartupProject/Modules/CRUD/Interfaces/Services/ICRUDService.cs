using Dapper;
using $safeprojectname$.Modules.Sql.Models;

namespace $safeprojectname$.Modules.CRUD.Interfaces.Services
{
	public interface ICRUDService
	{
		Task<T?> GetItem<T>(string connectionStringName, string query, DynamicParameters? parameters = null);

		Task<IEnumerable<T>> GetItems<T>(string connectionStringName, string query, DynamicParameters? parameters = null);

		Task SaveItem(string connectionStringName, string query, DynamicParameters parameters);

		Task SaveItems(string connectionStringName, List<TransactionQuery> queries);
	}
}
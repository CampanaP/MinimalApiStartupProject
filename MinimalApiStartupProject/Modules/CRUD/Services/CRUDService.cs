using Dapper;
using Serilog;
using $safeprojectname$.Modules.CRUD.Interfaces.Services;
using $safeprojectname$.Modules.Sql.Interfaces.Repositories;
using $safeprojectname$.Modules.Sql.Models;

namespace $safeprojectname$.Modules.CRUD.Services
{
	public class CRUDService : ICRUDService
	{
		private readonly IConfiguration _configuration;
		private readonly ISqlRepository _sqlRepository;

		public CRUDService(IConfiguration configuration, ISqlRepository sqlRepository)
		{
			_configuration = configuration;
			_sqlRepository = sqlRepository;
		}

		public async Task<IEnumerable<T>> GetItems<T>(string connectionStringName, string query, DynamicParameters? parameters = null)
		{
			IEnumerable<T> items = Enumerable.Empty<T>();

			string? connectionString = _configuration.GetConnectionString(connectionStringName);
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				Log.Error($"{nameof(CRUDService)} - {nameof(GetItems)} - {query}: ConnectionString is empty");

				return items;
			}

			items = await _sqlRepository.ExecuteQuery<T>(connectionString, query, parameters);

			return items;
		}

		public async Task<T?> GetItem<T>(string connectionStringName, string query, DynamicParameters? parameters = null)
		{
			T? item = default;

			string? connectionString = _configuration.GetConnectionString(connectionStringName);
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				Log.Error($"{nameof(CRUDService)} - {nameof(GetItem)} - {query}: ConnectionString is empty");

				return item;
			}

			item = await _sqlRepository.ExecuteFirstQuery<T>(connectionString, query, parameters);

			return item;
		}

		public async Task SaveItems(string connectionStringName, List<TransactionQuery> queries)
		{
			string? connectionString = _configuration.GetConnectionString(connectionStringName);
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				Log.Error($"{nameof(CRUDService)} - {nameof(SaveItems)}: ConnectionString is empty");

				return;
			}

			await _sqlRepository.ExecuteTransaction(connectionString, queries);

			return;
		}

		public async Task SaveItem(string connectionStringName, string query, DynamicParameters parameters)
		{
			string? connectionString = _configuration.GetConnectionString(connectionStringName);
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				Log.Error($"{nameof(CRUDService)} - {nameof(SaveItem)} - {query}: ConnectionString is empty");

				return;
			}

			await _sqlRepository.ExecuteNonQuery(connectionString, query, parameters);

			return;
		}
	}
}
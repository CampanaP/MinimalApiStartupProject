using Dapper;
using MinimalApiStartupProject.Infrastructures.Attributes;
using MinimalApiStartupProject.Infrastructures.StringExtensions;
using MinimalApiStartupProject.Modules.CRUD.Interfaces.Services;
using MinimalApiStartupProject.Modules.Sql.Interfaces.Repositories;
using MinimalApiStartupProject.Modules.Sql.Models;
using System.Text.Json;

namespace MinimalApiStartupProject.Modules.CRUD.Services
{
    [ScopedLifetime]
    public class CRUDService : ICRUDService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CRUDService> _logger;
        private readonly ISqlRepository _sqlRepository;

        public CRUDService(IConfiguration configuration, ILogger<CRUDService> logger, ISqlRepository sqlRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _sqlRepository = sqlRepository;
        }

        /// <summary>
        /// Async method to get items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionStringName"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetItemsAsync<T>(string connectionStringName, string query, DynamicParameters? parameters = null)
        {
            IEnumerable<T> items = Enumerable.Empty<T>();

            string? connectionString = _configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.LogException(nameof(CRUDService), nameof(GetItemsAsync), query, "ConnectionString is empty");

                return items;
            }

            items = await _sqlRepository.ExecuteQueryAsync<T>(connectionString, query, parameters);

            return items;
        }

        /// <summary>
        /// Async method to get single item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionStringName"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T?> GetItemAsync<T>(string connectionStringName, string query, DynamicParameters? parameters = null)
        {
            T? item = default(T?);

            string? connectionString = _configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.LogException(nameof(CRUDService), nameof(GetItemsAsync), query, "ConnectionString is empty");

                return item;
            }

            item = await _sqlRepository.ExecuteFirstQueryAsync<T>(connectionString, query, parameters);

            return item;
        }

        /// <summary>
        /// Async method to save items
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
        public async Task SaveItemsAsync(string connectionStringName, List<TransactionQuery> queries)
        {
            string? connectionString = _configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.LogException(nameof(CRUDService), nameof(GetItemsAsync), JsonSerializer.Serialize(queries), "ConnectionString is empty");

                return;
            }

            await _sqlRepository.ExecuteTransactionAsync(connectionString, queries);

            return;
        }

        /// <summary>
        /// Async method to save single item
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task SaveItemAsync(string connectionStringName, string query, DynamicParameters parameters)
        {
            string? connectionString = _configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _logger.LogException(nameof(CRUDService), nameof(GetItemsAsync), query, "ConnectionString is empty");

                return;
            }

            await _sqlRepository.ExecuteNonQueryAsync(connectionString, query, parameters);

            return;
        }
    }
}
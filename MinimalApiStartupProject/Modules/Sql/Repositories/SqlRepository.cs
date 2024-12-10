using Dapper;
using Microsoft.Data.SqlClient;
using MinimalApiStartupProject.Infrastructures.Attributes;
using MinimalApiStartupProject.Infrastructures.StringExtensions;
using MinimalApiStartupProject.Modules.Sql.Interfaces.Repositories;
using MinimalApiStartupProject.Modules.Sql.Models;
using System.Text.Json;

namespace MinimalApiStartupProject.Modules.Sql.Repositories
{
    [ScopedLifetime]
    public class SqlRepository : ISqlRepository
    {
        private readonly ILogger<SqlRepository> _logger;

        public SqlRepository(ILogger<SqlRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Async method to execute select query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string connectionString, string query, DynamicParameters? parameters = null)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    await command.Connection.OpenAsync();

                    results = await command.Connection.QueryAsync<T>(query, parameters);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogException(nameof(SqlRepository), nameof(ExecuteQueryAsync), query, ex.Message);

                throw;
            }

            return results;
        }

        /// <summary>
        /// Async method to execute select query with top 1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T?> ExecuteFirstQueryAsync<T>(string connectionString, string query, DynamicParameters? parameters = null)
        {
            T? result = default;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    await command.Connection.OpenAsync();

                    result = await command.Connection.QueryFirstOrDefaultAsync<T?>(query, parameters);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogException(nameof(SqlRepository), nameof(ExecuteFirstQueryAsync), query, ex.Message);

                throw;
            }

            return result;
        }

        /// <summary>
        /// Async method to execute query with count
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCountQueryAsync(string connectionString, string query, DynamicParameters? parameters = null)
        {
            int result = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    await command.Connection.OpenAsync();

                    result = await command.Connection.QueryFirstAsync<int>(query, parameters);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogException(nameof(SqlRepository), nameof(ExecuteCountQueryAsync), query, ex.Message);

                throw;
            }

            return result;
        }

        /// <summary>
        /// Async method to execute command query 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task ExecuteNonQueryAsync(string connectionString, string query, DynamicParameters? parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    await command.Connection.OpenAsync();

                    await command.Connection.QueryAsync(query, parameters);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogException(nameof(SqlRepository), nameof(ExecuteNonQueryAsync), query, ex.Message);

                throw;
            }

            return;
        }

        /// <summary>
        /// Async method to execute sql transaction with queries and commands
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="transactionQueries"></param>
        /// <returns></returns>
        public async Task ExecuteTransactionAsync(string connectionString, List<TransactionQuery> transactionQueries)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlTransaction sqlTransaction = connection.BeginTransaction();

                try
                {
                    foreach (TransactionQuery transactionQuery in transactionQueries)
                    {
                        await connection.ExecuteAsync(transactionQuery.Query, transactionQuery.Parameters, transaction: sqlTransaction);
                    }

                    await sqlTransaction.CommitAsync();
                }
                catch (SqlException ex)
                {
                    _logger.LogException(nameof(SqlRepository), nameof(ExecuteTransactionAsync), JsonSerializer.Serialize(transactionQueries), ex.Message);

                    try
                    {
                        await sqlTransaction.RollbackAsync();
                    }
                    catch (Exception exRollBack)
                    {
                        _logger.LogException(nameof(SqlRepository), nameof(ExecuteTransactionAsync), JsonSerializer.Serialize(transactionQueries), exRollBack.Message);

                        throw;
                    }

                    throw;
                }
            }

            return;
        }
    }
}
using Dapper;
using Microsoft.Data.SqlClient;
using Serilog;
using $safeprojectname$.Modules.Sql.Interfaces.Repositories;
using $safeprojectname$.Modules.Sql.Models;
using $safeprojectname$.Infrastructures.ServiceExtensions.Attributes;

namespace $safeprojectname$.Modules.Sql.Repositories
{
    [ScopedLifetime]
    public class SqlRepository : ISqlRepository
    {
        public SqlRepository()
        {
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(string connectionString, string query, DynamicParameters? parameters = null)
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
                Log.Error($"{nameof(SqlRepository)} - {nameof(ExecuteQuery)} - {query}: {ex.Message}");

                throw;
            }

            return results;
        }

        public async Task<T?> ExecuteFirstQuery<T>(string connectionString, string query, DynamicParameters? parameters = null)
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
                Log.Error($"{nameof(SqlRepository)} - {nameof(ExecuteFirstQuery)} - {query}: {ex.Message}");

                throw;
            }

            return result;
        }

        public async Task<int> ExecuteCountQuery(string connectionString, string query, DynamicParameters? parameters = null)
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
                Log.Error($"{nameof(SqlRepository)} - {nameof(ExecuteCountQuery)} - {query}: {ex.Message}");

                throw;
            }

            return result;
        }

        public async Task ExecuteNonQuery(string connectionString, string query, DynamicParameters? parameters = null)
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
                Log.Error($"{nameof(SqlRepository)} - {nameof(ExecuteNonQuery)} - {query}: {ex.Message}");

                throw;
            }

            return;
        }

        public async Task ExecuteTransaction(string connectionString, List<TransactionQuery> transactionQueries)
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
                    Log.Error($"{nameof(SqlRepository)} - {nameof(ExecuteTransaction)}: {ex.Message}");

                    try
                    {
                        await sqlTransaction.RollbackAsync();
                    }
                    catch (Exception exRollBack)
                    {
                        Log.Error($"{nameof(SqlRepository)} - {nameof(ExecuteTransaction)} - RollbackException: {exRollBack.Message}");

                        throw;
                    }

                    throw;
                }
            }

            return;
        }
    }
}
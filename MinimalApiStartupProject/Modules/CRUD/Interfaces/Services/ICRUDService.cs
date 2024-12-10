using Dapper;
using MinimalApiStartupProject.Modules.Sql.Models;

namespace MinimalApiStartupProject.Modules.CRUD.Interfaces.Services
{
    public interface ICRUDService
    {
        /// <summary>
        /// Async method to get items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionStringName"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<T?> GetItemAsync<T>(string connectionStringName, string query, DynamicParameters? parameters = null);

        /// <summary>
        /// Async method to get single item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionStringName"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync<T>(string connectionStringName, string query, DynamicParameters? parameters = null);

        /// <summary>
        /// Async method to save items
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
        Task SaveItemAsync(string connectionStringName, string query, DynamicParameters parameters);

        /// <summary>
        /// Async method to save single item
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SaveItemsAsync(string connectionStringName, List<TransactionQuery> queries);
    }
}
namespace ConsoleApp1
{
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICosmosDataProvider
    {
        Task<object> ExecuteQueryAsync<T>(
            string collectionName,
            string query,
            RequestOptions requestOptions,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
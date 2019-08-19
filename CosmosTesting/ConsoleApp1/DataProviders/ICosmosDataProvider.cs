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
            string collectionUri,
            string sqlQuery,
            FeedOptions feedOptions,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
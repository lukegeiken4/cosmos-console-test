namespace ConsoleApp1.DataProviders
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using ConsoleApp1.Models;

    public class CosmosDataProvider
    {
        #region Private const members

        private const int DefaultRetryAttempts = 3;

        #endregion

        #region Private members

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDataProvider"/> class.
        /// </summary>
        public CosmosDataProvider()
        {
            this.SetupDocumentClient();
        }

        #endregion

        #region Public Properties

        public DocumentClient Client { get; set; }

        #endregion

        #region Public Methods

        public async Task<CosmosResponse<T>> ExecuteQueryAsync<T>(
            string collectionUri,
            string sqlQuery,
            FeedOptions feedOptions,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = this.Client.CreateDocumentQuery(collectionUri, sqlQuery, feedOptions);
            var docQuery = query.AsDocumentQuery();
            FeedResponse<T> queryResponse = null;

            do
            {
                queryResponse = await docQuery.ExecuteNextAsync<T>(cancellationToken);
            }
            while (docQuery.HasMoreResults && queryResponse.Count <= 0);

            var resp = new CosmosResponse<T>()
            {
                Items = queryResponse.ToList(),
                Count = queryResponse.Count,
                RuCharge = queryResponse.RequestCharge,
                ContToken = queryResponse.ResponseContinuation

            };

            return resp;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting /// resources.
        /// </summary>
        public void Dispose()
        {
            this.Client?.Dispose();
        }
        #endregion

        #region Private Methods

        private void SetupDocumentClient()
        {
                var uri = ConfigurationManager.AppSettings["CosmosURI"];
                var secret = ConfigurationManager.AppSettings["CosmosDBKeySecretUri"];
                this.Client = new DocumentClient(
                    new Uri(uri), secret);
        }
        #endregion
    }
}

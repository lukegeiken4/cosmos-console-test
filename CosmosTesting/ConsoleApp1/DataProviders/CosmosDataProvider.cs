namespace ConsoleApp1
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


    public class CosmosDataProvider : ICosmosDataProvider
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

        public async Task<object> ExecuteQueryAsync<T>(
            string collectionName,
            string query,
            RequestOptions requestOptions,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            //CosmosQueryResponse<T> response = new CosmosQueryResponse<T>();
            //Tuple<bool, CosmosQueryResponse<T>> result = Tuple.Create(false, response);

            //try
            //{
            //    Container container = this.Db.GetContainer(collectionName);

            //    List<T> results = new List<T>();
            //    FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(query, null, requestOptions);
            //    double totalRUs = 0;
            //    while (resultSetIterator.HasMoreResults)
            //    {
            //        var read = await resultSetIterator.ReadNextAsync();
            //        totalRUs += read.RequestCharge;
            //        results.AddRange(read);
            //    }

            //    return new CosmosQueryResponse<T>
            //    {
            //        Items = results,
            //        Token = null,
            //        Total = results.Count()
            //    };

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            return null;
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

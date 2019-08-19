namespace ConsoleApp1
{
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        private const string DatabaseName = "Community";
        private const string CollectionName = "Moderation";
        private static Uri collectionUri;

        static void Main(string[] args)
        {
            collectionUri = UriFactory.CreateDocumentCollectionUri(
                    DatabaseName,
                    CollectionName);

        }

        public async static void MainAsync()
        {
            var provider = new CosmosDataProvider();

            var query = "SELECT * from m WHERE m.state = \"Approved\"";
            var response = await provider.ExecuteQueryAsync<object>(
                collectionUri.ToString(),
                query,
                new FeedOptions());

            var test = 1;
        }
    }
}

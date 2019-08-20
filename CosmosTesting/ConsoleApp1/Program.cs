﻿namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ConsoleApp1.DataProviders;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

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


            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            var provider = new CosmosDataProvider();

            // https://docs.microsoft.com/en-us/azure/cosmos-db/profile-sql-api-query
            Console.WriteLine("[Running DESC query]");
            var descQuery = "SELECT * from m WHERE m.createdDateTimeUtc != null ORDER BY m.createdDateTimeUtc DESC";

            Console.WriteLine($"\"{descQuery}\"");
            var descResp = await provider.ExecuteQueryAsync<object>(
                collectionUri.ToString(),
                descQuery,
                new FeedOptions()
                {
                    EnableCrossPartitionQuery = true,
                    PopulateQueryMetrics = true
                });
            Console.WriteLine("RU: " + descResp.RuCharge);
            Console.WriteLine("*** Metrics ***");
            foreach (KeyValuePair<string, QueryMetrics> kvp in descResp.Metrics)
            {
                string partitionId = kvp.Key;
                QueryMetrics queryMetrics = kvp.Value;

                // Do whatever logging you need
                Console.WriteLine($"{partitionId}: {queryMetrics}");
            }

            Console.WriteLine("");
            Console.WriteLine("[Running ASC query]");
            var ascQuery = "SELECT * from m WHERE m.createdDateTimeUtc != null ORDER BY m.createdDateTimeUtc ASC";
            Console.WriteLine($"\"{ascQuery}\"");

            var ascResp = await provider.ExecuteQueryAsync<object>(
                collectionUri.ToString(),
                ascQuery,
                new FeedOptions()
                {
                    EnableCrossPartitionQuery = true,
                    PopulateQueryMetrics = true
                });
            Console.WriteLine("RU: " + ascResp.RuCharge);

            Console.WriteLine("\nPress ENTER to exit");
            Console.ReadLine();
        }
    }
}

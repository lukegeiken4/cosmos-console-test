namespace ConsoleApp1
{
    using System;
    using System.IO;
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
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                string filename = String.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                ostrm = new FileStream($"../../output/{filename}.txt", FileMode.CreateNew, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot create file for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);

            var provider = new CosmosDataProvider();
            var descQuery = "SELECT * from m WHERE m.createdDateTimeUtc != null ORDER BY m.createdDateTimeUtc DESC";

            ConsoleShowQueryHeader(descQuery);
            var descResp = await provider.ExecuteQueryAsync<object>(
                collectionUri.ToString(),
                descQuery,
                new FeedOptions()
                {
                    EnableCrossPartitionQuery = true,
                    PopulateQueryMetrics = true
                });

            ConsoleShowQueryRU(descResp.RuCharge);
            foreach (KeyValuePair<string, QueryMetrics> kvp in descResp.Metrics)
            {
                ConsoleShowQueryMetrics(kvp.Key, kvp.Value);
            }

            ConsoleNewLine();

            var ascQuery = "SELECT * from m WHERE m.createdDateTimeUtc != null ORDER BY m.createdDateTimeUtc ASC";
            ConsoleShowQueryHeader(ascQuery);

            var ascResp = await provider.ExecuteQueryAsync<object>(
                collectionUri.ToString(),
                ascQuery,
                new FeedOptions()
                {
                    EnableCrossPartitionQuery = true,
                    PopulateQueryMetrics = true
                });
            ConsoleShowQueryRU(ascResp.RuCharge);
            foreach (KeyValuePair<string, QueryMetrics> kvp in ascResp.Metrics)
            {
                ConsoleShowQueryMetrics(kvp.Key, kvp.Value);
            }

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
        }

        private static void ConsoleNewLine(int num = 1)
        {
            for (var i = 0; i < num; i++)
            {
                Console.WriteLine();
            }
        }

        private static void ConsoleShowQueryRU(double ru)
        {
            Console.WriteLine($"========== TOTAL RU CHARGE: {ru} ==========");
        }

        private static void ConsoleShowQueryMetrics(string paritionId, QueryMetrics qm)
        {
            ConsoleNewLine();
            Console.WriteLine($"============ PARITION ID: {paritionId} ============");
            ConsoleNewLine();
            Console.WriteLine(qm);
        }

        private static void ConsoleShowQueryHeader(string query)
        {
            Console.WriteLine("==========================================================================================================");
            Console.WriteLine($"============================================ RUN QUERY ===================================================");
            Console.WriteLine("==========================================================================================================");
            Console.WriteLine(query);
            Console.WriteLine("==========================================================================================================");
        }
    }
}

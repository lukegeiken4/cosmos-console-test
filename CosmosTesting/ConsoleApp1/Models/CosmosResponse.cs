namespace ConsoleApp1.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CosmosResponse<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
        public string ContToken { get; set; }
        public double RuCharge { get; set; }
    }
}

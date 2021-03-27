using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class PageRequest
    {
        public string PartitionKey {get; set;}
        public string RowKey {get; set;}
        public int Take {get; set;}
        public string? ContinuationToken { get; set; }
    }
}
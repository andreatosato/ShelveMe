using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class Page<T> where T : class
    {
        public IReadOnlyList<T> Values { get; init; }
        public string? ContinuationToken { get; set; }
    }
}

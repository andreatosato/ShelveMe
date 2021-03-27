using System.Collections.Generic;

namespace SharedModels
{
    public class Page<T> where T : class
    {
        public IReadOnlyList<T> Values { get; init; } = new List<T>();
        public string? ContinuationToken { get; set; }
    }
}

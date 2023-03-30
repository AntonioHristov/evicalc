using System.Collections.Generic;

namespace evicalc.models
{
    public class QueryResponse
    {
        public IEnumerable<Record> Operations { get; set; }
    }
}
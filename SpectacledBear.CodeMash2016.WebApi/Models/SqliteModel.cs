using System.Collections.Generic;

namespace SpectacledBear.CodeMash2016.WebApi.Models
{
    public class SqliteModel
    {
        public string SqliteVersion { get; private set; }
        public long QueryResponseTime { get; private set; }
        public IEnumerable<string> Tables { get; private set; }

        public SqliteModel(string version, long queryResponseTime, IEnumerable<string> tables)
        {
            SqliteVersion = version;
            QueryResponseTime = queryResponseTime;
            Tables = tables;
        }
    }
}

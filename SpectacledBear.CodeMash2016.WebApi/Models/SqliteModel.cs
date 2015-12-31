using System.Collections.Generic;

namespace SpectacledBear.CodeMash2016.WebApi.Models
{
    public class SqliteModel
    {
        public string Result { get; private set; }
        public long QueryResponseTime { get; private set; }
        public long TotalChanges { get; private set; }
        public string SqliteVersion { get; private set; }
        public IEnumerable<string> Tables { get; private set; }

        public SqliteModel(string version, long queryResponseTime, IEnumerable<string> tables, long changes, string result)
        {
            SqliteVersion = version;
            QueryResponseTime = queryResponseTime;
            Tables = tables;
            TotalChanges = changes;
            Result = result;
        }
    }
}

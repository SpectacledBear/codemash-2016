using System.Collections.Generic;

namespace SpectacledBear.CodeMash2016.WebApi.Models
{
    public class SqliteModel
    {
        public string Version { get; private set; }
        public IEnumerable<string> Tables { get; private set; }

        public SqliteModel(string version, IEnumerable<string> tables)
        {
            Version = version;
            Tables = tables;
        }
    }
}

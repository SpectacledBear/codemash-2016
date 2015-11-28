using System;
using System.Collections.Generic;
using System.Data;

using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    public class SqliteDataManager
    {
        private IDbConnection _sqliteConnection = PersistentSqliteDatabase.Connection();

        public SqliteModel GetModels()
        {
            string version = string.Empty;
            List<string> tables = new List<string>();

            string versionQuery = "SELECT SQLITE_VERSION()";
            using (IDbCommand command = _sqliteConnection.CreateCommand())
            {
                command.CommandText = versionQuery;
                version = Convert.ToString(command.ExecuteScalar());
            }

            string tableQuery = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
            using (IDbCommand command = _sqliteConnection.CreateCommand())
            {
                command.CommandText = tableQuery;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string table = reader.GetString(0);
                        tables.Add(table);
                    }
                }
            }

            SqliteModel model = new SqliteModel(version, tables);

            return model;
        }
    }
}

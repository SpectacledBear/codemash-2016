using System;
using System.Collections.Generic;
using System.Data.SQLite;

using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    public class SqliteDataManager
    {
        private SQLiteConnection _sqliteConnection = SqliteHandler.GetConnection();

        public SqliteModel GetModels()
        {
            string version = string.Empty;
            List<string> tables = new List<string>();

            string versionQuery = "SELECT SQLITE_VERSION()";
            using (SQLiteCommand command = new SQLiteCommand(versionQuery, _sqliteConnection))
            {
                version = Convert.ToString(command.ExecuteScalar());

            }

            string tableQuery = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
            using (SQLiteCommand command = new SQLiteCommand(tableQuery, _sqliteConnection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
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

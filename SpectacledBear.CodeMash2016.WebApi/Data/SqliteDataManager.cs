using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal class SqliteDataManager
    {
        private IDbConnection _sqliteConnection = PersistentSqliteDatabase.Connection;

        public SqliteModel GetMetrics()
        {
            string version = string.Empty;
            List<string> tables = new List<string>();

            Stopwatch stopwatch = new Stopwatch();
            string versionQuery = "SELECT SQLITE_VERSION()";
            using (IDbCommand command = _sqliteConnection.CreateCommand())
            {
                command.CommandText = versionQuery;
                stopwatch.Start();
                version = Convert.ToString(command.ExecuteScalar());
                stopwatch.Stop();
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

            SqliteModel model = new SqliteModel(version, stopwatch.ElapsedMilliseconds, tables);

            return model;
        }
    }
}

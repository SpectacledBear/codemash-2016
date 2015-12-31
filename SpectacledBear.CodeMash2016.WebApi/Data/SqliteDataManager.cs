using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal class SqliteDataManager : IDataManager<SqliteModel>
    {
        private IDbConnection _sqliteConnection = PersistentSqliteDatabase.Connection;

        public IEnumerable<SqliteModel> GetAll()
        {
            string version = string.Empty;
            List<string> tables = new List<string>();
            long totalChanges = -1;
            string result = "pass";

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

            string changesQuery = "SELECT TOTAL_CHANGES()";
            using (IDbCommand command = _sqliteConnection.CreateCommand())
            {
                command.CommandText = changesQuery;
                totalChanges = (long)command.ExecuteScalar();
            }

            if (string.IsNullOrEmpty(version) || tables.Count == 0)
            {
                result = "fail";
            }

            SqliteModel model = new SqliteModel(version, stopwatch.ElapsedMilliseconds, tables, totalChanges, result);

            return new SqliteModel[] { model };
        }

        public bool TryGet(SqliteModel reference, out long id)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(long id, out SqliteModel result)
        {
            throw new NotImplementedException();
        }

        public SqliteModel Insert(SqliteModel input)
        {
            throw new NotImplementedException();
        }

        public SqliteModel Update(SqliteModel input, long id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}

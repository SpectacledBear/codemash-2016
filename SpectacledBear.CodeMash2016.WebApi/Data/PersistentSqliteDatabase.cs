using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal static class PersistentSqliteDatabase
    {
        private const string CONNECTION_STRING = "Data Source=:memory:";
        private const string CREATE_SCRIPT = @"Data\CreateSchema.sql";
        private const string DATA_SCRIPT = @"Data\CreateHobbits.sql";

        private static SQLiteConnection _database = null;

        internal static void Initialize()
        {
            _database = new SQLiteConnection(CONNECTION_STRING);

            if(_database.State != ConnectionState.Open)
            {
                _database.Open();
            }

            PopulateData();
        }

        internal static IDbConnection Connection
        {
            get
            {
                if (_database == null)
                {
                    Initialize();
                }

                return _database;
            }
        }

        internal static void Terminate()
        {
            if (_database != null && _database.State == ConnectionState.Open)
            {
                _database.Close();
            }

            _database = null;
        }

        #region Private methods
        private static void PopulateData()
        {
            using (IDbCommand command = _database.CreateCommand())
            {
                string query = LoadQueryFromFile(CREATE_SCRIPT);
                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            string file = LoadQueryFromFile(DATA_SCRIPT);
            string[] queries = file.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string query in queries)
            {
                using (IDbCommand command = _database.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string LoadQueryFromFile(string filename)
        {
            string queryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            string query = File.ReadAllText(queryFilePath);

            return query;
        }
        #endregion
    }
}

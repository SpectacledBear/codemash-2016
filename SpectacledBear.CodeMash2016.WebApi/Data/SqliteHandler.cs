using System;
using System.Data.SQLite;
using System.IO;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal static class SqliteHandler
    {
        private const string CONNECTION_STRING = "Data Source=:memory:";
        private const string CREATE_SCRIPT = @"Data\CreateTable.sql";

        private static SQLiteConnection _database = null;

        internal static void CreateDatabase()
        {
            _database = new SQLiteConnection(CONNECTION_STRING);

            _database.Open();

            PopulateData();
        }

        internal static void DestroyDatabase()
        {
            if (_database != null && _database.State == System.Data.ConnectionState.Open)
            {
                _database.Close();
            }

            _database = null;
        }

        internal static SQLiteConnection GetConnection()
        {
            if(_database == null)
            {
                CreateDatabase();
            }

            return _database;
        }

        private static void PopulateData()
        {
            string query = LoadQueryFromFile(CREATE_SCRIPT);
            SQLiteCommand command = new SQLiteCommand(query, _database);

            int result = command.ExecuteNonQuery();
        }

        private static string LoadQueryFromFile(string filename)
        {
            string queryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            string query = File.ReadAllText(queryFilePath);

            return query;
        }
    }
}

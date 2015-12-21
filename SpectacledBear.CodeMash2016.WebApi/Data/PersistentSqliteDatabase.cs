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

        private static SQLiteConnection _connection = null;

        internal static void Initialize()
        {
            _connection = new SQLiteConnection(CONNECTION_STRING);

            if(_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            using (IDbCommand command = _connection.CreateCommand())
            {
                string query = LoadQueryFromFile(CREATE_SCRIPT);
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
        }

        internal static IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    Initialize();
                }

                return _connection;
            }
        }

        internal static void PopulateData()
        {
            string file = LoadQueryFromFile(DATA_SCRIPT);
            string[] queries = file.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string query in queries)
            {
                using (IDbCommand command = _connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void Terminate()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection = null;
        }

        #region Private methods
        private static string LoadQueryFromFile(string filename)
        {
            string queryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            string query = File.ReadAllText(queryFilePath);

            return query;
        }
        #endregion
    }
}

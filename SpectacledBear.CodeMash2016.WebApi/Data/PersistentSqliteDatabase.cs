﻿using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal static class PersistentSqliteDatabase
    {
        private const string CONNECTION_STRING = "Data Source=:memory:";
        private const string CREATE_SCRIPT = @"Data\CreateTable.sql";

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

        internal static IDbConnection Connection()
        {
            if (_database == null)
            {
                Initialize();
            }

            return _database;
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
        #endregion
    }
}

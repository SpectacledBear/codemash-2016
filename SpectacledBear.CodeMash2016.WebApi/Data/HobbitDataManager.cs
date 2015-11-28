using System;
using System.Collections.Generic;
using System.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal class HobbitDataManager
    {
        private IDbConnection _database = PersistentSqliteDatabase.Connection;

        public bool DeleteHobbit(long hobbitId)
        {
            string query = "DELETE FROM Hobbits WHERE rowid=@ID";

            using (IDbCommand command = _database.CreateCommand())
            {
                command.CommandText = query;
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@ID";
                parameter.Value = hobbitId;
                command.Parameters.Add(parameter);

                int result = command.ExecuteNonQuery();
                if (result > 0) return true;
            }

            return false;
        }

        public IEnumerable<Hobbit> GetAllHobbits()
        {
            string query = "SELECT Name, FamilyName, BirthYear, DeathYear, rowid FROM Hobbits ORDER BY rowid";
            List<Hobbit> hobbits = new List<Hobbit>();

            using (IDbCommand command = _database.CreateCommand())
            {
                command.CommandText = query;

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string family = reader.GetString(1);
                        int birth = reader.GetInt32(2);
                        int death = reader.GetInt32(3);
                        long id = reader.GetInt64(4);

                        Hobbit hobbit = new Hobbit(name, family, birth, death, id);
                        hobbits.Add(hobbit);
                    }
                }
            }

            return hobbits;
        }

        public Hobbit InsertHobbit(Hobbit hobbit)
        {
            if (hobbit.Name == null) return null;

            string columns = string.Empty;
            string values = string.Empty;

            columns += "Name";
            values += string.Format("'{0}'", hobbit.Name);

            if (!string.IsNullOrEmpty(hobbit.FamilyName))
            {
                columns += ",FamilyName";
                values += string.Format(",'{0}'", hobbit.FamilyName);
            }

            if (hobbit.BirthYear != default(int))
            {
                columns += ",BirthYear";
                values += string.Format(",'{0}'", hobbit.BirthYear);
            }

            if (hobbit.DeathYear != default(int))
            {
                columns += ",DeathYear";
                values += string.Format(",'{0}'", hobbit.DeathYear);
            }

            string query = string.Format("INSERT INTO Hobbits ({0}) VALUES ({1})", columns, values);

            int affectedRows;
            using (IDbCommand command = _database.CreateCommand())
            {
                command.CommandText = query;
                affectedRows = command.ExecuteNonQuery();
            }

            if (affectedRows != default(int))
            {
                long hobbitId;
                if (TryGetHobbitId(hobbit, out hobbitId))   // SQLite row IDs start at 1.
                {
                    Hobbit insertedHobbit = GetHobbit(hobbitId);
                    return insertedHobbit;
                }
            }

            return null;
        }

        public bool TryGetHobbit(long hobbitId, out Hobbit hobbit)
        {
            hobbit = null;

            hobbit = GetHobbit(hobbitId);

            if (hobbit != null)
            {
                return true;
            }

            return false;
        }

        public bool TryGetHobbitId(Hobbit hobbit, out long hobbitId)
        {
            hobbitId = default(long);

            if (string.IsNullOrEmpty(hobbit.Name)) return false;

            string query = string.Format(
                "SELECT rowid FROM Hobbits WHERE Name='{0}'",
                hobbit.Name);

            using (IDbCommand command = _database.CreateCommand())
            {
                command.CommandText = query;
                try
                {
                    hobbitId = (long)command.ExecuteScalar();
                    return true;
                }
                catch (NullReferenceException) { }
            }

            return false;
        }

        public Hobbit UpdateHobbit(Hobbit hobbit, long hobbitId)
        {
            if (hobbit.Name == null) return null;

            string query = string.Format("UPDATE Hobbits SET Name='{0}'", hobbit.Name);

            if (!string.IsNullOrEmpty(hobbit.FamilyName))
            {
                query += string.Format(",FamilyName='{0}'", hobbit.FamilyName);
            }

            if (hobbit.BirthYear != default(int))
            {
                query += string.Format(",BirthYear={0}", hobbit.BirthYear);
            }

            if (hobbit.DeathYear != default(int))
            {
                query += string.Format(",DeathYear={0}", hobbit.DeathYear);
            }

            query += string.Format(" WHERE rowid={0}", hobbitId);

            int affectedRows;
            using (IDbCommand command = _database.CreateCommand())
            {
                command.CommandText = query;
                affectedRows = command.ExecuteNonQuery();
            }

            if (affectedRows != default(int))
            {
                Hobbit updatedHobbit = GetHobbit(hobbitId);
                return updatedHobbit;
            }

            return null;
        }

        #region Private methods
        private Hobbit GetHobbit(long hobbitId)
        {
            string query = "SELECT Name, FamilyName, BirthYear, DeathYear, rowid FROM Hobbits WHERE rowid=@ID";

            using (IDbCommand command = _database.CreateCommand())
            {
                command.CommandText = query;
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@ID";
                parameter.Value = hobbitId;
                command.Parameters.Add(parameter);
                IDataReader reader = command.ExecuteReader();

                try
                {
                    reader.Read();

                    string name = reader.GetString(0);
                    string family = reader.GetString(1);
                    int birth = reader.GetInt32(2);
                    int death = reader.GetInt32(3);
                    long id = reader.GetInt64(4);

                    Hobbit hobbit = new Hobbit(name, family, birth, death, id);

                    return hobbit;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }
        #endregion
    }
}

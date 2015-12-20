using System;
using System.Collections.Generic;
using System.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal class HobbitDataManager : IDataManager<Hobbit>
    {
        private IDbConnection _database = PersistentSqliteDatabase.Connection;

        public bool Delete(long hobbitId)
        {
            using (IDbCommand command = _database.CreateCommand())
            {
                string query = "DELETE FROM Hobbits WHERE rowid=@ID";
                command.CommandText = query;
                command.Parameters.Add(CreateCommandParameter("@ID", hobbitId, command));

                int result = command.ExecuteNonQuery();
                if (result > 0) return true;
            }

            return false;
        }

        public IEnumerable<Hobbit> GetAll()
        {
            List<Hobbit> hobbits = new List<Hobbit>();

            using (IDbCommand command = _database.CreateCommand())
            {
                string query = "SELECT Name, FamilyName, BirthYear, DeathYear, rowid " +
                    "FROM Hobbits ORDER BY rowid";
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

        public Hobbit Insert(Hobbit hobbit)
        {
            if (hobbit.Name == null) return null;

            int affectedRows;

            using (IDbCommand command = _database.CreateCommand())
            {
                string columns = string.Empty;
                string values = string.Empty;
                List<IDbDataParameter> parameters = new List<IDbDataParameter>();

                columns += "Name";
                values += "@Name";
                parameters.Add(CreateCommandParameter("@Name", hobbit.Name, command));

                if (!string.IsNullOrEmpty(hobbit.FamilyName))
                {
                    columns += ",FamilyName";
                    values += ",@FamilyName";
                    parameters.Add(CreateCommandParameter("@FamilyName", hobbit.FamilyName, command));
                }

                if (hobbit.BirthYear != default(int))
                {
                    columns += ",BirthYear";
                    values += ",@BirthYear";
                    parameters.Add(CreateCommandParameter("@BirthYear", hobbit.BirthYear, command));
                }

                if (hobbit.DeathYear != default(int))
                {
                    columns += ",DeathYear";
                    values += ",@DeathYear";
                    parameters.Add(CreateCommandParameter("@DeathYear", hobbit.DeathYear, command));
                }

                string query = string.Format("INSERT INTO Hobbits ({0}) VALUES ({1})", columns, values);
                command.CommandText = query;
                parameters.ForEach(p => command.Parameters.Add(p));
                affectedRows = command.ExecuteNonQuery();
            }

            if (affectedRows != default(int))   // SQLite row IDs start at 1.
            {
                long hobbitId;
                if (TryGet(hobbit, out hobbitId))
                {
                    Hobbit insertedHobbit = GetHobbit(hobbitId);
                    return insertedHobbit;
                }
            }

            return null;
        }

        public bool TryGet(long hobbitId, out Hobbit hobbit)
        {
            hobbit = null;

            hobbit = GetHobbit(hobbitId);

            if (hobbit != null)
            {
                return true;
            }

            return false;
        }

        public bool TryGet(Hobbit hobbit, out long hobbitId)
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

        public Hobbit Update(Hobbit hobbit, long hobbitId)
        {
            if (hobbit.Name == null) return null;

            int affectedRows;
            using (IDbCommand command = _database.CreateCommand())
            {
                List<IDbDataParameter> parameters = new List<IDbDataParameter>();

                string query = "UPDATE Hobbits SET Name=@Name";
                parameters.Add(CreateCommandParameter("@Name", hobbit.Name, command));

                if (!string.IsNullOrEmpty(hobbit.FamilyName))
                {
                    query += ",FamilyName=@FamilyName";
                    parameters.Add(CreateCommandParameter("@FamilyName", hobbit.FamilyName, command));
                }

                if (hobbit.BirthYear != default(int))
                {
                    query += ",BirthYear=@BirthYear";
                    parameters.Add(CreateCommandParameter("@BirthYear", hobbit.BirthYear, command));
                }

                if (hobbit.DeathYear != default(int))
                {
                    query += ",DeathYear=@DeathYear";
                    parameters.Add(CreateCommandParameter("@DeathYear", hobbit.DeathYear, command));
                }

                query += " WHERE rowid=@RowID";
                parameters.Add(CreateCommandParameter("@RowID", hobbitId, command));

                command.CommandText = query;
                parameters.ForEach(p => command.Parameters.Add(p));
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
                command.Parameters.Add(CreateCommandParameter("@ID", hobbitId, command));
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

        private IDbDataParameter CreateCommandParameter(string parameterName, object parameterValue, IDbCommand command)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;

            return parameter;
        }
        #endregion
    }
}

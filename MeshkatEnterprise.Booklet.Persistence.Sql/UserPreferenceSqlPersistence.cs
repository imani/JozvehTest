using System;
using System.Data;
using System.Data.SqlClient;
using Compositional.Composer;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class UserPreferenceSqlPersistence : IUserPreferencePersistence
    {
        public void AddPreference(long personId, String stateJson)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_InsertUserPreference",
                (SqlConnection) persistenceContext.Connection, persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", personId);
                command.Parameters.AddWithValue("@stateJson", stateJson);

                command.ExecuteScalar();
            }
        }

        public void RemovePreference(long userId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_DeleteUserPreference",
                (SqlConnection) persistenceContext.Connection, persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.ExecuteScalar();
            }
        }

        public String GetPreference(long userId)
        {
            String ret = "";
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_GetUserPreference",
                (SqlConnection) persistenceContext.Connection, persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ret = (string) reader["PreferenceData"];
                        }
                    }
                }
            }
            return ret;
        }
    }
}
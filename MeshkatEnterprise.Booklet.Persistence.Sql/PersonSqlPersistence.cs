using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class PersonSqlPersistence : IPersonPersistence
    {
        public List<Person> GetPersons(string[] personIdentities, string firstNamePattern, string lastNamePattern)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_Person_GetPersons",
                (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                var table = new DataTable();
                table.Columns.Add("Element", typeof (string));
                if (personIdentities != null)
                {
                    foreach (string personIdentity in personIdentities)
                    {
                        table.Rows.Add(personIdentity);
                    }
                }
                command.Parameters.AddWithValue("@personIdentities", table);

                if (string.IsNullOrEmpty(firstNamePattern))
                {
                    command.Parameters.AddWithValue("@firstNamePattern", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@firstNamePattern", firstNamePattern);
                }

                if (string.IsNullOrEmpty(lastNamePattern))
                {
                    command.Parameters.AddWithValue("@lastNamePattern", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@lastNamePattern", lastNamePattern);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var persons = new List<Person>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var person = new Person();
                            person.PersonIdentity = reader["PersonIdentity"].ToString();
                            person.PersonName = reader["PersonName"].ToString();
                            person.PersonLastName = reader["PersonLastName"].ToString();

                            persons.Add(person);
                        }
                    }

                    return persons;
                }
            }
        }

        public List<Person> SearchPersons(string searchContent)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_Person_SearchPersons",
                (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (string.IsNullOrEmpty(searchContent))
                {
                    command.Parameters.AddWithValue("@searchContent", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@searchContent", searchContent);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var persons = new List<Person>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var person = new Person();
                            person.PersonIdentity = reader["PersonIdentity"].ToString();
                            person.PersonName = reader["PersonName"].ToString();
                            person.PersonLastName = reader["PersonLastName"].ToString();

                            persons.Add(person);
                        }
                    }

                    return persons;
                }
            }
        }

        public long? GetPersonId(string personIdentity)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_Person_GetPersonId",
                (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@personIdentity", personIdentity);

                return (long?) command.ExecuteScalar();
            }
        }
    }
}
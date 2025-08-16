using System;
using System.Collections.Generic;
using api.DTO;
using api.Models;
using Microsoft.Data.Sqlite;

namespace api.Storage;

public class SqLiteStorage(string connectionString) : IStorage
{
    public List<Contact> GetContacts()
    {
        var contacts = new List<Contact>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM contacts";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            contacts.Add(new Contact
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
            });
        }

        return contacts;
    }

    public Contact GetContactById(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM contacts WHERE id=@id";
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read()
            ? new Contact { Id = reader.GetInt32(0), Name = reader.GetString(1), Email = reader.GetString(2) }
            : null;
    }

    public bool Add(ContactDto dto)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO contacts(name,email) VALUES (@name,@email)";
        command.Parameters.AddWithValue("@name", dto.Name);
        command.Parameters.AddWithValue("@email", dto.Email);

        return command.ExecuteNonQuery() > 0;
    }

    public bool DeleteContactById(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM contacts WHERE id=@id";
        command.Parameters.AddWithValue("@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    public bool UpdateContact(ContactDto dto, int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
                              UPDATE contacts
                              SET 
                                  name = @name,
                                  email = @email
                              WHERE
                                  id = @id
                              """;
        command.Parameters.AddWithValue("@name", dto.Name);
        command.Parameters.AddWithValue("@email", dto.Email);
        command.Parameters.AddWithValue("@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    public (List<Contact>, int TotalCount) GetContacts(int pageNumber, int pageSize)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var listCommand = connection.CreateCommand();
        listCommand.CommandText = """
                                  SELECT * FROM contacts
                                  LIMIT @pageSize
                                  OFFSET @offset
                                  """;
        listCommand.Parameters.AddWithValue("@pageNumber", pageNumber);
        listCommand.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);

        var contacts = new List<Contact>();

        using var reader = listCommand.ExecuteReader();
        while (reader.Read())
        {
            contacts.Add(new Contact
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
            });
        }

        using var countCommand = connection.CreateCommand();
        countCommand.CommandText = "SELECT COUNT(*) FROM contacts";
        var totalCount = Convert.ToInt32(countCommand.ExecuteScalar());

        return (contacts, totalCount);
    }
}
using System;
using api.Models;
using Bogus;
using Bogus.Extensions;
using Microsoft.Data.Sqlite;

namespace api.Seed;

public class FakerInitializer(string connectionString) : IInitializer
{
    public void Initialize(int count)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var createCommand = connection.CreateCommand();
        createCommand.CommandText = """
                                    CREATE TABLE IF NOT EXISTS contacts(
                                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        name TEXT NOT NULL,
                                        email TEXT NOT NULL
                                    );
                                    """;

        createCommand.ExecuteNonQuery();

        using var countCommand = connection.CreateCommand();
        countCommand.CommandText = "SELECT COUNT(*) FROM contacts";

        var existingCount = (long)countCommand.ExecuteScalar()!;

        if (existingCount > 0) return;

        var contacts = new Faker<Contact>("ru")
                      .RuleFor(f => f.Name, c => c.Name.FullName())
                      .RuleFor(f => f.Email, (c, f) =>
                       {
                           var name = f.Name.Split(' ');
                           var firstName = name[0].Transliterate();
                           var lastName = name.Length > 1 ? name[1].Transliterate() : "";
                           string[] domains = ["yandex.ru", "mail.ru", "inbox.ru", "gmail.com"];
                           var email =
                               $"{firstName.ToLower()}.{lastName.ToLower()}@{domains[Random.Shared.Next(domains.Length)]}";
                           return email;
                       }).Generate(count);

        using var transaction = connection.BeginTransaction();

        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = "INSERT INTO contacts(name, email) VALUES (@name,@email)";

        foreach (var contact in contacts)
        {
            insertCommand.Parameters.Clear();
            insertCommand.Parameters.AddWithValue("@name", contact.Name);
            insertCommand.Parameters.AddWithValue("@email", contact.Email);

            insertCommand.ExecuteNonQuery();
        }

        transaction.Commit();
    }
}
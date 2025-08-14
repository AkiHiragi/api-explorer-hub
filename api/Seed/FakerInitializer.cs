using api.Models;
using Bogus;
using Microsoft.Data.Sqlite;

namespace api.Seed;

public class FakerInitializer(string connectionString) : IInitializer
{
    public void Initialize(int count)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var countCommand = connection.CreateCommand();
        countCommand.CommandText = "SELECT COUNT(*) FROM contacts";

        var existingCount = (long)countCommand.ExecuteScalar()!;

        if (existingCount > 0) return;

        var contacts = new Faker<Contact>()
                      .RuleFor(f => f.Name, c => c.Name.FullName())
                      .RuleFor(f => f.Email, (c, f) =>
                       {
                           var name = f.Name.Split(' ');
                           var firstName = name[0];
                           var lastName = name.Length > 1 ? name[1] : "";
                           var email = $"{firstName.ToLower()}.{lastName.ToLower()}@{c.Internet.DomainName()}";
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

            countCommand.ExecuteNonQuery();
        }
        
        transaction.Commit();
    }
}
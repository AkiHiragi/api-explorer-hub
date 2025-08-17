using System;
using System.Linq;
using api.DataContext;
using api.Models;
using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;

namespace api.Seed;

public class SqliteFakerInitializer(SqliteDbContext context) : IInitializer
{
    public void Initialize(int count)
    {
        context.Database.Migrate();

        if (context.Contacts.Any()) return;

        var contacts = new Faker<Contact>("ru")
                      .RuleFor(c => c.Name, f => f.Name.FullName())
                      .RuleFor(c => c.Email, (f, c) =>
                       {
                           var fullName = c.Name.Split(' ');
                           var firstName = fullName[0].ToLower().Transliterate();
                           var lastName = fullName.Length > 1 ? fullName[1].ToLower().Transliterate() : "";
                           string[] domains = ["yandex.ru", "mail.ru", "inbox.ru", "gmail.com"];
                           return $"{firstName}.{lastName}@{domains[Random.Shared.Next(domains.Length)]}";
                       }).Generate(count);
            
        context.Contacts.AddRange(contacts);
        context.SaveChanges();
    }
}
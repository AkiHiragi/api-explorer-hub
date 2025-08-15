using System.Collections.Generic;
using System.Linq;
using api.DTO;
using api.Models;
using Bogus;

namespace api.Storage;

public class InMemoryStorage : IStorage
{
    public InMemoryStorage()
    {
        Contacts = new Faker<Contact>("ru")
                  .RuleFor(c => c.Id, f => f.IndexFaker)
                  .RuleFor(c => c.Name, f => f.Name.FullName())
                  .RuleFor(c => c.Email, (f, c) =>
                   {
                       var nameParts = c.Name.Split(' ');
                       var firstName = nameParts[0].ToLower();
                       var lastName = nameParts[1].ToLower();

                       var domain = f.PickRandom("yandex.ru", "mail.ru", "gmail.com");
                       return f.Internet.Email(firstName: firstName, lastName: lastName, provider: domain);
                   })
                  .Generate(10);
    }

    private List<Contact> Contacts { get; set; }

    public List<Contact> GetContacts() => Contacts;

    public Contact GetContactById(int id) => Contacts.Find(contact => contact.Id == id);

    public bool Add(ContactDto dto)
    {
        if (Contacts.Any(c => c.Email == dto.Email)) return false;
        var contact = new Contact { Email = dto.Email, Name = dto.Name };
        Contacts.Add(contact);
        return true;
    }

    public bool DeleteContactById(int id)
    {
        var contact = GetContactById(id);
        if (contact == null) return false;

        Contacts.Remove(contact);
        return true;
    }

    public bool UpdateContact(ContactDto dto, int id)
    {
        var contact = GetContactById(id);
        if (contact == null) return false;

        contact.Name = dto.Name;
        contact.Email = dto.Email;
        return true;
    }
}
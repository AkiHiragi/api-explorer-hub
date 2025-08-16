using System.Collections.Generic;
using System.Linq;
using api.DataContext;
using api.DTO;
using api.Models;

namespace api.Storage;

public class SqliteEfStorage(SqliteDbContext context) : IStorage
{
    public List<Contact> GetContacts() => context.Contacts.ToList();

    public Contact GetContactById(int id) => context.Contacts.Find(id);

    public bool Add(ContactDto dto)
    {
        if (context.Contacts.Any(c => c.Email == dto.Email)) return false;

        context.Contacts.Add(new Contact { Name = dto.Name, Email = dto.Email });
        context.SaveChanges();
        return true;
    }

    public bool DeleteContactById(int id)
    {
        var contact = GetContactById(id);
        if (contact == null) return false;

        context.Contacts.Remove(contact);
        context.SaveChanges();

        return true;
    }

    public bool UpdateContact(ContactDto dto, int id)
    {
        var contact = GetContactById(id);
        if (contact == null) return false;

        contact.Name = dto.Name;
        contact.Email = dto.Email;
        context.SaveChanges();

        return true;
    }

    public (List<Contact>, int TotalCount) GetContacts(int pageNumber, int pageSize)
    {
        return (context.Contacts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(), context.Contacts.Count());
    }
}
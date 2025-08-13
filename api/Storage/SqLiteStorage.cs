using System.Collections.Generic;
using api.DTO;
using api.Models;
using Microsoft.Data.Sqlite;

namespace api.Storage;

public class SqLiteStorage:IStorage
{
    public List<Contact> GetContacts()
    {
        var contacts = new List<Contact>();

        using var connection = new SqliteConnection("Data Source=contacts.db");

        using var command = connection.CreateCommand(@"SELECT * FROM contacts");
        
        return contacts;
    }

    public Contact GetContactById(int id)
    {
        throw new System.NotImplementedException();
    }

    public bool Add(Contact contact)
    {
        throw new System.NotImplementedException();
    }

    public bool DeleteContactById(int id)
    {
        throw new System.NotImplementedException();
    }

    public bool UpdateContact(ContactDto dto, int id)
    {
        throw new System.NotImplementedException();
    }
}
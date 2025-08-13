using System.Collections.Generic;
using api.DTO;
using api.Models;

namespace api.Storage;

public interface IStorage
{
    List<Contact> GetContacts();
    Contact GetContactById(int id);
    bool Add(Contact contact);
    bool DeleteContactById(int id);
    bool UpdateContact(ContactDto dto, int id);
}
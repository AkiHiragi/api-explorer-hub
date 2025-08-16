using System;
using System.Collections.Generic;
using System.Linq;
using api.DTO;
using api.Models;
using api.Storage;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class ContactManagementController(IStorage storage) : BaseController
{
    [HttpGet("contacts")]
    public ActionResult<List<Contact>> GetContacts()
    {
        return Ok(storage.GetContacts());
    }

    [HttpGet("contacts/{id}")]
    public ActionResult<Contact> GetContactById(int id)
    {
        if (id < 0) return BadRequest("Invalid Id");
        var contact = storage.GetContactById(id);
        return contact != null
            ? Ok(contact)
            : NotFound("Контакт с указанным ID не найден");
    }

    [HttpDelete("contacts/{id}")]
    public IActionResult DeleteContact(int id)
    {
        return storage.DeleteContactById(id)
            ? NoContent()
            : BadRequest("Контакт с указанным ID не найден");
    }

    [HttpPut("contacts/{id}")]
    public IActionResult UpdateContact([FromBody] ContactDto dto, int id)
    {
        if (storage.UpdateContact(dto, id))
            return Ok(storage.GetContactById(id));

        return Conflict("Контакт с указанным ID не найден");
    }

    [HttpPost("contacts")]
    public IActionResult Create([FromBody] ContactDto dto)
    {
        if (storage.Add(dto))
            return Created(string.Empty, dto);
        return Conflict("Контакт с указанным ID уже существует");
    }

    [HttpGet("contacts/page")]
    public IActionResult GetContacts(int pageNumber = 1, int pageSize = 10)
    {
        var (data, totalCount) = storage.GetContacts(pageNumber, pageSize);
        return Ok(new
        {
            data,
            totalCount,
            pageNumber,
            pageSize
        });
    }
}
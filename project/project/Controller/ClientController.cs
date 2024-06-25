using Microsoft.AspNetCore.Mvc;
using project.DTOs;
using project.Models;
using project.Repositories;

namespace project.Controller;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IDbService _dbService;
    public ClientController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddNewClient(NewClientDTO newClientDto)
    {
        if (newClientDto.PESEL != null && newClientDto.KRS != null)
        {
            return BadRequest("Client can be either individual or company. Put null in proper fields.");
        }
        
        if (newClientDto.PESEL == null && newClientDto.KRS == null)
        {
            return BadRequest("Client must be either individual or company. ");
        }
        
        if (newClientDto.PESEL != null)
        {
            if (await _dbService.DoesIndividualClientExist(newClientDto.PESEL))
            {
                return BadRequest("Individual client with this Pesel already exist.");
            }
            
            if (newClientDto.LastName == null)
            {
                return BadRequest("Individual clients must have last name.");
            }
        }
        else
        {
            if (newClientDto.LastName != null)
            {
                return BadRequest("Company client can't have last name. Put null in last name field.");
            }
        }
        
        var client = new Client()
        {
            Name = newClientDto.Name,
            Address = newClientDto.Address,
            Mail = newClientDto.Mail,
            Phone = newClientDto.Phone,
            PESEL = newClientDto.PESEL,
            KRS = newClientDto.KRS,
            LastName = newClientDto.LastName
        };

        await _dbService.AddClient(client);

        return Created();
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveClient(int id)
    {
        var client = await _dbService.GetClientById(id);

        if (client == null)
        {
            return BadRequest("Client with given Id doesn't exist.");
        }

        if (!await _dbService.IsClientIndividual(id))
        {
            return BadRequest("You cant remove company clients.");
        }

        await _dbService.RemoveIndividualClient(id);

        return Ok();
    }
    //
    // [HttpPost]
    // public async Task<IActionResult> UpdateCLient(Client client)
    // {
    //     
    // }



}
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
    public async Task<IActionResult> AddNewClient(NewClientDTO newClient)
    {
        if (newClient.PESEL != null && newClient.KRS != null)
        {
            return BadRequest("Client can be either individual or company. Put null in proper fields.");
        }
        
        if (newClient.PESEL == null && newClient.KRS == null)
        {
            return BadRequest("Client must be either individual or company. ");
        }
        
        if (newClient.PESEL != null)
        {
            if (await _dbService.DoesIndividualClientExist(newClient.PESEL))
            {
                return BadRequest("Individual client with this Pesel already exist.");
            }
            
            if (newClient.LastName == null)
            {
                return BadRequest("Individual clients must have last name.");
            }
        }
        else
        {
            if (newClient.LastName != null)
            {
                return BadRequest("Company client can't have last name. Put null in last name field.");
            }
        }
        
        var client = new Client()
        {
            Name = newClient.Name,
            Address = newClient.Address,
            Mail = newClient.Mail,
            Phone = newClient.Phone,
            PESEL = newClient.PESEL,
            KRS = newClient.KRS,
            LastName = newClient.LastName
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

        return Ok("Client removed.");
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> UpdateCLient(int id, NewClientDTO newClient)
    {
        var client = await _dbService.GetClientById(id);

        if (client == null)
        {
            return BadRequest("Client with given Id doesn't exist.");
        }
        
        await _dbService.UpdateClientInfo(id, newClient);

        return Ok("Client updated.");
    }



}
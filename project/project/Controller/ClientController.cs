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

    [HttpPost]
    public async Task<IActionResult> AddNewClient(NewClientDTO newClientDto)
    {
        // Check if client doesnt already exist
        if (newClientDto.PESEL != null)
        {
            if (newClientDto.LastName is null)
            {
                return BadRequest("For individual clients last name must be provided.");
            }
            if (await _dbService.DoesIndividualClientExist(newClientDto.PESEL))
            {
                return NotFound("Individual client with this Pesel already exist.");
            }
        }
        else if (newClientDto.KRS != null)
        {
            if (await _dbService.DoesCompanyClientExist(newClientDto.KRS))
            {
                return NotFound("Company client with this Krs already exist.");
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

    [HttpPost]
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



}
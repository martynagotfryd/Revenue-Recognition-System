using Microsoft.AspNetCore.Mvc;
using project.DTOs;
using project.Models;
using project.Repositories;

namespace project.Controller;

[Route("api/[controller]")]
[ApiController]
public class ContractController : ControllerBase
{
    private readonly IDbService _dbService;
    public ContractController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewContract(NewContractDTO newContract)
    {
        var dif = newContract.End - newContract.Start;
        
        if (dif.Days < 3 || dif.Days > 30)
        {
            return BadRequest("The time range should be at least 3 days and at most 30 days.");
        }

        if (!await _dbService.DoesSoftwareVersionExists(newContract.IdSoftwareVersion))
        { 
            return BadRequest("Software Version with given Id doesn't exist.");
        }
        
        var client = await _dbService.GetClientById(newContract.IdClient);

        if (client == null)
        {
            return BadRequest("Client with given Id doesn't exist.");
        }
        
        if (await _dbService.DoesClientHasActiveContract(client))
        {
            return BadRequest("Client already have active contract.");
        }
        
        

        var price = 0;
        
        if (client.Contracts.Any())
        {
            //reduce price
        }

        
        var contract = new Contract()
        {
            
        };

        await _dbService.AddClient(contract);

        return Created();
    }

}
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

        var highestDiscount = await _dbService.GetHighestActiveDiscount(newContract.IdSoftwareVersion);

        var softwareVersion = await _dbService.GetSoftwareVersionById(newContract.IdSoftwareVersion);
        
        var price = softwareVersion.Software.Cost - softwareVersion.Software.Cost*(highestDiscount / 100);
        
        if (client.Contracts.Any())
        {
            price -= price * (5 / 100);
        }

        
        var contract = new Contract()
        {
            Start = newContract.Start,
            End = newContract.End,
            UpgradesEnd = newContract.Start.AddYears(1),
            Price = price,
            Signed = false,
            IdClient = newContract.IdClient,
            IdSoftwareVersion = newContract.IdSoftwareVersion
        };

        await _dbService.AddContract(contract);

        return Created();
    }

}
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

    [HttpPost("add")]
    public async Task<IActionResult> AddNewContract(NewContractDTO newContract)
    {
        // check time range
        var dif = newContract.End - newContract.Start;
        
        if (dif.Days < 3 || dif.Days > 30)
        {
            return BadRequest("The time range should be at least 3 days and at most 30 days.");
        }

        // check if softwareversion exist
        if (!await _dbService.DoesSoftwareVersionExists(newContract.IdSoftwareVersion))
        { 
            return BadRequest("Software Version with given Id doesn't exist.");
        }
        
        // check if client exist
        var client = await _dbService.GetClientById(newContract.IdClient);

        if (client == null)
        {
            return BadRequest("Client with given Id doesn't exist.");
        }
        
        // // check if client has active contact
        // if (await _dbService.DoesClientHasActiveContract(client, newContract.Start, newContract.End, newContract.IdSoftwareVersion))
        // {
        //     return BadRequest("Client already have active contract in those dates.");
        // }

        // get highest discount for given software if there is any
        var highestDiscount = await _dbService.GetHighestActiveDiscount(newContract.IdSoftwareVersion);

        var softwareVersion = await _dbService.GetSoftwareVersionById(newContract.IdSoftwareVersion);

        // calculate price
        double price;
        
        // check if there is discount on software
        if (highestDiscount != null)
        {
            price = softwareVersion.Software.Cost - softwareVersion.Software.Cost*(highestDiscount / 100);
        }
        else
        {
            price = softwareVersion.Software.Cost;
        }
        
        // check if there is discount for previous clients
        if (client.Contracts.Any())
        {
            var newPrice = price - price * 0.05;
            price = newPrice;
        }

        // add new contract
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

    [HttpPost("pay")]
    public async Task<IActionResult> SignContract(NewPaymentDTO paymentDto)
    {
        var contract = await _dbService.GetContractById(paymentDto.IdContract);

        if (contract == null)
        {
            return BadRequest("Contract with given id doesnt exist.");
        }
        
        if (contract.Signed)
        {
            return BadRequest("Contract is already paid.");
        }

        if (contract.End < DateTime.Now)
        {
            return BadRequest("Contract is expired.");
        }

        var payment = new Payment()
        {
            IdContract = paymentDto.IdContract,
            Value = paymentDto.Value
        };

        await _dbService.Pay(payment);
        await _dbService.SignContract(contract);
    
        return Ok("Payment made.");
    }
    
    [HttpPost("additional support")]
    public async Task<IActionResult> AddNewContract(int id, int years)
    {
        if (!await _dbService.DoesContractExist(id))
        {
            return BadRequest("Contract with given id doesnt exist.");
        }

        if (years != 1 && years != 2 && years != 3)
        {
            return BadRequest("You can only extend support by 1,2 or 3 years.");
        }

        await _dbService.AddAdditionalServices(id, years);

        return Ok("Additional support added to contract.");
    }
    
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveContract(int id)
    {
        if (!await _dbService.DoesContractExist(id))
        {
            return BadRequest("Contract with given id doesnt exist.");
        }

        await _dbService.RemoveContract(id);

        return Ok("Contract Deleted.");
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> CalculateRevenue(int? idSoftWare)
    {
        if (idSoftWare != null)
        {
            if (!await _dbService.DoesSoftwareExist(idSoftWare))
            {
                return BadRequest("Software with given id doesnt exist.");
            }
        }

        var revenue = await _dbService.GetRevenue(idSoftWare);

        return Ok(revenue);
    }
    
    [HttpGet("predicted revenue")]
    public async Task<IActionResult> CalculatePredictedRevenue(int? idSoftWare)
    {
        if (idSoftWare != null)
        {
            if (!await _dbService.DoesSoftwareExist(idSoftWare))
            {
                return BadRequest("Software with given id doesnt exist.");
            }
        }
        
        var revenue = await _dbService.GetPredictedRevenue(idSoftWare);

        return Ok(revenue);
    }
}
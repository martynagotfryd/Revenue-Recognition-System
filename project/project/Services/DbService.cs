using Microsoft.EntityFrameworkCore;
using project.Data;
using project.DTOs;
using project.Models;
using project.Repositories;

namespace project.Services;

public class DbService : IDbService
{
    private readonly DataBaseContext _context;
    private readonly CurrencyService _currencyService;

    public DbService(DataBaseContext context, CurrencyService currencyService)
    {
        _context = context;
        _currencyService = currencyService;

    }
    
    public async Task<bool> IsClientIndividual(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        return client != null && client.PESEL.HasValue;
    }

    public async Task<bool> DoesIndividualClientExist(int? pesel)
    {
        return await _context.Clients.AnyAsync(e => e.PESEL == pesel);

    }

    public async Task<bool> DoesCompanyClientExist(int? krs)
    {
        return await _context.Clients.AnyAsync(e => e.KRS == krs);
    }

    public async Task AddClient(Client client)
    {
        await _context.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveIndividualClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        
        if (client != null) client.IsDeleted = true;

        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }
    
    

    public async Task UpdateClientInfo(int id, NewClientDTO newClient)
    {
        var existingClient = await _context.Clients.FindAsync(id);
        
       
        existingClient.Name = newClient.Name; 
        existingClient.Address = newClient.Address;
        existingClient.Mail = newClient.Mail; 
        existingClient.Phone = newClient.Phone;
        
        if (existingClient.KRS == null) 
        { 
            existingClient.LastName = newClient.LastName;
        }

        _context.Clients.Update(existingClient);
        await _context.SaveChangesAsync();

    }

    public async Task<Client?> GetClientById(int id)
    {
        return await _context.Clients.Include(e => e.Contracts).FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<Client?> GetClientByPesel(int pesel)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsClientDeleted(int id)
    {
        throw new NotImplementedException();
    }

    public Task UnremoveIndividualClient(int id)
    {
        throw new NotImplementedException();
    }

    // Contract
    public async Task AddContract(Contract contract)
    {
        await _context.AddAsync(contract);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DoesClientHasActiveContract(Client client, DateTime start, DateTime end, int softwareVersion)
    {
        var activeContracts = client.Contracts.Any(c => !c.Signed && start >= c.Start && start <= c.End && c.IdSoftwareVersion == softwareVersion);
        return activeContracts;
    }
    

    public async Task<bool> DoesSoftwareVersionExists(int id)
    {
        return await _context.SoftwareVersions.AnyAsync(e => e.Id == id);

    }

    public async Task<SoftwareVersion?> GetSoftwareVersionById(int id)
    {
        return await _context.SoftwareVersions.FirstOrDefaultAsync(e => e.Id == id);

    }

    public async Task<double> GetHighestActiveDiscount(int id)
    {
        var softwareVersion = await _context.SoftwareVersions
            .Include(sv => sv.Software)
            .ThenInclude(s => s.Discounts)
            .FirstOrDefaultAsync(sv => sv.Id == id);

        var currentDate = DateTime.Now;
        Discount? highestDiscount = null;
        
        if (softwareVersion != null)
        {
            highestDiscount = softwareVersion.Software.Discounts
                .Where(d => d.Start <= currentDate && d.End >= currentDate)
                .OrderByDescending(d => d.Value)
                .FirstOrDefault();
        }

        if (highestDiscount != null)
        {
            return highestDiscount.Value;
        }
        else
        {
            return 0;
        }
    }

    public async Task SignContract(Contract contract)
    {
        double totalPaymentsValue = contract.Payments.Sum(p => p.Value);

        if (contract.Price <= totalPaymentsValue)
        {
            contract.Signed = true;

            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddAdditionalServices(int id, int years)
    {
        var contract = await _context.Contracts.FindAsync(id);

        if (contract != null)
        {
            contract.UpgradesEnd = contract.UpgradesEnd.AddYears(years);
            contract.Price += years * 1000;
        }

        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
        
    }

    public async Task<bool> DoesContractExist(int id)
    {
        return await _context.Contracts.AnyAsync(e => e.Id == id);
    }

    public async Task RemoveContract(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);

        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync();

    }

    public async Task Pay(Payment payment)
    {
        await _context.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task<Contract?> GetContractById(int id)
    {
        return await _context.Contracts.FirstOrDefaultAsync(e => e.Id == id);

    }

    public async Task<bool> DoesSoftwareExist(int? id)
    {
        return await _context.Softwares.AnyAsync(e => e.Id == id);

    }

    public async Task<double> GetRevenue(int? id, string? currency = null)
    {
        var contracts = _context.Contracts.AsQueryable();

        if (id != null)
        {
            contracts = contracts.Where(c => c.SoftwareVersion.IdSoftware == id);
        }

        double revenue = await contracts
            .SelectMany(c => c.Payments)
            .SumAsync(p => p.Value);

        if (currency != null && currency != "PLN")
        {
            decimal exchangeRate = await _currencyService.GetExchangeRateAsync(currency);
            revenue *= (double)exchangeRate;
        }

        return revenue;
    }

    public async Task<double> GetPredictedRevenue(int? id, string? currency = null)
    {
        var contracts = _context.Contracts.AsQueryable();

        if (id != null)
        {
            contracts = contracts.Where(c => c.SoftwareVersion.IdSoftware == id);
        }

        double revenue = await contracts.SumAsync(c => c.Price);

        if (currency != null && currency != "PLN")
        {
            decimal exchangeRate = await _currencyService.GetExchangeRateAsync(currency);
            revenue *= (double)exchangeRate;
        }

        return revenue;
    }
}
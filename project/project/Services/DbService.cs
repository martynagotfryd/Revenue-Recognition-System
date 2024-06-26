using Microsoft.EntityFrameworkCore;
using project.Data;
using project.DTOs;
using project.Models;

namespace project.Repositories;

public class DbService : IDbService
{
    private readonly DataBaseContext _context;
    public DbService(DataBaseContext context)
    {
        _context = context;
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
        return await _context.Clients.FirstOrDefaultAsync(e => e.Id == id);
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

    public async Task<bool> DoesClientHasActiveContract(Client client)
    {
        var currentDate = DateTime.Now;
        var activeContracts = client.Contracts.Any(c => c.Start <= currentDate || c.End >= currentDate);
        return activeContracts;
    }

    public Task<bool> DidClientHadAnyContract(Client client)
    {
        throw new NotImplementedException();
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
        
        return (double)highestDiscount?.Value;
    }
}
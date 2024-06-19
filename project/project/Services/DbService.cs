using Microsoft.EntityFrameworkCore;
using project.Data;
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

    public async Task UpdateClientInfo(int id, Client newClient)
    {
        var existingClient = await _context.Clients.FindAsync(id);
        
        if (existingClient != null)
        {
            existingClient.Name = newClient.Name;
            existingClient.Address = newClient.Address;
            existingClient.Mail = newClient.Mail;
            existingClient.Phone = newClient.Phone;
            
            if (existingClient.KRS == null)
            {
                existingClient.LastName = newClient.LastName;
            }
        }

        _context.Clients.Update(existingClient);
        await _context.SaveChangesAsync();

    }

    public async Task<Client?> GetClientById(int id)
    {
        return await _context.Clients.FirstOrDefaultAsync(e => e.Id == id);
    }
}
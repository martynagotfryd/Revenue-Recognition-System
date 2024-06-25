using project.DTOs;
using project.Models;

namespace project.Repositories;

public interface IDbService
{
    Task<bool> IsClientIndividual(int id);
    // use IsClientIndividual
    Task<bool> DoesIndividualClientExist(int? pesel);
    // use IsClientIndividual
    Task<bool> DoesCompanyClientExist(int? krs);
    Task AddClient(Client client);
    // use IsClientIndividual and do soft delete
    Task RemoveIndividualClient(int id);
    // except pesel and krs
    Task UpdateClientInfo(int id, NewClientDTO newClient);
    Task<Client?> GetClientById(int id);
    Task<Client?> GetClientByPesel(int pesel);
    Task<bool> IsClientDeleted(int id);
    Task UnremoveIndividualClient(int id);
}
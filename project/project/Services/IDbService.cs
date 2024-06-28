using project.DTOs;
using project.Models;

namespace project.Repositories;

public interface IDbService
{
    // Clients
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
    
    // Contracts
    Task AddContract(Contract contract);
    Task<bool> DoesClientHasActiveContract(Client client, DateTime strat, DateTime end, int softwareVersion);
    Task<bool> DoesSoftwareVersionExists(int id);
    Task<SoftwareVersion?> GetSoftwareVersionById(int id);
    Task<double> GetHighestActiveDiscount(int id);
    Task SignContract(Contract contract);
    Task AddAdditionalServices(int id, int years);
    Task<bool> DoesContractExist(int id);
    Task RemoveContract(int id);
    Task Pay(Payment payment);
    Task<Contract?> GetContractById(int id);
    Task<bool> DoesSoftwareExist(int? id);
    Task<double> GetRevenue(int? id, string? currency = null);
    Task<double> GetPredictedRevenue(int? id, string? currency = null);
    Task<bool> RegisterUser(string login, string password, string role);
    Task<Employee?> ValidateUser(string login, string password);
}
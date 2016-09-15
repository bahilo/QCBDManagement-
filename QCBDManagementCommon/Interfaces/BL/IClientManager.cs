using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IClientManager : IContactManager, IAddressManager
    {
        // Operations

        Task<List<Client>> InsertClient(List<Client> clientList);

        Task<List<Client>> UpdateClient(List<Client> clientList);

        Task<List<Client>> DeleteClient(List<Client> clientList);

        Task<List<Client>> GetClientData(int nbLine);

        Task<List<Client>> GetClientDataByBillList(List<Bill> billList);

        Task<List<Command>> GetQuoteCLient(int id);

        Task<List<Command>> GetCommandClient(int id);

        Task<List<Client>> searchClient(Client client, string filterOperator);

        Task<List<Client>> searchClientFromWebService(Client client, string filterOperator);

        Task<List<Client>> GetClientDataById(int id);

        Task<List<Client>> MoveClientAgentBySelection(List<Client> clientList, Agent toAgent);

    } /* end interface IClientManager */
}
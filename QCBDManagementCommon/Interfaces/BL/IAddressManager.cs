using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IAddressManager
    {
        Task<List<Address>> InsertAddress(List<Address> listAddress);

        Task<List<Address>> UpdateAddress(List<Address> listAddress);

        Task<List<Address>> DeleteAddress(List<Address> listAddress);

        Task<List<Address>> GetAddressData(int nbLine);

        Task<List<Address>> GetAddressDataByCommandList(List<Command> commandList);

        Task<List<Address>> GetAddressDataByClientList(List<Client> clientList);

        Task<List<Address>> searchAddress(Address Address, string filterOperator);

        Task<List<Address>> searchAddressFromWebService(Address Address, string filterOperator);

        Task<List<Address>> GetAddressDataById(int id);
    }
}

using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IContactManager
    {
        Task<List<Contact>> InsertContact(List<Contact> listContact);

        Task<List<Contact>> UpdateContact(List<Contact> listContact);

        Task<List<Contact>> DeleteContact(List<Contact> listContact);

        Task<List<Contact>> GetContactData(int nbLine);

        Task<List<Contact>> GetContactDataByClientList(List<Client> clientList);

        Task<List<Contact>> searchContact(Contact Contact, string filterOperator);

        Task<List<Contact>> searchContactFromWebService(Contact Contact, string filterOperator);

        Task<List<Contact>> GetContactDataById(int id);
    }
}

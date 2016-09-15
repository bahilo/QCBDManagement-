using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IProvider_itemManager
    {
        Task<List<Provider_item>> InsertProvider_item(List<Provider_item> listProvider_item);

        Task<List<Provider_item>> UpdateProvider_item(List<Provider_item> listProvider_item);

        Task<List<Provider_item>> DeleteProvider_item(List<Provider_item> listProvider_item);

        Task<List<Provider_item>> GetProvider_itemData(int nbLine);

        Task<List<Provider_item>> searchProvider_item(Provider_item Provider_item, string filterOperator);

        Task<List<Provider_item>> searchProvider_itemFromWebService(Provider_item Provider_item, string filterOperator);

        Task<List<Provider_item>> GetProvider_itemDataById(int id);

        Task<List<Provider_item>> GetProvider_itemDataByItemList(List<Item> itemList);
    }
}

// FILE: D:/Just IT Training/BillManagment/Classes//IItemManager.cs

// In this section you can add your own using directives
using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IItemManager : IProviderManager, IProvider_itemManager, IItem_deliveryManager, ITax_itemManager, IAuto_refManagement
    {
        // Operations

        Task<List<Item>> InsertItem(List<Item> itemList);

        Task<List<Item>> UpdateItem(List<Item> itemList);

        Task<List<Item>> DeleteItem(List<Item> itemList);

        Task<List<Item>> GetItemData(int nbLine);

        Task<List<Item>> GetItemDataByCommand_itemList(List<Command_item> command_itemList);

        Task<List<Item>> searchItem(Item item, string filterOperator);

        Task<List<Item>> searchItemFromWebService(Item item, string filterOperator);

        Task<List<Item>> GetItemDataById(int id);

    } /* end interface IItemManager */
}
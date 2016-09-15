using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ITax_itemManager
    {
        Task<List<Tax_item>> InsertTax_item(List<Tax_item> listTax_item);

        Task<List<Tax_item>> UpdateTax_item(List<Tax_item> listTax_item);

        Task<List<Tax_item>> DeleteTax_item(List<Tax_item> listTax_item);

        Task<List<Tax_item>> GetTax_itemData(int nbLine);

        Task<List<Tax_item>> GetTax_itemDataByItemList(List<Item> itemList);

        Task<List<Tax_item>> searchTax_item(Tax_item Tax_item, string filterOperator);

        Task<List<Tax_item>> GetTax_itemDataById(int id);
    }
}

using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IItem_deliveryManager
    {
        Task<List<Item_delivery>> InsertItem_delivery(List<Item_delivery> listItem_delivery);

        Task<List<Item_delivery>> UpdateItem_delivery(List<Item_delivery> listItem_delivery);

        Task<List<Item_delivery>> DeleteItem_delivery(List<Item_delivery> listItem_delivery);

        Task<List<Item_delivery>> GetItem_deliveryData(int nbLine);

        Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList);

        Task<List<Item_delivery>> searchItem_delivery(Item_delivery Item_delivery, string filterOperator);

        Task<List<Item_delivery>> searchItem_deliveryFromWebService(Item_delivery Item_delivery, string filterOperator);

        Task<List<Item_delivery>> GetItem_deliveryDataById(int id);
    }
}

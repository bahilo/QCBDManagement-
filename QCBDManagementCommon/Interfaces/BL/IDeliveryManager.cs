using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IDeliveryManager
    {
        Task<List<Delivery>> InsertDelivery(List<Delivery> listDelivery);

        Task<List<Delivery>> UpdateDelivery(List<Delivery> listDelivery);

        Task<List<Delivery>> DeleteDelivery(List<Delivery> listDelivery);

        Task<List<Delivery>> GetDeliveryData(int nbLine);

        Task<List<Delivery>> GetDeliveryDataByCommandList(List<Command> commandList);

        Task<List<Delivery>> searchDelivery(Delivery Delivery, string filterOperator);

        Task<List<Delivery>> searchDeliveryFromWebService(Delivery Delivery, string filterOperator);

        Task<List<Delivery>> GetDeliveryDataById(int id);
    }
}

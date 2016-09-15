using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ITaxManager
    {
        Task<List<Tax>> InsertTax(List<Tax> listTax);

        Task<List<Tax>> UpdateTax(List<Tax> listTax);

        Task<List<Tax>> DeleteTax(List<Tax> listTax);

        Task<List<Tax>> GetTaxData(int nbLine);

        Task<List<Tax>> searchTax(Tax Tax, string filterOperator);

        Task<List<Tax>> GetTaxDataById(int id);
    }
}

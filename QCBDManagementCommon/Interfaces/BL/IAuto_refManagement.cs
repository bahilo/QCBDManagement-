using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IAuto_refManagement
    {
        Task<List<Auto_ref>> InsertAuto_ref(List<Auto_ref> listAuto_ref);

        Task<List<Auto_ref>> UpdateAuto_ref(List<Auto_ref> listAuto_ref);

        Task<List<Auto_ref>> DeleteAuto_ref(List<Auto_ref> listAuto_ref);

        Task<List<Auto_ref>> GetAuto_refData(int nbLine);

        Task<List<Auto_ref>> searchAuto_ref(Auto_ref Auto_ref, string filterOperator);

        Task<List<Auto_ref>> GetAuto_refDataById(int id);
    }
}

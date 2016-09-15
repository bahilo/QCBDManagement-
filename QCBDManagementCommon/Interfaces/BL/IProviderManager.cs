using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IProviderManager
    {
        Task<List<Provider>> InsertProvider(List<Provider> listProvider);

        Task<List<Provider>> UpdateProvider(List<Provider> listProvider);

        Task<List<Provider>> DeleteProvider(List<Provider> listProvider);

        Task<List<Provider>> GetProviderData(int nbLine);

        Task<List<Provider>> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList);

        Task<List<Provider>> searchProvider(Provider Provider, string filterOperator);
        
        Task<List<Provider>> searchProviderFromWebService(Provider Provider, string filterOperator);

        Task<List<Provider>> GetProviderDataById(int id);
    }
}

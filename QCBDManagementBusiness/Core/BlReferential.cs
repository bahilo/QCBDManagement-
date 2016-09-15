using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementBusiness.Core
{
    public class BlReferential : IReferentialManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC;

        public BlReferential(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public async Task<List<Infos>> InsertInfos(List<Infos> infosList)
        {
            if (infosList == null || infosList.Count == 0)
                return new List<Infos>();

            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.InsertInfos(infosList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Infos>> DeleteInfos(List<Infos> infosList)
        {
            if (infosList == null || infosList.Count == 0)
                return new List<Infos>();

            if (infosList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting Infos(count = " + infosList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.DeleteInfos(infosList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Infos>> UpdateInfos(List<Infos> infosList)
        {
            if (infosList == null || infosList.Count == 0)
                return new List<Infos>();

            if (infosList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating Infos(count = " + infosList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.UpdateInfos(infosList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Infos>> GetInfosData(int nbLine)
        {
            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.GetInfosData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Infos>> GetInfosDataById(int id)
        {
            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.GetInfosDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Infos>> searchInfosFromWebService(Infos infos, string filterOperator)
        {
            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.searchInfosFromWebService(infos, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Infos>> searchInfos(Infos Infos, string filterOperator)
        {
            List<Infos> result = new List<Infos>();
            try
            {
                result = await DAC.DALReferential.searchInfos(Infos, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }
    } /* end class BlReferential */
}

using QCBDManagementCommon.Entities;
using System;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface IDataAccessManager: IDisposable
    {
        IAgentManager DALAgent { get; set; }        
        IStatisticManager DALStatistic { get; set; }        
        ICommandManager DALCommand { get; set; }        
        IClientManager DALClient { get; set; }        
        IItemManager DALItem { get; set; }
        IReferentialManager DALReferential { get; set; }
        ISecurityManager DALSecurity { get; set; }

        void SetUserCredential(Agent authenticatedUser, bool isNewAgentAuthentication = false);
    } /* end interface IDataAccessManager */
}
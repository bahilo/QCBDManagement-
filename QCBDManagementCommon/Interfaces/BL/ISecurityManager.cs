using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ISecurityManager : IActionRecordManager, IRoleManager, ISecurityActionManager, IAgent_roleManager, IRole_actionManager, IPrivilegeManager 
    {
        // Operations

        Agent GetAuthenticatedUser();

        Task<Agent> AuthenticateUser(string username, string password, bool isClearPassword = true);
             
        List<Agent> DisableAgent(List<Agent> listAgent);

        List<Agent> EnableAgent(List<Agent> listAgent);

        Agent UseAgent(Agent inAgent);

        string CalculateHash(string clearTextPassword);

    } /* end interface Isecurity */
}
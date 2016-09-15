// FILE: D:/Just IT Training/BillManagment/Classes//IActionManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000B88 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000B88 end

/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IActionManager
    {

        IAgentManager BlAgent { get; set; }
        ICommandManager BlCommand { get; set; }
        IClientManager BlClient { get; set; }
        IItemManager BlItem { get; set; }
        IReferentialManager BlReferential { get; set; }
        ISecurityManager BlSecurity { get; set; }
        IDisplayManager BlDisplay { get; set; }
        IStatisticManager DALStatisitc { get; set; }
    } /* end interface IActionManager */
}

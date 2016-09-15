// FILE: D:/Just IT Training/BillManagment/Classes//BusinessLogic.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000B5D begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000B5D end

using QCBDManagementCommon.Interfaces.BL;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementBusiness
{
    public class BusinessLogic : IActionManager
    {
        // Attributes

        public IAgentManager BlAgent { get; set; }
        public ICommandManager BlCommand { get; set; }
        public IClientManager BlClient { get; set; }
        public IItemManager BlItem { get; set; }
        public IReferentialManager BlReferential { get; set; }
        public ISecurityManager BlSecurity { get; set; }
        public IDisplayManager BlDisplay { get; set; }
        public IStatisticManager DALStatisitc { get; set; }

        public BusinessLogic(
                            IAgentManager inBlAgent,
                            IClientManager inBlClient,
                            IItemManager inBlItem,
                            ICommandManager inBlCommande,
                            ISecurityManager inBlSecurity,
                            IDisplayManager inBlDisplay,
                            IStatisticManager inDALStatisitc,
                            IReferentialManager inBlReferential)
                            
        {
            this.BlAgent = inBlAgent;
            this.BlClient = inBlClient;
            this.BlCommand = inBlCommande;
            this.BlDisplay = inBlDisplay;
            this.BlItem = inBlItem;
            this.BlReferential = inBlReferential;
            this.BlSecurity = inBlSecurity;
            this.DALStatisitc = inDALStatisitc;
        }
        
    } /* end class BusinessLogic */
}
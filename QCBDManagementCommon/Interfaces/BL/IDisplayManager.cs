// FILE: D:/Just IT Training/BillManagment/Classes//IDisplayManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000ED0 begin
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000ED0 end

/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IDisplayManager
    {
        // Operations

        void ClientView();

        void ClientDetailsView();

        void CatalogSearchView();

        void CatalogCreateView();

        void WelcomePageView();

        void QuoteView();

        void QuoteDetailsView();

        void CommandView();

        void CommandDetailsView();

        void StatisticView();

        void AgentView();

        void NotificationView();

        void ReferentialView();
    } /* end interface IDisplayManager */
}
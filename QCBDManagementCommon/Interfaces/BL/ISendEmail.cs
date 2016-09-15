// FILE: D:/Just IT Training/BillManagment/Classes//ISendEmail.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000C7D begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000C7D end

using QCBDManagementCommon.Entities;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ISendEmail
    {

        Client Client { get; set; }

        Agent Agent { get; set; }

        Bill Bill { get; set; }

        string Message { get; set; }

        // Operations

        void Send();
    } /* end interface ISendEmail */
}
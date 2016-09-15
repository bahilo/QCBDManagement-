// FILE: D:/Just IT Training/BillManagment/Classes//Delivery.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D17 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D17 end

using System;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Delivery
    {
        // Attributes

        public int ID {get; set;}

        public int CommandId {get; set;}

        public int BillId {get; set;}

        public int Package {get; set;}

        public DateTime Date {get; set;}

        public string Status {get; set;}
    } /* end class Delivery */
}
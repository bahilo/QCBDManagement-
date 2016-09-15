// FILE: D:/Just IT Training/BillManagment/Classes//Command_Item.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D27 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D27 end

/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Command_item
    {
        // Attributes

        public int ID {get; set;}

        public int CommandId {get; set;}

        public string Item_ref {get; set;}

        public int ItemId { get; set; }

        public int Quantity {get; set;}

        public int Quantity_delivery {get; set;}

        public int Quantity_current {get; set;}

        public decimal Price {get; set;}

        public decimal Price_purchase {get; set;}

        public string Comment_Purchase_Price {get; set;}

        public int Order {get; set;}
    } /* end class Command_Item */
}
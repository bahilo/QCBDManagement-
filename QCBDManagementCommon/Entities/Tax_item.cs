// FILE: D:/Just IT Training/BillManagment/Classes//Tax_item.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000DEE begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000DEE end

/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Tax_item
    {
        // Attributes

        public int ID {get; set;}

        public int itemId {get; set;}

        public string Item_ref {get; set;}

        public double Tax_value {get; set;}

        public int TaxId {get; set;}

        public string Tax_type {get; set;}
    } /* end class Tax_item */
}
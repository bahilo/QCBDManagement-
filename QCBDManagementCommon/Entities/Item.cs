// FILE: D:/Just IT Training/BillManagment/Classes//Item.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D79 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D79 end

/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Item
    {
        // Attributes

        public int ID {get; set;}

        public string Ref {get; set;}

        public string Name {get; set;}

        public string Type {get; set;}

        public string Type_sub {get; set;}

        public decimal Price_purchase {get; set;}

        public decimal Price_sell {get; set;}

        public int Source {get; set;}

        public int Number_of_sale { get; set;}

        public string Comment {get; set;}

        public int Option { get; set; }

        public string Erasable {get; set;}
    } /* end class Item */
}
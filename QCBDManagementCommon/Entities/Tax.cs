// FILE: D:/Just IT Training/BillManagment/Classes//Tax.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000DC8 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000DC8 end

using System;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Tax
    {
        // Attributes

        public int ID {get; set;}

        public string Type {get; set;}

        public DateTime Date_insert {get; set;}

        public double Value {get; set;}

        public string Comment {get; set;}

        public int Tax_current {get; set;}
    } /* end class Tax */
}
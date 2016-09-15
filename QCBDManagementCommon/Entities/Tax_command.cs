// FILE: D:/Just IT Training/BillManagment/Classes//Tax_command.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000DDB begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000DDB end

using System;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Tax_command
    {
        // Attributes

        public int ID {get; set;}

        public int CommandId {get; set;}

        public int TaxId {get; set;}

        public DateTime Date_insert {get; set;}

        public double Tax_value {get; set;}

        public string Target {get; set;}
    } /* end class Tax_command */
}
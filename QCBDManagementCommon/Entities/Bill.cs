// FILE: D:/Just IT Training/BillManagment/Classes//Bill.cs

// In this section you can add your own using directives
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000962 begin
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000962 end

using System;
using System.Collections;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Bill
    {
        // Attributes

        public int ID {get; set;}

        public int ClientId {get; set;}

        public int CommandId {get; set;}

        public string PayMod {get; set;}

        public decimal Pay {get; set;}

        public decimal PayReceived {get; set;}

        public string Comment1 {get; set;}

        public string Comment2 {get; set;}

        public DateTime Date {get; set;}

        public DateTime DateLimit {get; set;}

        public DateTime PayDate {get; set;}
        
    }
}
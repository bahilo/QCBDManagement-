// FILE: D:/Just IT Training/BillManagment/Classes//Command.cs

// In this section you can add your own using directives
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:00000000000009C1 begin
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:00000000000009C1 end

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
    public class Command
    {
        // Attributes

        public int ID {get; set;}

        public int AgentId {get; set;}

        public int ClientId {get; set;}

        public string Comment1 {get; set;}

        public string Comment2 {get; set;}

        public string Comment3 {get; set;}

        public int BillAddress {get; set;}

        public int DeliveryAddress {get; set;}

        public string Status {get; set;}

        public DateTime Date {get; set;}

        public double Tax {get; set;}
        
        
    } /* end class Command */
}
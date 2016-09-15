// FILE: D:/Just IT Training/BillManagment/Classes//Notification.cs

// In this section you can add your own using directives
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000A0B begin
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000A0B end

using System;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Notification
    {
        // Attributes

        public int ID {get; set;}

        public int BillId {get; set;}

        public string Reminder1 {get; set;}

        public string Reminder2 {get; set;}

        public DateTime Date {get; set;}
    } /* end class Notification */
}
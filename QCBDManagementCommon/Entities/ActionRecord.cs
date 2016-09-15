// FILE: D:/Just IT Training/BillManagment/Classes//ActionRecord.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000BA0 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000BA0 end

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
    public class ActionRecord
    {
        // Attributes

        public int ID {get; set;}

        public int AgentId {get; set;}

        public DateTime Date {get; set;}

        public string Action {get; set;}

        public string TargetName {get; set;}

        public int TargetId {get; set;}

        // Associations
        
    } /* end class ActionRecord */
}
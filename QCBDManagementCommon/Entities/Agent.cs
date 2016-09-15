// FILE: D:/Just IT Training/BillManagment/Classes//Agent.cs

// In this section you can add your own using directives
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:00000000000009E9 begin
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:00000000000009E9 end

using System.Collections;
using System.Collections.Generic;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Entities
{
    public class Agent
    {
        // Attributes

        public int ID {get; set;}

        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string Phone {get; set;}

        public string Fax {get; set;}

        public string Email {get; set;}

        public string Login {get; set;}

        public string HashedPassword { get; set;}

        public string Admin { get; set; }

        public string Status {get; set;}

        public int ListSize {get; set;}

        public List<Role> RoleList { get; set; }

    } /* end class Agent */
}
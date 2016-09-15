using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Enum
{
    public enum EStatusCommand
    {
        Quote,                  //devis
        Pre_Command,             //preco
        Command,                //command
        Command_Close,           // close
        Pre_Credit,              // preavoir
        Credit,                 // avoir
        Credit_CLose,            // a_close
        Pre_Client_Validation,    // revalid
        Bill_Command,            // facture
        Bill_Credit,              // a_facture
        Billed,                    //f
        Not_Billed,               //nf,
        Valid,                    // Revalid
        Detail,                   // whether display of command detail
        Null,                      // No match found in this Enum
        Proforma

    }
}

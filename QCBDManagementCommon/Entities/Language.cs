using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Entities
{
    public class Language
    {
        public int ID { get; set; }

        public string Lang_table { get; set; }

        public string Table_column { get; set; }

        public string ColumnId { get; set; }

        public string Lang { get; set; }

        public string Content { get; set; }

        public string CultureInfo_name { get; set; }

        public string CultureInfo_fullName { get; set; }
    }
}

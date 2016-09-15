using System;


namespace QCBDManagementCommon.Entities
{
    public class Statistic
    {
        // Attributes

        public int ID {get; set;}

        public DateTime Bill_date {get; set;}

        public int BillId {get; set;}

        public string Company {get; set;}

        public decimal Price_purchase_total {get; set;}

        public decimal Total {get; set;}

        public decimal Total_tax_included {get; set;}

        public double Income_percent {get; set;}

        public decimal Income {get; set;}

        public decimal Pay_received {get; set;}

        public DateTime Date_limit {get; set;}

        public DateTime Pay_date {get; set;}

        public double Tax_value {get; set; }

        public int Option { get; set; }
    } /* end class Statistic */
}
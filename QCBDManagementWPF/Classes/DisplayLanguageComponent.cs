using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Classes
{
    

    public class DisplayLanguageComponent
    {
        public List<string> HomeComponentIDList { get; set; }
        public List<string> ClientDetailComponentIDList { get; set; }
        public List<string> CatalogComponentIDList { get; set; }
        public List<string> CatalogItemComponentIDList { get; set; }
        public List<string> QuoteComponentIDList { get; set; }
        public List<string> CommandComponentIDList { get; set; }
        public List<string> CommandDetailComponentIDList { get; set; }
        public List<string> StatisticComponentIDList { get; set; }
        public List<string> AgentComponentIDList { get; set; }
        public List<string> AgentDetailComponentIDList { get; set; }
        public List<string> NotificationComponentIDList { get; set; }
        public List<string> OptionComponentIDList { get; set; }
        public List<string> OptionAgentCredentailComponentIDList { get; set; }
        public List<string> OptionDataAndDisplayComponentIDList { get; set; }
        public List<string> OptionEmailComponentIDList { get; set; }
        public List<string> OptionMonitoringComponentIDList { get; set; }
        public List<string> ClientComponentIDList { get; set; }
        public List<string> MainComponentIDList { get; set; }
        public List<string> DeliveryPdfComponentIDList { get; internal set; }
        public List<string> QuotePdfComponentIDList { get; internal set; }
        public List<string> BillPdfComponentIDList { get; internal set; }

        public DisplayLanguageComponent()
        {
            init();
        }

        private void init()
        {
            HomeComponentIDList = new List<string>
            {
                "TITLE",
            };

            MainComponentIDList = new List<string>
            {
                "TITLE",
                "HOME",
                "CLIENT",
                "CATALOG",
                "QUOTE",
                "COMMAND",
                "AGENT",
                "NOTIFICATION",
                "STATISTIC",
                "OPTION"
            };

            ClientComponentIDList = new List<string>
            {
                "TITLE",
                "CLIENT_ID",
                "CONTACT_NAME",
                "COMPANY",
                "CLIENT",
                "PROSPECT",
                "DEEP_SEARCH",
                "FIRSRT_NAME",
                "LAST_NAME",
                "STATUS",
                "AGENT",
                "MOVE",
                "DETAIL",
                "SEARCH_RESULT",
                "NEW_CLIENT_PROSPECT",
                "NEW_CONTACT",
                "NEW_DELIVERY_BILL_ADDRESS",
                "SEARCH_CLIENT_PROSPECT",
                "SELECT_CLIENT_FOR_QUOTE",
                "SEE_CLIENT_COMAND",
                "SEE_CLIENT_QUOTE",
                "SETUPS",
                "UTILITIES",
                "MOVE_CHECKED_CLIENT_TO",
            };

            ClientDetailComponentIDList = new List<string>
            {
                "TITLE",
                "SEARCH",
                "COMPANY_NAME",
                "CLIENT_DETAIL",
                "CLIENT",
                "PROSPECT",
                "CLIENT_ID",
                "FIRSRT_NAME",
                "LAST_NAME",
                "STATUS",
                "MAX_CREDIT",
                "RIB",
                "LIMIT_PAYMENT",
                "MAIN_CONTACT",
                "ADDRESS",
                "PHONE",
                "FAX",
                "EMAIL",
                "POSITION",
                "CELL_PHONE",
                "COMMENT",
                "ADDRESS_TYPE",
                "BILL_ADDRESS",
                "DELIVERY_ADDRESS",
                "NAME",
                "CITY",
                "POST_CODE",
                "COUNTRY",
                "ADDRESS_COMMENT",
            };

            CatalogComponentIDList = new List<string>
            {
                "TITLE",
                "BRAND",
                "AND_OR_IN_FAMIY",
                "REFERENCE",
                "ITEM_NAME",
                "MATCH_EXACTLY",
                "DEEP_SEARCH",
                "ITEM",
                "QUANTITY",
                "TOTAL_SALE_PRICE",
                "TOTAL_PURCHASE_PRICE",
                "TOTAL_SALE_PRICE",
                "NAME",
                "BRAND",
                "FAMILY",
                "PROVIDER",
                "PURCHASE_PRICE",
                "SALE_PRICE",
                "SELECT_TO_CART",
                "DETAIL",
            };

            CatalogItemComponentIDList = new List<string>
            {
                "TITLE",
                "ITEM_BRAND",
                "ITEM_REFERENCE",
                "NAME",
                "REFERENCE",
                "MATCH_EXACTLY",
                "DEEP_SEARCH",
                "BRAND",
                "FAMILY",
                "NEW_BRAND",
                "NEW_FAMILY",
                "PROVIDER",
                "NEW_PROVIDER",
                "PURCHASE_PRICE",
                "SALE_PRICE",
            };

            QuoteComponentIDList = new List<string>
            {
                "TITLE",
                "SEARCH_TITLE",
                "ITEM_NAME",
                "REFERENCE",
                "MATCH_EXACTLY",
                "TOTAL_SALE_PRICE",
                "TOTAL_PURCHASE_PRICE",
                "TOTAL_SALE_PRICE",
                "BRAND",
                "AGENT",
                "CLIENT",
                "DATE",
                "QUANTITY",
                "ITEM",
                "PURCHASE_PRICE",
                "NO_CLIENT_SELECTED",
                "CART",
                "QUOTE_LIST",
            };

            CommandComponentIDList = new List<string>
            {
                "TITLE",
                "SEARCH_TITLE",
                "SETUPS",
                "UTILITIES",
                "GENERATE_QUOTE_PDF",
                "GENERATE_INVOICE_PDF",
                "GENERATE_DELIVERY_PDF",
                "CONVERT_QUOTE_TO_COMMAND",
                "CONVERT_COMMAND_TO_QUOTE",
                "CONVERT_QUOTE_INTO_CREDIT",
                "VALID_COMMAND",
                "CLOSE_COMMAND",
                "UPDATE_COMMAND",
                "COMMAND_CREDIT_ID",
                "INVOICE_ID",
                "CLIENT_ID",
                "COMPANY_NAME",
                "AGENT_IN_CHARGE",
                "START_DATE",
                "END_DATE",
                "AGENT_IN_CHARGE",
                "DATE",
                "CLIENT",
                "AGENT",
                "DETAIL",
                "COMMAND_CREDIT_WAITING_VALIDATION",
                "COMMAND_WAITING_VALIDATION_CLIENT",
                "COMMAND_DELIVERED_WAITING_PAY",
                "COMMAND_CREDIT_CLOSED",
            };

            AgentComponentIDList = new List<string>
            {
                "TITLE",
                "AGENT_LIST",
                "SETUPS",
                "UTILITIES",
                "ADD_NEW_AGENT",
                "ACTIVATE_AGENT",
                "DEACTIVATE_AGENT",
                "CONNECT_AS",
                "DEACTIVATED_AGENT_LIST",
                "FIRST_NAME",
                "LAST_NAME",
                "STATUS",
                "PHONE",
                "FAX",
                "EMAIL",
                "LOGIN",
                "SELECT",
                "DETAIL",
                "MOVE_ALL_CLIENT_TO",
                "MOVE_CLIENT",
            };

            NotificationComponentIDList = new List<string>
            {
                "TITLE",
                "INVOICE_NOT_PAID_LIST",
                "SETUPS",
                "UTILITIES",
                "INVOICE_ID",
                "COMMAND_ID",
                "DATE",
                "COMPANY_NAME",
                "INVOICE_TOTAL",
                "PAY_RECEIVED",
                "LIMIT_DATE",
                "COMMAND_STATUS",
                "DAYS_LATE",
                "DATE_FIRST_REMINDER",
                "DATE_SECOND_REMINDER",
                "WAITING_TO_BE_VALIDATED_MORE_THAN_A_WEEK_LIST",
                "CLIENT",
                "AGENT",
                "DETAIL",
                "CLIENT_WHO_ARE_OVER_MAX_CREDIT",
                "CLIENT_ID",
                "NAME",
                "COMPANY_NAME",
                "STATUS",
                "USED_CREDIT",
                "MAX_CREDIT",
            };

            OptionComponentIDList = new List<string>
            {
                "TITLE",
                "AGENT_CREDENTIAL",
                "DATA_DISPLAY",
                "UTILITIES",
                "SECURITY",
                "ACTIVITY_MONITORING",
                "EMAILS",
                "COMMAND_QUOTE_LIST",
                "UPDATE_LIST_SIZE",
                "BANK_INFORMATION",
                "SORT_CODE",
                "ACCOUNT_NUMBER",
                "ACCOUNT_KEY_NUMBER",
                "BANK_MAME",
                "AGENCY_CODE",
                "IBAN",
                "BIC",
                "COMPANY_NAME",
                "ADDRESS",
                "PHONE",
                "FAX",
                "EMAIL",
                "TAX_CODE",
                "TAX",
                "TAX_VALUE",
                "TAX_TYPE",
                "DATE",
                "COMMENT",
                "NEW_TAX",
                "LEGAL_INFORMATION",
                "TAX_TYPE",
                "TAX_TYPE",
                "TAX_TYPE",
            };

            OptionAgentCredentailComponentIDList = new List<string>
            {
                "TITLE",
                "ROLES",
                "ACTION",
                "CREATE",
                "READ",
                "UPDATE",
                "DELETE",
                "SEND_EMAIL",
                "AGENT_CREDENTIALS",
                "AGENT_ID",
                "AGENT_NAME",
            };

            CommandDetailComponentIDList = new List<string>
            {
                "TITLE",
                "COMMAND_ID",
                "FOR_THE_COMAPANY",
                "MANAGED_BY",
                "PURCHASE_PRICE",
                "SALE_PRICE",
                "QUANTITY",
                "QUANTITY_PENDING",
                "TOTAL_SALE",
                "PERCENTAGE_PROFIT",
                "PROFIT",
                "TOTAL_BEFORE_TAXES",
                "TOTAL_AFTER_TAX",
                "TOTAL_PERCENTAGE_PROFIT",
                "TOTAL_PROFIT",
                "TOTAL_TAXES_AMOUNT",
                "TAX",
                "PRIVATE_COMMENT",
                "COMMENT_WILL_BE_SEEN_BY_CLIENT_ON_QUOTE",
                "ADMIN_COMMENT_ONLY",
                "QUOTE",
                "PRO_FORMAT",
                "DAYS_VALIDITY_OF_QUOTE",
                "GENERATE_QUOTE_PDF",
                "DELIVERY_ADDRESS",
                "DELIVERY_RECEIPT_CREATION",
                "ITEM_REFERENCE",
                "ITEM_NAME",
                "QUANTITY_IN_PROCESS",
                "NUMBER_OF_PACKAGE",
                "INVOICE_CREATION",
                "DELIVERY_ID",
                "CREATION_DATE",
                "NUMBER_OF_PACKAGE_SENT",
                "DELIVERY_RECEIPT_CREATED",
                "SUBJECT",
                "EMAIL_INVOICE",
                "CONSTRUCTOR_REFERENCE_VISISBLE",
                "INVOICE_CREATED",
                "AMOUNT",
                "INVOICE",
                "A_COPY_WILL_BE_SENT_TO_YOU",
                "SEND_EMAIL",
            };

            OptionDataAndDisplayComponentIDList = new List<string>
            {
                "TITLE",
                "INVOICE_IMAGE",
                "WIDTH",
                "HEIGHT",
                "LOGO_IMAGE",
                "HEADER_IMAGE",
                "IMAGES",
                "DATA",
                "NAME",
                "TARGET",
                "TARGETED_COMPONENT",
                "CONTENT",
                "LANGUAGE",
                "CLIENT",
                "HOME",
                "MAIN",
                "CATALOG",
                "QUOTE",
                "COMMAND",
                "AGENT",
                "NOTIFICATION",
                "STATISTIC",
                "OPTION",
                "CATALOG_ITEM",
                "COMMAND_DETAIL",
                "AGENT_DETAIL",
                "OPTION_AGENT_CREDENTIAL",
                "COMPONENT_ID",
                "EMAIL",
            };

            OptionEmailComponentIDList = new List<string>
            {
                "TITLE",
                "QUOTE_EMAIL",
                "COMMAND_VALIDATED_CONFIRMATION_EMAIL",
                "INVOICE_FIRST_REMINDER_EMAIL",
                "INVOICE_SECOND_REMINDER_EMAIL",
                "EMAIL_SENT_ALONG_WITH_INVOICE",
                "UPDATE",
                "ERASE",
            };

            OptionMonitoringComponentIDList = new List<string>
            {

            };

            StatisticComponentIDList = new List<string>
            {

            };

            AgentDetailComponentIDList = new List<string>
            {
                "TITLE",
                "LOGIN",
                "FAX",
                "EMAIL",
                "PHONE",
                "FIRSRT_NAME",
                "LAST_NAME",
                "STATUS",
                "AGENT_STATISTICS",
                "AGENT_RIGHTS",
                "AGENT_ID",
                "SEARCH",
                "AGENT_NAME",
                "LIST_SIZE",
            };

            DeliveryPdfComponentIDList = new List<string> {
                "TITLE",
                "DELIVERY",
                "DATE",
                "PAGE",
                "PACKAGE",
                "DELIVERY_ADDRESS",
                "COMMENT",
                "CLIENT",
                "DELIVERY_RECEIPT",
                "ITEM",
                "QUANTITY_DELIVERED",
                "QUANTITY_ORDERED",
                "NUMBER_OF_PACKAGE",
                "COMPANY_STAMP",
                "SIGNATURE",
                "NAME",
                "COMMAND_NUMBER",
                "PHONE",
            };

            BillPdfComponentIDList = new List<string> {
                "TITLE",
                "BILL",
                "QUOTE",
                "NUMBER",
                "REFERENCE",
                "DATE",
                "PAGE",
                "COMMAND",
                "NUMBER",
                "DELIVERY_ADDRESS",
                "BUSINESS_FOLLOWED_BY",
                "BILL_ADDRESS",
                "COMMENT",
                "CLIENT",
                "UNIT_PRICE",
                "TOTAL_PRICE",
                "BEFORE_TAX",
                "TOTAL_BEFORE_TAX",
                "AMOUNT_TAX",
                "TAX",
                "TOTAL_TAX_INCLUDED",
                "BIC",
                "ACCOUNT_KEY_NUMBER",
                "SORT_CODE",
                "BRANCH_CODE",
                "IBAN",
                "BANK_NAME",
                "ACCOUNT_NUMBER",
                "TAX_CODE",
                "BANK_ADDRESS",
                "CURRENCY",
            };

            QuotePdfComponentIDList = new List<string> {
                "TITLE",
                "BILL",
                "DATE",
                "PAGE",
                "REFERENCE",
                "ITEM",
                "DELIVERY_ADDRESS",
                "BUSINESS_FOLLOWED_BY",
                "BILL_ADDRESS",
                "COMMENT",
                "CLIENT",
                "UNIT_PRICE",
                "TOTAL_PRICE",
                "TOTAL_BEFORE_TAX",
                "AMOUNT_TAX",
                "TAX",
                "TOTAL_AFTER_TAX",
                "BIC",
                "ACCOUNT_KEY_NUMBER",
                "SORT_CODE",
                "BRANCH_CODE",
                "IBAN",
                "BANK_NAME",
                "ACCOUNT_NUMBER",
                "TAX_CODE",
                "BANK_ADDRESS",
            };
        }
    }
}

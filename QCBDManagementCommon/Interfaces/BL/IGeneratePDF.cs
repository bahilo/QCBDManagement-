
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Structures;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IGeneratePDF
    {
        // Operations

        void GeneratePdfCommand(ParamCommandToPdf paramCommandToPdf);

        void GeneratePdfQuote(ParamCommandToPdf paramCommandToPdf);

        void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf);

    } /* end interface IGeneratePDF */
}
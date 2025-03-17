using CarDealership.EDM.Core.Abstractions.Handlers;
using CarDealership.EDM.Core.Models;
using Data;

namespace CarDealership.EDM.Models
{
    public class DealReceiptGenerator : BaseGenerator
    {
        public DealReceiptGenerator(DealReceiptResponse dealReceipt) : base(dealReceipt)
        {
            FileName = $"{dealReceipt.DealDate} - {dealReceipt.Customer}{FormatHelper.GetFormat(DocumentFormat)}";
            Directory = "deals\\receipts\\";
        }
    }
}

using CarDealership.EDM.Core.Abstractions.Handlers;
using CarDealership.EDM.Models;
using Data;

namespace CarDealership.EDM.Factories
{
    public static class GeneratorFactory
    {
        public static BaseGenerator Create (DataResponse response)
        {
            return response.ResponseDataCase switch
            {
                DataResponse.ResponseDataOneofCase.DealReceiptResponse
                    => new DealReceiptGenerator(response.DealReceiptResponse),
                _ => throw new NotSupportedException("Неизвестный тип ответа")
            };
        }
    }
}

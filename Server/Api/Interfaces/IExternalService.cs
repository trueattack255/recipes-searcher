using Sdk.Ofd.Service;
using Sdk.Ofd.Receipt;
using System;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IExternalService
    {
        Task<Data> TryReceiveReceiptAsync(string fiscalNumber, string fiscalDocument, string fiscalSign);

        Task<ReceiptResponse> TryCheckReceiptAsync(string fiscalNumber, string fiscalDocument, string fiscalSign, DateTime date, decimal sum);
    }
}

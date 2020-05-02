using Microsoft.Extensions.Options;
using Sdk.Ofd.Service;
using Sdk.Ofd.Receipt;
using System;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Interfaces;

namespace Api.Services
{
    public class ExternalService : IExternalService
    {
        private readonly string _phone;
        private readonly string _password;

        public ExternalService(IOptions<Settings> settings)
        {
            _phone = settings.Value.Phone;
            _password = settings.Value.Password;
        }

        public async Task<Data> TryReceiveReceiptAsync(string fiscalNumber, string fiscalDocument, string fiscalSign) =>
            await FaultToleranceWrapper.Do(Interface.ReceiveAsync(fiscalNumber, fiscalDocument, fiscalSign, _phone, _password));


        public async Task<ReceiptResponse> TryCheckReceiptAsync(string fiscalNumber, string fiscalDocument,
            string fiscalSign, DateTime date, decimal sum) =>
            await FaultToleranceWrapper.Do(Interface.CheckAsync(fiscalNumber, fiscalDocument, fiscalSign, date, sum));
    }
}

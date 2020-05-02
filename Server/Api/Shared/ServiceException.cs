using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Shared.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceException : Exception
    {
        private class RequestParameter
        {
            [JsonProperty("key")]
            public string Key { get; set; }
            [JsonProperty("value")]
            public string Value { get; set; }
        }

        [JsonProperty("request_params")]
        private List<RequestParameter> RequestParameters { get; }

        public enum CodeError
        {
            Success,
            Error,
            DatabaseError,
            ExternalServiceError,
            ExternalServiceNotAvailable,
            InvalidFormatError,
            ReceiptNotFound,
            IngredientNotFound
        }

        [JsonProperty("error_code")]
        public CodeError ErrorCode { get; }

        [JsonProperty("error_msg")]
        public string ReturnMessage { get; }

        public string FunctionName { get; }

        public string ClassName { get; }

        public ServiceException(string message, IQueryCollection parameters, Exception innerException, string functionName, string className, CodeError errorCode)
            : base(message, innerException)
        {
            ReturnMessage = message;
            FunctionName = functionName;
            ClassName = className;
            ErrorCode = errorCode;
            RequestParameters = ConvertDictToRequestParameter(parameters);
        }

        private List<RequestParameter> ConvertDictToRequestParameter(IQueryCollection dictionary) =>
            dictionary.Select(it => new RequestParameter {Key = it.Key, Value = it.Value}).ToList();
    }
}

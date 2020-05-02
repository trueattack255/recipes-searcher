using Microsoft.AspNetCore.Http;
using System;

namespace Api.Shared.Exceptions
{
    public class DatabaseException : ServiceException
    {
        public DatabaseException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Не удалось подключиться к базе данных", arguments,
            innerException, functionName, controllerName,
            CodeError.DatabaseError)
        { }
    }

    public class DatabaseNotFoundException : ServiceException
    {
        public DatabaseNotFoundException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Чек отсутствует в базе данных", arguments,
            null, null, null,
            CodeError.DatabaseError)
        { }
    }

    public class ExternalServiceNotAvailableException : ServiceException
    {
        public ExternalServiceNotAvailableException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Внешний сервис не отвечает", arguments,
            null, null, null,
            CodeError.ExternalServiceNotAvailable)
        { }
    }

    public class ExternalServiceException : ServiceException
    {
        public ExternalServiceException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Не удалось получить информацию с чека", arguments,
            null, null, null,
            CodeError.ExternalServiceError)
        { }
    }

    public class InternalException : ServiceException
    {
        public InternalException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Ошибка запроса", arguments,
            innerException, null, null,
            CodeError.Error)
        { }
    }

    public class InvalidFormatException : ServiceException
    {
        public InvalidFormatException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Недопустимые значения параметров", arguments,
            innerException, null, null,
            CodeError.InvalidFormatError)
        { }
    }

    public class ReceiptNotFoundException : ServiceException
    {
        public ReceiptNotFoundException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Не удалось обнаружить чек", arguments,
            null, null, null,
            CodeError.ReceiptNotFound)
        { }
    }

    public class IngredientNotFoundException : ServiceException
    {
        public IngredientNotFoundException(IQueryCollection arguments, Exception innerException = null,
            string functionName = null, string controllerName = null) : base(
            "Не удалось получить список ингредиентов", arguments,
            null, null, null,
            CodeError.IngredientNotFound)
        { }
    }
}

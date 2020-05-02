using Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Middleware;
using Api.Models;
using Api.Services;
using Api.Shared;
using Api.Shared.Exceptions;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class RecipesController : Controller, IRecipesContract
    {
        private readonly IDataService _dataService;
        private readonly ISearchService _searchService;
        private readonly ILogService _receiptLogService;
        private readonly IExternalService _externalService;

        private const string FiscalNumberRegexPattern = @"(?<=fn=)(\d*)";
        private const string FiscalDocumentRegexPattern = @"(?<=i=)(\d*)";
        private const string FiscalSignRegexPattern = @"(?<=fp=)(\d*)";
        private const string PurchaseDatePattern = "yyyyMMddTHHmm";

        public RecipesController(IDataService dataService, ISearchService searchService, IExternalService externalService, ReceiptLogService receiptLogService)
        {
            _dataService = dataService;
            _searchService = searchService;
            _externalService = externalService;
            _receiptLogService = receiptLogService;
        }

        [HttpGet]
        public async Task<ActionResult<Recipe>> GetById([FromQuery] string id)
        {
            try
            {
                var result = _dataService.GetById(id);
                return Ok(FormatResponse.Get(result));
            }
            catch (Exception ex)
            {
                throw new DatabaseException(Request.Query, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetByRange([FromQuery] int skip, [FromQuery] int limit)
        {
            try
            {
                var result = _dataService.GetByRange(skip, limit);
                return Ok(FormatResponse.Get(result));
            }
            catch (Exception ex)
            {
                throw new DatabaseException(Request.Query, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Ingredient>>> GetIngredients([FromQuery] string id)
        {
            try
            {
                var receipt = _receiptLogService.GetNote(id);
                var queryString = receipt?.Body;

                if (string.IsNullOrEmpty(queryString))
                    throw new DatabaseNotFoundException(Request.Query);

                var fiscalNumber = queryString.Substring(FiscalNumberRegexPattern);
                var fiscalDocument = queryString.Substring(FiscalDocumentRegexPattern);
                var fiscalSign = queryString.Substring(FiscalSignRegexPattern);

                var receiveResult = _externalService.TryReceiveReceiptAsync(fiscalNumber, fiscalDocument, fiscalSign).Result;
                if (receiveResult == null)
                    throw new ExternalServiceNotAvailableException(Request.Query);

                if (receiveResult.Items == null)
                    throw new ExternalServiceException(Request.Query);

                var itemList = receiveResult.Items.Select(it => it.Name).ToList();
                var goodList = Sdk.Nlp.Analyzer.ParseGoodList(itemList);
                var result = _searchService.SearchIngredients(goodList.Distinct());

                return Ok(FormatResponse.Get(result));
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalException(Request.Query, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<Note>> CheckReceipt([FromQuery] string t, [FromQuery] string s,
            [FromQuery] string fn, [FromQuery] string i, [FromQuery] string fp)
        {
            try
            {
                var sum = s.ToDecimal();
                var date = t.ToDateTime(PurchaseDatePattern);

                var checkResult = _externalService.TryCheckReceiptAsync(fn, i, fp, date.Value, sum.Value).Result;
                if (checkResult.StatusCode == HttpStatusCode.NotAcceptable)
                    throw new ReceiptNotFoundException(Request.Query);

                if (checkResult.StatusCode != HttpStatusCode.NoContent)
                    throw new ExternalServiceNotAvailableException(Request.Query);

                var result = _receiptLogService.AddNote(MainMiddleware.GetQueryString(HttpContext),
                    MainMiddleware.GetErrorName(ServiceException.CodeError.Success));

                return Ok(FormatResponse.Get(result));
            }
            catch (ArgumentException ex)
            {
                throw new InvalidFormatException(Request.Query, ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidFormatException(Request.Query, ex);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalException(Request.Query, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> Search([FromQuery] string con, [FromQuery] string ing)
        {
            try
            {
                _searchService.Percent = con.ToDouble().Value;
                if (string.IsNullOrEmpty(ing))
                    throw new IngredientNotFoundException(Request.Query);

                var result = _searchService.SearchRecipes(ing.Split(','))
                    .OrderByDescending(it => it.Concurrency)
                    .ToList();

                return Ok(FormatResponse.Get(result));
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalException(Request.Query, ex);
            }
        }
    }
}

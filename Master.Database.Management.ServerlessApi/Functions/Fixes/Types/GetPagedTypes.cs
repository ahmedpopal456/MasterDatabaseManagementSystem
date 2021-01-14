using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Fixit.Core.DataContracts.Fixes.Types;

namespace Master.Database.Management.ServerlessApi.Functions.Fixes.Templates
{
  public class GetPagedTypes
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;
    private const int _PAGESIZE = 20;

    public GetPagedTypes(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(IRequestMediatorFactory)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("GetPagedTypesAsync")]
    [OpenApiOperation("get", "FixTypes")]
    [OpenApiParameter("pageNumber", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiParameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("typeName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PagedModelCollectionDto<FixTypeDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixTypes/{pageNumber:int}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          int pageNumber)
    {
      int.TryParse(HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("pageSize"), out var parsedPageSize);
      var typeName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("typeName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetPagedTypesAsync(parsedPageSize, pageNumber, cancellationToken, typeName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetPagedTypesAsync(int pageSize, int currentPage, CancellationToken cancellationToken, string typeName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid, cannot validate TimestampUtc...");
      }

      var result = await _requestMediatorFactory.RequestFixTypeMediator().GetManyByPageAsync(currentPage, cancellationToken, pageSize, typeName, minTimestampUtcResult, maxTimestampUtcResult);

      return new OkObjectResult(result);
    }
  }
}

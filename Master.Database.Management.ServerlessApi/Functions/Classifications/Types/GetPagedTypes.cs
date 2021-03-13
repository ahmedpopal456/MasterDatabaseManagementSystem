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
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using Fixit.Core.DataContracts.Classifications;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Types
{
  public class GetPagedTypes
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetPagedTypes(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetPagedTypes)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetPagedWorkTypesAsync")]
    [OpenApiOperation("get", "WorkTypes")]
    [OpenApiParameter("pageNumber", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiParameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("typeName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PagedModelCollectionDto<WorkTypeDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "workTypes/{pageNumber:int}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          int pageNumber)
    {
      int.TryParse(HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("pageSize"), out var parsedPageSize);
      var typeName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("typeName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetPagedWorkTypesAsync(parsedPageSize, pageNumber, cancellationToken, typeName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetPagedWorkTypesAsync(int pageSize, int currentPage, CancellationToken cancellationToken, string typeName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"{nameof(GetPagedWorkTypesAsync)}: Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid...");
      }

      var paginationRequestDto = new PaginationRequestBaseDto
      {
        PageNumber = currentPage,
        PageSize = pageSize,
        Name = typeName,
        MinTimestampUtc = minTimestampUtcResult,
        MaxTimestampUtc = maxTimestampUtcResult
      };
      var result = await _requestMediatorFactory.RequestWorkTypeMediator().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}

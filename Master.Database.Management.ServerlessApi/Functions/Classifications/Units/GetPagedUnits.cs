using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Units
{
  public class GetPagedUnits
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetPagedUnits(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetPagedUnits)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetPagedUnitsAsync")]
    [OpenApiOperation("get", "GetPagedUnits")]
    [OpenApiParameter("pageNumber", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiParameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("unitName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PagedModelCollectionDto<FixUnitDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixUnits/{pageNumber:int}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          int pageNumber)
    {
      int.TryParse(HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("pageSize"), out var parsedPageSize);
      var unitName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("unitName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetPagedUnitsAsync(parsedPageSize, pageNumber, cancellationToken, unitName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetPagedUnitsAsync(int pageSize, int currentPage, CancellationToken cancellationToken, string unitName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"{nameof(GetPagedUnitsAsync)}: Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid...");
      }

      var paginationRequestDto = new PaginationRequestBaseDto
      {
        PageNumber = currentPage,
        PageSize = pageSize,
        Name = unitName,
        MinTimestampUtc = minTimestampUtcResult,
        MaxTimestampUtc = maxTimestampUtcResult
      };
      var result = await _requestMediatorFactory.RequestFixUnitMediator().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}

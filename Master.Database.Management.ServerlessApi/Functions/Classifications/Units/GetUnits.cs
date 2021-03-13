using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using Fixit.Core.DataContracts.Classifications;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Units
{
  public class GetUnits
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetUnits(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetUnits)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetUnitsAsync")]
    [OpenApiOperation("get", "GetUnits")]
    [OpenApiParameter("unitName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IList<WorkCategoryDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixUnits")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken)
    {
      var unitName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("unitName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetUnitsAsync(cancellationToken, unitName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetUnitsAsync(CancellationToken cancellationToken, string unitName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"{nameof(GetUnitsAsync)}: Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid...");
      }

      var filterBaseDto = new FilterBaseDto
      {
        Name = unitName,
        MinTimestampUtc = minTimestampUtcResult,
        MaxTimestampUtc = maxTimestampUtcResult
      };
      var result = await _requestMediatorFactory.RequestFixUnitMediator().GetManyAsync(filterBaseDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}

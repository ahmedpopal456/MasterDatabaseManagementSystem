using System;
using System.Linq;
using System.Collections.Generic;
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
using Fixit.Core.DataContracts.Fixes.Types;

namespace Master.Database.Management.ServerlessApi.Functions.Fixes.Templates
{
  public class GetTypes
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetTypes(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(IRequestMediatorFactory)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("GetFixTypesAsync")]
    [OpenApiOperation("get", "FixTypes")]
    [OpenApiParameter("typeName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IEnumerable<FixTypeDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixTypes")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken)
    {
      var typeName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("typeName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetFixTypesAsync(cancellationToken, typeName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetFixTypesAsync(CancellationToken cancellationToken, string typeName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid, cannot validate TimestampUtc...");
      }

      var result = await _requestMediatorFactory.RequestFixTypeMediator().GetManyAsync(cancellationToken, typeName, minTimestampUtcResult, maxTimestampUtcResult);

      return new OkObjectResult(result);
    }
  }
}

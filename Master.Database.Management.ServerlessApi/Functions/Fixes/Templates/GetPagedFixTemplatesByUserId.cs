using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using System.Web;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.ServerlessApi.Functions.Fixes.Templates
{
  public class GetPagedFixTemplatesByUserId
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetPagedFixTemplatesByUserId(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(IRequestMediatorFactory)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("GetPagedFixTemplatesByUserIdAsync")]
    [OpenApiOperation("get", "FixTemplate")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter("pageNumber", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiParameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
    [OpenApiParameter("status", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("typeName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("categoryName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PagedModelCollectionDto<FixTemplateDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixTemplates/users/{id:Guid}/{pageNumber}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          Guid id,
                                          int pageNumber)
    {
      int.TryParse(HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("pageSize"), out var parsedPageSize);
      var status = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("status");
      var typeName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("typeName");
      var categoryName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("categoryName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetPagedFixTemplatesByUserIdAsync(id, pageNumber, cancellationToken, parsedPageSize, status, typeName, categoryName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetPagedFixTemplatesByUserIdAsync(Guid userId, int pageNumber, CancellationToken cancellationToken, int? pageSize = null, string status = null, string typeName = null, string categoryName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid, cannot validate TimestampUtc...");
      }

      FixTemplateStatus? fixTemplateStatus = default;
      if (status != null && !OptionalQueryValidators.TryParseStatus(status, out fixTemplateStatus))
      {
        return new BadRequestObjectResult($"{nameof(status)} is not an underlying value of the {nameof(FixTemplateStatus)} enumeration...");
      }

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().GetManyByPageAsync(pageNumber, cancellationToken, pageSize, userId, tags: null, status: fixTemplateStatus, typeName, categoryName, minTimestampUtcResult, maxTimestampUtcResult);

      return new OkObjectResult(result);
    }
  }
}

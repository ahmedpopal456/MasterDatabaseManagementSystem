using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.FixTemplates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;

namespace Master.Database.Management.ServerlessApi.Functions.FixTemplates
{
  public class GetFixTemplatesByUserId
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetFixTemplatesByUserId(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(IRequestMediatorFactory)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("GetFixTemplatesByUserIdAsync")]
    [OpenApiOperation("get", "FixTemplate")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter("status", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("tags", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("templateName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("typeName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("categoryName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("unitName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixTemplates/users/{id:Guid}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          Guid id)
    {
      return await GetFixTemplatesByUserIdAsync(id, httpRequest, cancellationToken);
    }

    public async Task<IActionResult> GetFixTemplatesByUserIdAsync(Guid userId, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      #region Parse Query
      var status = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("status");
      var tags = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("tags")?.Split(',');
      var templateName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("templateName");
      var typeName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("typeName");
      var categoryName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("categoryName");
      var unitName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("unitName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");
      #endregion

      var result = default(IEnumerable<FixTemplateDto>);

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"{nameof(GetFixTemplatesByUserIdAsync)}: Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid...");
      }

      FixTemplateStatus? fixTemplateStatus = default;
      if (status != null && !OptionalQueryValidators.TryParseStatus(status, out fixTemplateStatus))
      {
        return new BadRequestObjectResult($"{nameof(GetFixTemplatesByUserIdAsync)}: {nameof(status)} is not an underlying value of the {nameof(FixTemplateStatus)} enumeration...");
      }

      var fixTemplateFilterDto = new FixTemplateFilterDto
      {
        UserId = userId,
        Status = fixTemplateStatus,
        Name = templateName,
        TypeName = typeName,
        CategoryName = categoryName,
        UnitName = unitName,
        Tags = tags,
        MinTimestampUtc = minTimestampUtcResult,
        MaxTimestampUtc = maxTimestampUtcResult
      };
      result = await _requestMediatorFactory.RequestFixTemplateMediator().GetManyAsync(fixTemplateFilterDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}

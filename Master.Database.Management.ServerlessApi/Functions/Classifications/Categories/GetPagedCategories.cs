using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Helpers.Validators;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;
using Fixit.Core.DataContracts.Classifications;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Categories
{
  public class GetPagedCategories
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetPagedCategories(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetPagedCategories)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetPagedWorkCategoriesAsync")]
    [OpenApiOperation("get", "GetPagedWorkCategories")]
    [OpenApiParameter("pageNumber", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiParameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("categoryName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PagedModelCollectionDto<WorkCategoryDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "workCategories/{pageNumber:int}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          int pageNumber)
    {
      int.TryParse(HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("pageSize"), out var parsedPageSize);
      var categoryName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("categoryName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetPagedWorkCategoriesAsync(parsedPageSize, pageNumber, cancellationToken, categoryName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetPagedWorkCategoriesAsync(int pageSize, int currentPage, CancellationToken cancellationToken, string categoryName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"{nameof(GetPagedWorkCategoriesAsync)}: Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid...");
      }

      var paginationRequestDto = new PaginationRequestBaseDto
      {
        PageNumber = currentPage,
        PageSize = pageSize,
        Name = categoryName,
        MinTimestampUtc = minTimestampUtcResult,
        MaxTimestampUtc = maxTimestampUtcResult
      };
      var result = await _requestMediatorFactory.RequestWorkCategoryMediator().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}

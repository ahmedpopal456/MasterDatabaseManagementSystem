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
using Fixit.Core.DataContracts.Users.Skills;

namespace Master.Database.Management.ServerlessApi.Functions.Skills
{
  public class GetPagedSkills
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetPagedSkills(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetPagedSkills)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetPagedSkillsAsync")]
    [OpenApiOperation("get", "GetPagedSkills")]
    [OpenApiParameter("pageNumber", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
    [OpenApiParameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("skillName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("minTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiParameter("maxTimestampUtc", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PagedModelCollectionDto<SkillDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "skills/{pageNumber:int}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          int pageNumber)
    {
      int.TryParse(HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("pageSize"), out var parsedPageSize);
      var skillName = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("skillName");
      var minTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("minTimestampUtc");
      var maxTimestampUtc = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("maxTimestampUtc");

      return await GetPagedSkillsAsync(parsedPageSize, pageNumber, cancellationToken, skillName, minTimestampUtc, maxTimestampUtc);
    }

    public async Task<IActionResult> GetPagedSkillsAsync(int pageSize, int currentPage, CancellationToken cancellationToken, string skillName = null, string minTimestampUtc = null, string maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      long? minTimestampUtcResult = default;
      long? maxTimestampUtcResult = default;
      if ((minTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(minTimestampUtc, out minTimestampUtcResult))
          || (maxTimestampUtc != null && !OptionalQueryValidators.TryParseTimestampUtc(maxTimestampUtc, out maxTimestampUtcResult)))
      {
        return new BadRequestObjectResult($"{nameof(GetPagedSkillsAsync)}: Either {nameof(minTimestampUtc)} or {nameof(maxTimestampUtc)} is invalid...");
      }

      var paginationRequestDto = new PaginationRequestBaseDto
      {
        PageNumber = currentPage,
        PageSize = pageSize,
        Name = skillName,
        MinTimestampUtc = minTimestampUtcResult,
        MaxTimestampUtc = maxTimestampUtcResult
      };
      var result = await _requestMediatorFactory.RequestSkillMediator().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}

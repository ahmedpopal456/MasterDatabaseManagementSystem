using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;
using Fixit.Core.DataContracts.Classifications;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Categories
{
  public class GetCategoryById
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetCategoryById(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetCategoryById)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetWorkCategoryByIdAsync")]
    [OpenApiOperation("get", "GetWorkCategoryById")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(WorkCategoryDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "workCategories/{id:Guid}")]
                                          CancellationToken cancellationToken,
                                          Guid id)
    {

      return await GetWorkCategoryByIdAsync(id, cancellationToken);
    }

    public async Task<IActionResult> GetWorkCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMediatorFactory.RequestWorkCategoryMediator().GetByIdAsync(id, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"{nameof(GetWorkCategoryByIdAsync)}: The requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

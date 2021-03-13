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
using Fixit.Core.DataContracts.Classifications;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Types
{
  public class GetTypeById
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetTypeById(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetTypeById)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("GetWorkTypeByIdAsync")]
    [OpenApiOperation("get", "WorkType")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(WorkTypeDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "workTypes/{id:Guid}")]
                                          CancellationToken cancellationToken,
                                          Guid id)
    {

      return await GetWorkTypeByIdAsync(id, cancellationToken);
    }

    public async Task<IActionResult> GetWorkTypeByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMediatorFactory.RequestWorkTypeMediator().GetByIdAsync(id, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"{nameof(GetWorkTypeByIdAsync)}: The requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

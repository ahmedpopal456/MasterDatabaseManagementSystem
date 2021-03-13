using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace Master.Database.Management.ServerlessApi.Functions.Classifications.Units
{
  public class GetUnitById
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetUnitById(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetUnitById)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetUnitByIdAsync")]
    [OpenApiOperation("get", "GetUnitById")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixUnitDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixUnits/{id:Guid}")]
                                          CancellationToken cancellationToken,
                                          Guid id)
    {

      return await GetUnitByIdAsync(id, cancellationToken);
    }

    public async Task<IActionResult> GetUnitByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMediatorFactory.RequestFixUnitMediator().GetByIdAsync(id, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"{nameof(GetUnitByIdAsync)}: The requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

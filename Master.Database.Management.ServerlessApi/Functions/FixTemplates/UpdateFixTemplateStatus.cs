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
using Fixit.Core.DataContracts.FixTemplates;
using Master.Database.Management.ServerlessApi.Helpers.Validators;

namespace Master.Database.Management.ServerlessApi.Functions.FixTemplates
{
  public class UpdateFixTemplateStatus
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public UpdateFixTemplateStatus(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(UpdateFixTemplateStatus)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("UpdateFixTemplateStatusAsync")]
    [OpenApiOperation("put", "FixTemplateStatus")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter("status", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixTemplates/{id:Guid}/status/{status}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          Guid id,
                                          string status)
    {

      return await UpdateFixTemplateStatusAsync(id, status, cancellationToken);
    }

    public async Task<IActionResult> UpdateFixTemplateStatusAsync(Guid id, string status, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!OptionalQueryValidators.TryParseStatus(status, out var fixTemplateStatus))
      {
        return new BadRequestObjectResult($"{nameof(UpdateFixTemplateStatusAsync)}: Either {nameof(status)} is null or is not an underlying value of the {nameof(FixTemplateStatus)} enumeration...");
      }

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().UpdateStatusAsync(id, fixTemplateStatus.Value, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"{nameof(UpdateFixTemplateStatusAsync)}: Unable to update {nameof(status)}, the requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

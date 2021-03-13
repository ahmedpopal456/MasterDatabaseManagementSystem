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
using Master.Database.Management.ServerlessApi.Helpers.Validators.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.ServerlessApi.Functions.FixTemplates
{
  public class UpdateFixTemplate
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public UpdateFixTemplate(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(UpdateFixTemplate)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("UpdateFixTemplateAsync")]
    [OpenApiOperation("put", "FixTemplates")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiRequestBody("application/json", typeof(FixTemplateUpdateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixTemplates/{id:Guid}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          Guid id)
    {

      return await UpdateFixTemplateAsync(id, httpRequest, cancellationToken);
    }

    public async Task<IActionResult> UpdateFixTemplateAsync(Guid id, HttpRequestMessage httpRequest, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixTemplateUpdateRequestValidators.IsValidFixTemplateUpdateRequest(httpRequest.Content, out FixTemplateUpdateRequestDto fixTemplateUpdateRequestDto))
      {
        return new BadRequestObjectResult($"{nameof(UpdateFixTemplateAsync)}: Either {nameof(FixTemplateUpdateRequestDto)} is null or has one or more invalid fields...");
      }
      if (id.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(UpdateFixTemplateAsync)} expects a value for {nameof(id)}... null argument was provided");
      }

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().UpdateAsync(id, fixTemplateUpdateRequestDto, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"{nameof(UpdateFixTemplateAsync)}: The requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

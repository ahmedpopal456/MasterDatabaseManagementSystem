using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Helpers.Validators.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.ServerlessApi.Functions.FixTemplates
{
  public class CreateFixTemplate
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public CreateFixTemplate(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(CreateFixTemplate)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("CreateFixTemplateAsync")]
    [OpenApiOperation("post", "FixTemplates")]
    [OpenApiRequestBody("application/json", typeof(FixTemplateCreateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "fixTemplates")]
                                         HttpRequestMessage httpRequest,
                                         CancellationToken cancellationToken)
    {
      return await CreateFixTemplateAsync(httpRequest, cancellationToken);
    }

    public async Task<IActionResult> CreateFixTemplateAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixTemplateCreateRequestValidators.IsValidFixTemplateCreateRequest(httpRequest.Content, out FixTemplateCreateRequestDto fixTemplateCreateRequestDto))
      {
        return new BadRequestObjectResult($"{nameof(CreateFixTemplateAsync)}: Either {nameof(FixTemplateCreateRequestDto)} is null or has one or more invalid fields...");
      }

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().CreateAsync(fixTemplateCreateRequestDto, cancellationToken);
      if (result == null)
      {
        return new UnprocessableEntityObjectResult($"{nameof(CreateFixTemplateAsync)}: The client request contains semantic errors, and the server can’t process it...");
      }

      return new OkObjectResult(result);
    }
  }
}

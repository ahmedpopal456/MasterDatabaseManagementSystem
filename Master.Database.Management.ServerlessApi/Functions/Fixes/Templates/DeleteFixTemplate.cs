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
using Microsoft.Extensions.Logging;
using Master.Database.Management.ServerlessApi.Mediators;
using System.Web;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.ServerlessApi.Functions.Fixes.Templates
{
  public class DeleteFixTemplate
  {
    private readonly ILogger _logger;
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public DeleteFixTemplate(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(CreateFixTemplate)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("DeleteFixTemplateAsync")]
    [OpenApiOperation("delete", "FixTemplates")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter("softDelete", In = ParameterLocation.Query, Required = false, Type = typeof(bool))]
    [OpenApiRequestBody("application/json", typeof(FixTemplateCreateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "fixTemplates/{id:Guid}")]
                                         HttpRequestMessage httpRequest,
                                         CancellationToken cancellationToken,
                                         Guid id)
    {
      var softDelete = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get("softDelete");
      
      return await DeleteFixTemplateAsync(id, softDelete, cancellationToken);
    }

    public async Task<IActionResult> DeleteFixTemplateAsync(Guid id, string softDelete, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      bool defaultSoftDelete = true;
      if (softDelete != null && bool.TryParse(softDelete, out bool parsedSoftDelete))
      {
        defaultSoftDelete = parsedSoftDelete;
      }

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().DeleteAsync(id, defaultSoftDelete, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"A fix template with id {id} was not found, cannot delete...");
      }

      return new OkObjectResult(result);
    }
  }
}

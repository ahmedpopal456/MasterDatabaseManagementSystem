using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Master.Database.Management.ServerlessApi.Mediators;
using System.Web;

namespace Master.Database.Management.ServerlessApi.Functions.FixTemplates
{
  public class DeleteFixTemplate
  {
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
        return new NotFoundObjectResult($"{nameof(DeleteFixTemplateAsync)}: Unable to delete, the requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

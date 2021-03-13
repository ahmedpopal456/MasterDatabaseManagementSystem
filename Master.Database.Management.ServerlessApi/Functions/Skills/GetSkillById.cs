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
using Fixit.Core.DataContracts.Users.Skills;

namespace Master.Database.Management.ServerlessApi.Functions.Skills
{
  public class GetSkillById
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetSkillById(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(GetSkillById)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");
    }

    [FunctionName("GetSkillByIdAsync")]
    [OpenApiOperation("get", "GetSkillById")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SkillDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "skills/{id:Guid}")]
                                          CancellationToken cancellationToken,
                                          Guid id)
    {

      return await GetSkillByIdAsync(id, cancellationToken);
    }

    public async Task<IActionResult> GetSkillByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMediatorFactory.RequestSkillMediator().GetByIdAsync(id, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"{nameof(GetSkillByIdAsync)}: The requested resource with {nameof(Guid)} {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

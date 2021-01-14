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

namespace Master.Database.Management.ServerlessApi.Functions.Fixes.Templates
{
  public class UpdateSystemCostEstimate
  {
		private readonly IRequestMediatorFactory _requestMediatorFactory;

		public UpdateSystemCostEstimate(IRequestMediatorFactory requestMediatorFactory)
		{
			_requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(UpdateSystemCostEstimate)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

		}

		[FunctionName("UpdateSystemCostEstimateAsync")]
		[OpenApiOperation("put", "SystemCostEstimate")]
		[OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter("cost", In = ParameterLocation.Path, Required = true, Type = typeof(double))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixTemplates/{id:Guid}/cost/{cost}")]
																					HttpRequestMessage httpRequest,
																					CancellationToken cancellationToken,
																					Guid id,
                                          double cost)
		{

			return await UpdateSystemCostEstimateAsync(id, cost, cancellationToken);
		}

		public async Task<IActionResult> UpdateSystemCostEstimateAsync(Guid id, double cost, CancellationToken cancellationToken)
		{
      cancellationToken.ThrowIfCancellationRequested();

      if (cost < default(double))
      {
        return new BadRequestObjectResult($"The {nameof(cost)} is not a valid number...");
      }

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().UpdateCostAsync(id, cost, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"A fix template with id {id} was not found, cannot update its system cost estimate with value {cost}...");
      }

      return new OkObjectResult(result);
    }
	}
}

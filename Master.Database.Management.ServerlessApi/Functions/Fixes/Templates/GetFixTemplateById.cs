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
  public class GetFixTemplateById
  {
		private readonly IRequestMediatorFactory _requestMediatorFactory;

		public GetFixTemplateById(IRequestMediatorFactory requestMediatorFactory)
		{
			_requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(IRequestMediatorFactory)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

		}

		[FunctionName("GetFixTemplateByIdAsync")]
		[OpenApiOperation("get", "FixTemplate")]
		[OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
		[OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTemplateDto))]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixTemplates/{id:Guid}")]
																					HttpRequestMessage httpRequest,
																					CancellationToken cancellationToken,
																					Guid id)
		{

			return await GetFixTemplateByIdAsync(id, cancellationToken);
		}

		public async Task<IActionResult> GetFixTemplateByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMediatorFactory.RequestFixTemplateMediator().GetByIdAsync(id, cancellationToken);

			if (result == null)
			{
				return new NotFoundObjectResult($"A fix template with id {id} was not found...");
			}

			return new OkObjectResult(result);
		}
	}
}

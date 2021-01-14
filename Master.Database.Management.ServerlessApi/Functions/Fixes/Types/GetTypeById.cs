﻿using System;
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
using Fixit.Core.DataContracts.Fixes.Types;

namespace Master.Database.Management.ServerlessApi.Functions.Fixes.Templates
{
  public class GetTypeById
  {
    private readonly IRequestMediatorFactory _requestMediatorFactory;

    public GetTypeById(IRequestMediatorFactory requestMediatorFactory)
    {
      _requestMediatorFactory = requestMediatorFactory ?? throw new ArgumentNullException($"{nameof(IRequestMediatorFactory)} expects a value for {nameof(requestMediatorFactory)}... null argument was provided");

    }

    [FunctionName("GetTypeByIdAsync")]
    [OpenApiOperation("get", "FixType")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixTypeDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixTypes/{id:Guid}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          Guid id)
    {

      return await GetTypeByIdAsync(id, cancellationToken);
    }

    public async Task<IActionResult> GetTypeByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMediatorFactory.RequestFixTypeMediator().GetByIdAsync(id, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"A fix type with id {id} was not found...");
      }

      return new OkObjectResult(result);
    }
  }
}

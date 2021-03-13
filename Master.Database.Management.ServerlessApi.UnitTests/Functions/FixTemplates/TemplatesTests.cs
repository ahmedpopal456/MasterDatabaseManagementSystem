using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Functions.FixTemplates;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions.FixTemplates
{
  [TestClass]
  public class TemplatesTests : TestBase
  {
    private Mock<IFixTemplateMediator> _fixTemplateMediator;

    // Fake Data
    private IEnumerable<FixTemplateDto> _fakeFixTemplates;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _fixTemplateMediator = new Mock<IFixTemplateMediator>();

      _fakeFixTemplates = _fakeDtoSeederFactory.CreateSeederFactory(new FixTemplateDto());

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestFixTemplateMediator()).Returns(_fixTemplateMediator.Object);
    }

    #region CreateFixTemplateAsyncTests
    [TestMethod]
    public async Task CreateFixTemplateAsync_CreateSuccess_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fakeFixTemplateCreateRequestDto = _fakeDtoSeederFactory.CreateSeederFactory(new FixTemplateCreateRequestDto()).First();
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(fakeFixTemplateCreateRequestDto);
      var createFixTemplateAsyncFunction = new CreateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.CreateAsync(It.IsAny<FixTemplateCreateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await createFixTemplateAsyncFunction.CreateFixTemplateAsync(fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task CreateFixTemplateAsync_InvalidFixTemplateCreateRequest_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(default(FixTemplateCreateRequestDto));
      var createFixTemplateAsyncFunction = new CreateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.CreateAsync(It.IsAny<FixTemplateCreateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await createFixTemplateAsyncFunction.CreateFixTemplateAsync(fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task CreateFixTemplateAsync_CreateFail_ReturnsUnprocessableEntityObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fakeFixTemplateCreateRequestDto = _fakeDtoSeederFactory.CreateSeederFactory(new FixTemplateCreateRequestDto()).First();
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(fakeFixTemplateCreateRequestDto);
      var createFixTemplateAsyncFunction = new CreateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.CreateAsync(It.IsAny<FixTemplateCreateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await createFixTemplateAsyncFunction.CreateFixTemplateAsync(fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(UnprocessableEntityObjectResult));
    }
    #endregion

    #region DeleteFixTemplateAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_FixTemplateId")]
    public async Task DeleteFixTemplateAsync_DeleteSuccess_ReturnsOkObjectResult(string fixTemplatId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplatIdGuid = new Guid(fixTemplatId);
      var softDeleteString = "true";
      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true
      };
      var deleteFixTemplateAsyncFunction = new DeleteFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.DeleteAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(operationStatus);

      // Act
      var actionResult = await deleteFixTemplateAsyncFunction.DeleteFixTemplateAsync(fixTemplatIdGuid, softDeleteString, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out OperationStatus operationStatusResult));
      Assert.IsTrue(operationStatusResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_FixTemplateId")]
    public async Task DeleteFixTemplateAsync_FixTemplateIdNotFound_ReturnsNotFoundObjectResult(string fixTemplatId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplatIdGuid = new Guid(fixTemplatId);
      var softDeleteString = "true";
      var deleteFixTemplateAsyncFunction = new DeleteFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.DeleteAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(OperationStatus));

      // Act
      var actionResult = await deleteFixTemplateAsyncFunction.DeleteFixTemplateAsync(fixTemplatIdGuid, softDeleteString, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Any_FixTemplateId")]
    public async Task DeleteFixTemplateAsync_DeleteFail_ReturnsOkObjectResult(string fixTemplatId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplatIdGuid = new Guid(fixTemplatId);
      var softDeleteString = "true";
      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false
      };
      var deleteFixTemplateAsyncFunction = new DeleteFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.DeleteAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(operationStatus);

      // Act
      var actionResult = await deleteFixTemplateAsyncFunction.DeleteFixTemplateAsync(fixTemplatIdGuid, softDeleteString, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out OperationStatus operationStatusResult));
      Assert.IsFalse(operationStatusResult.IsOperationSuccessful);
    }
    #endregion

    #region GetFixTemplateByIdAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_FixTemplateId")]
    public async Task GetFixTemplateByIdAsync_FixTemplateIdFound_ReturnsOkObjectResult(string fixTemplatId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplatIdGuid = new Guid(fixTemplatId);
      var getFixTemplateByIdFunction = new GetFixTemplateById(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await getFixTemplateByIdFunction.GetFixTemplateByIdAsync(fixTemplatIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_FixTemplateId")]
    public async Task GetFixTemplateByIdAsync_FixTemplateIdNotFound_ReturnsNotFoundObjectResult(string fixTemplatId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplatIdGuid = new Guid(fixTemplatId);
      var getFixTemplateByIdFunction = new GetFixTemplateById(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await getFixTemplateByIdFunction.GetFixTemplateByIdAsync(fixTemplatIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetFixTemplatesByUserIdAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", "http://localhost:123/api/fixTemplates/users/445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_UserId")]
    public async Task GetFixTemplatesByUserIdAsync_UserIdFound_ReturnsOkObjectResult(string userId, string uri)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var getFixTemplatesByUserIdFunction = new GetFixTemplatesByUserId(_fakeRequestMediatorFactory.Object);
      var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetManyAsync(It.IsAny<FixTemplateFilterDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates);

      // Act
      var actionResult = await getFixTemplatesByUserIdFunction.GetFixTemplatesByUserIdAsync(userIdGuid, httpRequest, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", "http://localhost:123/api/fixTemplates/users/db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_UserId")]
    public async Task GetFixTemplatesByUserIdAsync_UserIdNotFound_ReturnsOkObjectResult(string userId, string uri)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var getFixTemplatesByUserIdFunction = new GetFixTemplatesByUserId(_fakeRequestMediatorFactory.Object);
      var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetManyAsync(It.IsAny<FixTemplateFilterDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<FixTemplateDto>());

      // Act
      var actionResult = await getFixTemplatesByUserIdFunction.GetFixTemplatesByUserIdAsync(userIdGuid, httpRequest, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<FixTemplateDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", "http://localhost:123/api/fixTemplates/users/445e50d1-b2e7-4c25-a628-c610aed7a357?status=UnkownStatus", DisplayName = "Known_UserId")]
    public async Task GetFixTemplatesByUserIdAsync_InvalidStatus_ReturnsBadRequestObjectResult(string userId, string uri)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var getFixTemplatesByUserIdFunction = new GetFixTemplatesByUserId(_fakeRequestMediatorFactory.Object);
      var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetManyAsync(It.IsAny<FixTemplateFilterDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates);

      // Act
      var actionResult = await getFixTemplatesByUserIdFunction.GetFixTemplatesByUserIdAsync(userIdGuid, httpRequest, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region GetPagedFixTemplatesByUserIdAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", 1, "http://localhost:123/api/fixTemplates/users/445e50d1-b2e7-4c25-a628-c610aed7a357/1", DisplayName = "Known_UserId")]
    public async Task GetPagedFixTemplatesByUserIdAsync_UserIdFound_ReturnsOkObjectResult(string userId, int pageNumber, string uri)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var pagedModelCollectionDto = new PagedModelCollectionDto<FixTemplateDto>()
      {
        Results = _fakeFixTemplates.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeFixTemplates.Count()
      };
      var getPagedFixTemplatesByUserIdFunction = new GetPagedFixTemplatesByUserId(_fakeRequestMediatorFactory.Object);
      var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetManyByPageAsync(It.IsAny<FixTemplatePaginationRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await getPagedFixTemplatesByUserIdFunction.GetPagedFixTemplatesByUserIdAsync(userIdGuid, pageNumber, httpRequest, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", 1, "http://localhost:123/api/fixTemplates/users/db53bc7b-2892-47a9-9134-3696579878df/1", DisplayName = "Any_UserId")]
    public async Task GetPagedFixTemplatesByUserIdAsync_UserIdNotFound_ReturnsOkObjectResult(string userId, int pageNumber, string uri)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var getPagedFixTemplatesByUserIdFunction = new GetPagedFixTemplatesByUserId(_fakeRequestMediatorFactory.Object);
      var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetManyByPageAsync(It.IsAny<FixTemplatePaginationRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new PagedModelCollectionDto<FixTemplateDto>());

      // Act
      var actionResult = await getPagedFixTemplatesByUserIdFunction.GetPagedFixTemplatesByUserIdAsync(userIdGuid, pageNumber, httpRequest, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<FixTemplateDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Results.Any());
      Assert.IsTrue(objectResultValue.PageNumber == 0);
      Assert.IsTrue(objectResultValue.TotalModelCount == 0);
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", 1, "http://localhost:123/api/fixTemplates/users/445e50d1-b2e7-4c25-a628-c610aed7a357/1?status=UnknownStatus", DisplayName = "Known_UserId")]
    public async Task GetPagedFixTemplatesByUserIdAsync_InvalidStatus_ReturnsBadRequestObjectResult(string userId, int pageNumber, string uri)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var pagedModelCollectionDto = new PagedModelCollectionDto<FixTemplateDto>()
      {
        Results = _fakeFixTemplates.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeFixTemplates.Count()
      };
      var getPagedFixTemplatesByUserIdFunction = new GetPagedFixTemplatesByUserId(_fakeRequestMediatorFactory.Object);
      var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.GetManyByPageAsync(It.IsAny<FixTemplatePaginationRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await getPagedFixTemplatesByUserIdFunction.GetPagedFixTemplatesByUserIdAsync(userIdGuid, pageNumber, httpRequest, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region UpdateFixTemplateAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateFixTemplateAsync_FixTemplateIdFound_ReturnsOkObjectResult(string userId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var fakeFixTemplateUpdateRequestDto = _fakeDtoSeederFactory.CreateSeederFactory(new FixTemplateUpdateRequestDto()).First();
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(fakeFixTemplateUpdateRequestDto);
      var updateFixTemplateFunction = new UpdateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateUpdateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await updateFixTemplateFunction.UpdateFixTemplateAsync(userIdGuid, fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateFixTemplateAsync_FixTemplateIdNotFound_ReturnsNotFoundObjectResult(string userId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var fakeFixTemplateUpdateRequestDto = _fakeDtoSeederFactory.CreateSeederFactory(new FixTemplateUpdateRequestDto()).First();
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(fakeFixTemplateUpdateRequestDto);
      var updateFixTemplateFunction = new UpdateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateUpdateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await updateFixTemplateFunction.UpdateFixTemplateAsync(userIdGuid, fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    [DataRow("00000000-0000-0000-0000-000000000000", DisplayName = "Any_FixTemplateId")]
    public async Task UpdateFixTemplateAsync_FixTemplateIdNotValid_ReturnsBadRequestObjectResult(string userId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var fakeFixTemplateUpdateRequestDto = _fakeDtoSeederFactory.CreateSeederFactory(new FixTemplateUpdateRequestDto()).First();
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(fakeFixTemplateUpdateRequestDto);
      var updateFixTemplateFunction = new UpdateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateUpdateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await updateFixTemplateFunction.UpdateFixTemplateAsync(userIdGuid, fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateFixTemplateAsync_RequestBodyNotValid_ReturnsBadRequestObjectResult(string userId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var fakeHttpRequestMessage = _fakeHttpRequestMessageAdapterBase.GetFakeHttpRequestMessage(default(FixTemplateUpdateRequestDto));
      var updateFixTemplateFunction = new UpdateFixTemplate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateUpdateRequestDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await updateFixTemplateFunction.UpdateFixTemplateAsync(userIdGuid, fakeHttpRequestMessage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region UpdateFixTemplateStatusAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", "Public", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateFixTemplateStatusAsync_FixTemplateIdFoundWithValidStatus_ReturnsOkObjectResult(string userId, string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var updateFixTemplateStatusFunction = new UpdateFixTemplateStatus(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateStatus>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await updateFixTemplateStatusFunction.UpdateFixTemplateStatusAsync(userIdGuid, status, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", "Popular", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateFixTemplateStatusAsync_FixTemplateIdFoundWithInvalidStatus_ReturnsBadRequestObjectResult(string userId, string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var updateFixTemplateStatusFunction = new UpdateFixTemplateStatus(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateStatus>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await updateFixTemplateStatusFunction.UpdateFixTemplateStatusAsync(userIdGuid, status, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", "Public", DisplayName = "Any_FixTemplateId")]
    public async Task UpdateFixTemplateStatusAsync_FixTemplateIdNotFoundWithValidStatus_ReturnsBadRequestObjectResult(string userId, string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var updateFixTemplateStatusFunction = new UpdateFixTemplateStatus(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<FixTemplateStatus>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await updateFixTemplateStatusFunction.UpdateFixTemplateStatusAsync(userIdGuid, status, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region UpdateSystemCostEstimateAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", 123.45, DisplayName = "Known_FixTemplateId")]
    public async Task UpdateSystemCostEstimateAsync_FixTemplateIdFoundWithValidCost_ReturnsOkObjectResult(string userId, double cost)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var updateSystemCostEstimateFunction = new UpdateSystemCostEstimate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateCostAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await updateSystemCostEstimateFunction.UpdateSystemCostEstimateAsync(userIdGuid, cost, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", -123.45, DisplayName = "Known_FixTemplateId")]
    public async Task UpdateSystemCostEstimateAsync_FixTemplateIdFoundWithInvalidCost_ReturnsBadRequestObjectResult(string userId, double cost)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var updateSystemCostEstimateFunction = new UpdateSystemCostEstimate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateCostAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeFixTemplates.First());

      // Act
      var actionResult = await updateSystemCostEstimateFunction.UpdateSystemCostEstimateAsync(userIdGuid, cost, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", 123.45, DisplayName = "Any_FixTemplateId")]
    public async Task UpdateSystemCostEstimateAsync_FixTemplateIdNotFoundWithValidCost_ReturnsNotFoundObjectResult(string userId, double cost)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var userIdGuid = new Guid(userId);
      var updateSystemCostEstimateFunction = new UpdateSystemCostEstimate(_fakeRequestMediatorFactory.Object);

      _fixTemplateMediator.Setup(fixTemplateMediator => fixTemplateMediator.UpdateCostAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await updateSystemCostEstimateFunction.UpdateSystemCostEstimateAsync(userIdGuid, cost, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _fixTemplateMediator.Reset();
      _fakeConfiguration.Reset();
      _fakeRequestMediatorFactory.Reset();

      // Clean-up data objects
      _fakeFixTemplates = null;
    }
    #endregion
  }
}

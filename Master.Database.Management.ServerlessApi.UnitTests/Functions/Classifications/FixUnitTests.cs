using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Functions.Classifications.Units;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions.Classifications
{
  [TestClass]
  public class FixUnitTests : TestBase
  {
    private Mock<IFixUnitMediator> _fixUnitMediator;

    // Fake Data
    private IEnumerable<FixUnitDto> _fakeFixUnits;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _fixUnitMediator = new Mock<IFixUnitMediator>();

      _fakeFixUnits = _fakeDtoSeederFactory.CreateSeederFactory(new FixUnitDto());

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestFixUnitMediator()).Returns(_fixUnitMediator.Object);
    }

    #region GetFixUnitByIdAsyncTests
    [TestMethod]
    [DataRow("727012a4-773c-4994-99c9-0ff83d9e8734", DisplayName = "Known_TypeId")]
    public async Task GetFixUnitByIdAsync_TypeIdFound_ReturnsOkObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixUnitIdGuid = new Guid(typeId);

      var GetUnitByIdFunction = new GetUnitById(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeFixUnits.Where(fakeFixUnit => fakeFixUnit.Id.Equals(fixUnitIdGuid)).First());

      // Act
      var actionResult = await GetUnitByIdFunction.GetUnitByIdAsync(fixUnitIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetFixUnitByIdAsync_TypeIdNotFound_ReturnsNotFoundObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixUnitIdGuid = new Guid(typeId);

      var GetUnitByIdFunction = new GetUnitById(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(default(FixUnitDto));

      // Act
      var actionResult = await GetUnitByIdFunction.GetUnitByIdAsync(fixUnitIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetUnitsAsyncTests
    [TestMethod]
    public async Task GetUnitsAsync_FiltersFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var GetUnitsFunction = new GetUnits(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeFixUnits);

      // Act
      var actionResult = await GetUnitsFunction.GetUnitsAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetUnitsAsync_FiltersNotFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";
      var GetUnitsFunction = new GetUnits(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new List<FixUnitDto>());

      // Act
      var actionResult = await GetUnitsFunction.GetUnitsAsync(cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<FixUnitDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    public async Task GetUnitsAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var GetUnitsFunction = new GetUnits(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeFixUnits);

      // Act
      var actionResult = await GetUnitsFunction.GetUnitsAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region GetPagedUnitsAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedUnitsAsync_FiltersFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var pagedModelCollectionDto = new PagedModelCollectionDto<FixUnitDto>()
      {
        Results = _fakeFixUnits.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeFixUnits.Count()
      };

      var GetPagedUnitsFunction = new GetPagedUnits(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await GetPagedUnitsFunction.GetPagedUnitsAsync(pageSize, pageNumber, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedUnitsAsync_FiltersNotFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";
      var pageSize = 0;

      var GetPagedUnitsFunction = new GetPagedUnits(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new PagedModelCollectionDto<FixUnitDto>());

      // Act
      var actionResult = await GetPagedUnitsFunction.GetPagedUnitsAsync(pageSize, pageNumber, cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<FixUnitDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Results.Any());
      Assert.IsTrue(objectResultValue.PageNumber == 0);
      Assert.IsTrue(objectResultValue.TotalModelCount == 0);
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedUnitsAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var GetPagedUnitsFunction = new GetPagedUnits(_fakeRequestMediatorFactory.Object);

      _fixUnitMediator.Setup(fixUnitMediator => fixUnitMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new PagedModelCollectionDto<FixUnitDto>());

      // Act
      var actionResult = await GetPagedUnitsFunction.GetPagedUnitsAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _fixUnitMediator.Reset();
      _fakeConfiguration.Reset();
      _fakeRequestMediatorFactory.Reset();
      _fixUnitMediator.Reset();

      // Clean-up data objects
      _fakeFixUnits = null;
    }
    #endregion

  }
}

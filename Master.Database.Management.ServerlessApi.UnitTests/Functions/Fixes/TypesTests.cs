using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Types;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Functions.Fixes.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions.Fixes
{
  [TestClass]
  public class TypesTests : TestBase
  {
    private Mock<IFixTypeMediator> _fixTypeMediator;

    // Fake Data
    private IEnumerable<FixTypeDto> _fakeFixTypes;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _fixTypeMediator = new Mock<IFixTypeMediator>();

      _fakeFixTypes = _fakeDtoSeederFactory.CreateFakeSeeder<FixTypeDto>().SeedFakeDtos();

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestFixTypeMediator()).Returns(_fixTypeMediator.Object);
    }

    #region GetTypeByIdAsyncTests
    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_TypeId")]
    public async Task GetTypeByIdAsync_TypeIdFound_ReturnsOkObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid typeIdGuid = new Guid(typeId);

      var getTypeByIdFunction = new GetTypeById(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeFixTypes.Where(fakeFixType => fakeFixType.Id.Equals(typeIdGuid)).First());

      // Act
      var actionResult = await getTypeByIdFunction.GetTypeByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetTypeByIdAsync_TypeIdNotFound_ReturnsNotFoundObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid typeIdGuid = new Guid(typeId);

      var getTypeByIdFunction = new GetTypeById(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(default(FixTypeDto));

      // Act
      var actionResult = await getTypeByIdFunction.GetTypeByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetFixTypesAsyncTests
    [TestMethod]
    public async Task GetFixTypesAsync_FiltersFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      var getTypesFunction = new GetTypes(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetManyAsync(It.IsAny<CancellationToken>(), null, null, null))
                      .ReturnsAsync(_fakeFixTypes);

      // Act
      var actionResult = await getTypesFunction.GetFixTypesAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetFixTypesAsync_FiltersNotFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";

      var getTypesFunction = new GetTypes(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetManyAsync(It.IsAny<CancellationToken>(), It.IsAny<string>(), null, null))
                      .ReturnsAsync(new List<FixTypeDto>());

      // Act
      var actionResult = await getTypesFunction.GetFixTypesAsync(cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<FixTypeDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    public async Task GetFixTypesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getTypesFunction = new GetTypes(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetManyAsync(It.IsAny<CancellationToken>(), null, It.IsAny<long>(), It.IsAny<long>()))
                      .ReturnsAsync(_fakeFixTypes);

      // Act
      var actionResult = await getTypesFunction.GetFixTypesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region GetPagedTypesAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedTypesAsync_FiltersFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var pagedModelCollectionDto = new PagedModelCollectionDto<FixTypeDto>()
      {
        Results = _fakeFixTypes.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeFixTypes.Count()
      };

      var getPagedTypesFunction = new GetPagedTypes(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetManyByPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>(), null, null, null))
                      .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await getPagedTypesFunction.GetPagedTypesAsync(pageSize, pageNumber, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedTypesAsync_FiltersNotFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";
      var pageSize = 0;

      var getPagedTypesFunction = new GetPagedTypes(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetManyByPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>(), null, null))
                      .ReturnsAsync(new PagedModelCollectionDto<FixTypeDto>());

      // Act
      var actionResult = await getPagedTypesFunction.GetPagedTypesAsync(pageSize, pageNumber, cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<FixTypeDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Results.Any());
      Assert.IsTrue(objectResultValue.PageNumber == 0);
      Assert.IsTrue(objectResultValue.TotalModelCount == 0);
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedTypesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getPagedTypesFunction = new GetPagedTypes(_fakeRequestMediatorFactory.Object);

      _fixTypeMediator.Setup(fixTypeMediator => fixTypeMediator.GetManyByPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>(), null, It.IsAny<long>(), It.IsAny<long>()))
                      .ReturnsAsync(new PagedModelCollectionDto<FixTypeDto>());

      // Act
      var actionResult = await getPagedTypesFunction.GetPagedTypesAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion
  }
}

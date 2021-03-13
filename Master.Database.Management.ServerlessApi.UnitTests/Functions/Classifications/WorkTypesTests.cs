using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Functions.Classifications.Types;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions.Classifications
{
  [TestClass]
  public class WorkTypesTests : TestBase
  {
    private Mock<IWorkTypeMediator> _workTypeMediator;

    // Fake Data
    private IEnumerable<WorkTypeDto> _fakeWorkTypes;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _workTypeMediator = new Mock<IWorkTypeMediator>();

      _fakeWorkTypes = _fakeDtoSeederFactory.CreateSeederFactory(new WorkTypeDto());

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestWorkTypeMediator()).Returns(_workTypeMediator.Object);
    }

    #region GetTypeByIdAsyncTests
    [TestMethod]
    [DataRow("96371910-43e3-4621-98c2-2396cd663e0c", DisplayName = "Known_TypeId")]
    public async Task GetTypeByIdAsync_TypeIdFound_ReturnsOkObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      var getTypeByIdFunction = new GetTypeById(_fakeRequestMediatorFactory.Object);

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeWorkTypes.Where(fakeWorkType => fakeWorkType.Id.Equals(typeIdGuid)).First());

      // Act
      var actionResult = await getTypeByIdFunction.GetWorkTypeByIdAsync(typeIdGuid, cancellationToken);

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
      var typeIdGuid = new Guid(typeId);

      var getTypeByIdFunction = new GetTypeById(_fakeRequestMediatorFactory.Object);

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(default(WorkTypeDto));

      // Act
      var actionResult = await getTypeByIdFunction.GetWorkTypeByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetWorkTypesAsyncTests
    [TestMethod]
    public async Task GetWorkTypesAsync_FiltersFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      var getTypesFunction = new GetTypes(_fakeRequestMediatorFactory.Object);

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeWorkTypes);

      // Act
      var actionResult = await getTypesFunction.GetWorkTypesAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetWorkTypesAsync_FiltersNotFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";

      var getTypesFunction = new GetTypes(_fakeRequestMediatorFactory.Object);

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new List<WorkTypeDto>());

      // Act
      var actionResult = await getTypesFunction.GetWorkTypesAsync(cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<WorkTypeDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    public async Task GetWorkTypesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getTypesFunction = new GetTypes(_fakeRequestMediatorFactory.Object);

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(_fakeWorkTypes);

      // Act
      var actionResult = await getTypesFunction.GetWorkTypesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

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
      var pagedModelCollectionDto = new PagedModelCollectionDto<WorkTypeDto>()
      {
        Results = _fakeWorkTypes.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeWorkTypes.Count()
      };

      var getPagedTypesFunction = new GetPagedTypes(_fakeRequestMediatorFactory.Object);

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await getPagedTypesFunction.GetPagedWorkTypesAsync(pageSize, pageNumber, cancellationToken);

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

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new PagedModelCollectionDto<WorkTypeDto>());

      // Act
      var actionResult = await getPagedTypesFunction.GetPagedWorkTypesAsync(pageSize, pageNumber, cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<WorkTypeDto> objectResultValue));
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

      _workTypeMediator.Setup(workTypeMediator => workTypeMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new PagedModelCollectionDto<WorkTypeDto>());

      // Act
      var actionResult = await getPagedTypesFunction.GetPagedWorkTypesAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

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
      _workTypeMediator.Reset();
      _fakeConfiguration.Reset();
      _fakeRequestMediatorFactory.Reset();

      // Clean-up data objects
      _fakeWorkTypes = null;
    }
    #endregion
  }
}

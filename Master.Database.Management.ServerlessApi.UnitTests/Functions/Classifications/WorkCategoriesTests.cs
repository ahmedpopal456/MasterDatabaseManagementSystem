using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Functions.Classifications.Categories;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions.Classifications
{
  [TestClass]
  public class WorkCategoriesTests : TestBase
  {
    private Mock<IWorkCategoryMediator> _fakeWorkCategoryMediator;

    // Fake Data
    private IEnumerable<WorkCategoryDto> _fakeWorkCategories;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _fakeWorkCategoryMediator = new Mock<IWorkCategoryMediator>();

      _fakeWorkCategories = _fakeDtoSeederFactory.CreateSeederFactory(new WorkCategoryDto());

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestWorkCategoryMediator()).Returns(_fakeWorkCategoryMediator.Object);
    }

    #region GetWorkCategoryByIdAsyncTests
    [TestMethod]
    [DataRow("96371910-43e3-4621-98c2-2396cd663e0c", DisplayName = "Known_CategoryId")]
    public async Task GetWorkCategoryByIdAsync_CategoryIdFound_ReturnsOkObjectResult(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var categoryIdGuid = new Guid(categoryId);

      var getCategoryByIdFunction = new GetCategoryById(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeWorkCategories.Where(fakeWorkCategory => fakeWorkCategory.Id.Equals(categoryIdGuid)).First());

      // Act
      var actionResult = await getCategoryByIdFunction.GetWorkCategoryByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_CategoryId")]
    public async Task GetWorkCategoryByIdAsync_CategoryIdNotFound_ReturnsNotFoundObjectResult(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var categoryIdGuid = new Guid(categoryId);

      var getCategoryByIdFunction = new GetCategoryById(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(default(WorkCategoryDto));

      // Act
      var actionResult = await getCategoryByIdFunction.GetWorkCategoryByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetWorkCategoriesAsyncTests
    [TestMethod]
    public async Task GetWorkCategoriesAsync_FiltersFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "1609618205";
      var maxTimestampUtcString = "1609636182";

      var getCategoriesFunction = new GetCategories(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(_fakeWorkCategories.Take(2));

      // Act
      var actionResult = await getCategoriesFunction.GetWorkCategoriesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetWorkCategoriesAsync_FiltersNotFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "1609618205";
      var maxTimestampUtcString = "1609636182";

      var getCategoriesFunction = new GetCategories(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<WorkCategoryDto>());

      // Act
      var actionResult = await getCategoriesFunction.GetWorkCategoriesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<WorkCategoryDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    public async Task GetWorkCategoriesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getCategoriesFunction = new GetCategories(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<WorkCategoryDto>());

      // Act
      var actionResult = await getCategoriesFunction.GetWorkCategoriesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region GetPagedWorkCategoriesAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedWorkCategoriesAsync_FiltersFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var pagedModelCollectionDto = new PagedModelCollectionDto<WorkCategoryDto>()
      {
        Results = _fakeWorkCategories.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeWorkCategories.Count()
      };

      var getPagedCategoriesFunction = new GetPagedCategories(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await getPagedCategoriesFunction.GetPagedWorkCategoriesAsync(pageSize, pageNumber, cancellationToken, null, null, null);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedWorkCategoriesAsync_FiltersNotFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "1609618205";
      var maxTimestampUtcString = "1609636182";

      var getPagedCategoriesFunction = new GetPagedCategories(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new PagedModelCollectionDto<WorkCategoryDto>());

      // Act
      var actionResult = await getPagedCategoriesFunction.GetPagedWorkCategoriesAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<WorkCategoryDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Results.Any());
      Assert.IsTrue(objectResultValue.PageNumber == 0);
      Assert.IsTrue(objectResultValue.TotalModelCount == 0);
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedWorkCategoriesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getPagedCategoriesFunction = new GetPagedCategories(_fakeRequestMediatorFactory.Object);

      _fakeWorkCategoryMediator.Setup(workCategoryMediator => workCategoryMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new PagedModelCollectionDto<WorkCategoryDto>());

      // Act
      var actionResult = await getPagedCategoriesFunction.GetPagedWorkCategoriesAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

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
      _fakeWorkCategoryMediator.Reset();
      _fakeConfiguration.Reset();
      _fakeRequestMediatorFactory.Reset();

      // Clean-up data objects
      _fakeWorkCategories = null;
    }
    #endregion
  }
}

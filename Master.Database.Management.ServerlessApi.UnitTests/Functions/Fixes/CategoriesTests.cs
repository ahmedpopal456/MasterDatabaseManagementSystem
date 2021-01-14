using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Master.Database.Management.ServerlessApi.Functions.Fixes.Templates;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Fixit.Core.DataContracts.Fixes.Categories;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions.Fixes
{
  [TestClass]
  public class CategoriesTests : TestBase
  {
    private Mock<IFixCategoryMediator> _fakeFixCategoryMediator;

    // Fake Data
    private IEnumerable<FixCategoryDto> _fakeFixCategories;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _fakeFixCategoryMediator = new Mock<IFixCategoryMediator>();

      _fakeFixCategories = _fakeDtoSeederFactory.CreateFakeSeeder<FixCategoryDto>().SeedFakeDtos();

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestFixCategoryMediator()).Returns(_fakeFixCategoryMediator.Object);
    }

    #region GetFixCategoryByIdAsyncTests
    [TestMethod]
    [DataRow("96371910-43e3-4621-98c2-2396cd663e0c", DisplayName = "Known_CategoryId")]
    public async Task GetFixCategoryByIdAsync_CategoryIdFound_ReturnsOkObjectResult(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid categoryIdGuid = new Guid(categoryId);

      var getCategoryByIdFunction = new GetCategoryById(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(_fakeFixCategories.Where(fakeFixCategory => fakeFixCategory.Id.Equals(categoryIdGuid)).First());

      // Act
      var actionResult = await getCategoryByIdFunction.GetFixCategoryByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_CategoryId")]
    public async Task GetFixCategoryByIdAsync_CategoryIdNotFound_ReturnsNotFoundObjectResult(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid categoryIdGuid = new Guid(categoryId);

      var getCategoryByIdFunction = new GetCategoryById(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(default(FixCategoryDto));

      // Act
      var actionResult = await getCategoryByIdFunction.GetFixCategoryByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetFixCategoriesAsyncTests
    [TestMethod]
    public async Task GetFixCategoriesAsync_FiltersFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "1609618205";
      var maxTimestampUtcString = "1609636182";

      var getCategoriesFunction = new GetCategories(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetManyAsync(It.IsAny<CancellationToken>(), null, It.IsAny<long>(), It.IsAny<long>()))
                              .ReturnsAsync(_fakeFixCategories.Take(2));

      // Act
      var actionResult = await getCategoriesFunction.GetFixCategoriesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetFixCategoriesAsync_FiltersNotFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "1609618205";
      var maxTimestampUtcString = "1609636182";

      var getCategoriesFunction = new GetCategories(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetManyAsync(It.IsAny<CancellationToken>(), null, It.IsAny<long>(), It.IsAny<long>()))
                              .ReturnsAsync(new List<FixCategoryDto>());

      // Act
      var actionResult = await getCategoriesFunction.GetFixCategoriesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<FixCategoryDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    public async Task GetFixCategoriesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getCategoriesFunction = new GetCategories(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetManyAsync(It.IsAny<CancellationToken>(), null, It.IsAny<long>(), It.IsAny<long>()))
                              .ReturnsAsync(new List<FixCategoryDto>());

      // Act
      var actionResult = await getCategoriesFunction.GetFixCategoriesAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region GetPagedFixCategoriesAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedFixCategoriesAsync_FiltersFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var pagedModelCollectionDto = new PagedModelCollectionDto<FixCategoryDto>()
      {
        Results = _fakeFixCategories.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeFixCategories.Count()
      };

      var getPagedCategoriesFunction = new GetPagedCategories(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetManyByPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>(), null, null, null))
                              .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await getPagedCategoriesFunction.GetPagedFixCategoriesAsync(pageSize, pageNumber, cancellationToken, null, null, null);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedFixCategoriesAsync_FiltersNotFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "1609618205";
      var maxTimestampUtcString = "1609636182";

      var getPagedCategoriesFunction = new GetPagedCategories(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetManyByPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>(), null, It.IsAny<long>(), It.IsAny<long>()))
                              .ReturnsAsync(new PagedModelCollectionDto<FixCategoryDto>());

      // Act
      var actionResult = await getPagedCategoriesFunction.GetPagedFixCategoriesAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<FixCategoryDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Results.Any());
      Assert.IsTrue(objectResultValue.PageNumber == 0);
      Assert.IsTrue(objectResultValue.TotalModelCount == 0);
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedFixCategoriesAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var getPagedCategoriesFunction = new GetPagedCategories(_fakeRequestMediatorFactory.Object);

      _fakeFixCategoryMediator.Setup(fixCategoryMediator => fixCategoryMediator.GetManyByPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>(), null, It.IsAny<long>(), It.IsAny<long>()))
                              .ReturnsAsync(new PagedModelCollectionDto<FixCategoryDto>());

      // Act
      var actionResult = await getPagedCategoriesFunction.GetPagedFixCategoriesAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion
  }
}

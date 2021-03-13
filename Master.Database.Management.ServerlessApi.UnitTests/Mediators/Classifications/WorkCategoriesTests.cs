using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Mediators.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Mediators.Classifications
{
  [TestClass]
  public class WorkCategoriesTests : TestBase
  {
    private Mock<IMdmWorkCategoryDal> _requestMdmWorkCategoryDal;

    private IWorkCategoryMediator _workCategoryMediator;

    // Fake Data
    private IEnumerable<WorkCategoryDto> _fakeWorkCategories;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmWorkCategoryDal = new Mock<IMdmWorkCategoryDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeWorkCategories = _fakeDtoSeederFactory.CreateSeederFactory(new WorkCategoryDto());

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmWorkCategoryDal()).Returns(_requestMdmWorkCategoryDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _workCategoryMediator = _requestMediatorFactory.RequestWorkCategoryMediator();
    }

    #region GetWorkCategoryByIdAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_CategoryId")]
    public async Task GetFirstOrDefaultAsync_CategoryIdNotFound_ReturnsDefault(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var categoryIdGuid = new Guid(categoryId);

      _requestMdmWorkCategoryDal.Setup(requestMdmWorkCategoryDal => requestMdmWorkCategoryDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(default(WorkCategoryDto));

      // Act
      var actionResult = await _workCategoryMediator.GetByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("51a1e09e-bf7e-48d1-ac51-538d6f1bb957", DisplayName = "Known_CategoryId")]
    public async Task GetFirstOrDefaultAsync_CategoryIdFound_ReturnsWorkCategoryDto(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var categoryIdGuid = new Guid(categoryId);

      _requestMdmWorkCategoryDal.Setup(requestMdmWorkCategoryDal => requestMdmWorkCategoryDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeWorkCategories.Where(fakeWorkCategory => fakeWorkCategory.Id.Equals(categoryIdGuid)).First());

      // Act
      var actionResult = await _workCategoryMediator.GetByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("Electricity", DisplayName = "Known_FilterByName")]
    public async Task GetManyAsync_CategoryNameFound_ReturnsMatchedWorkCategoryDtos(string name)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = name };
      _requestMdmWorkCategoryDal.Setup(requestMdmWorkCategoryDal => requestMdmWorkCategoryDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeWorkCategories.Where(fakeWorkCategory => fakeWorkCategory.Name.Equals(name)).ToList());

      // Act
      var actionResult = await _workCategoryMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Any());
    }

    [TestMethod]
    [DataRow("Backyard", DisplayName = "Any_FilterByName")]
    public async Task GetManyAsync_CategoryNameNotFound_ReturnsDefault(string name)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = name };
      _requestMdmWorkCategoryDal.Setup(requestMdmWorkCategoryDal => requestMdmWorkCategoryDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new List<WorkCategoryDto>());

      // Act
      var actionResult = await _workCategoryMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 0);
    }

    [TestMethod]
    [DataRow(DisplayName = "Any_Filter")]
    public async Task GetManyAsync_FilterNotSpecified_ReturnsAllWorkCategoryDtos()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto();
      _requestMdmWorkCategoryDal.Setup(requestMdmWorkCategoryDal => requestMdmWorkCategoryDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeWorkCategories);

      // Act
      var actionResult = await _workCategoryMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeWorkCategories.Count());
    }
    #endregion

    #region GetManyByPageAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<WorkCategoryDto>()
      {
        Results = _fakeWorkCategories.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeWorkCategories.Count()
      };
      var paginationRequestBaseDto = new PaginationRequestBaseDto { PageNumber = currentPage };
      _requestMdmWorkCategoryDal.Setup(requestMdmWorkCategoryDal => requestMdmWorkCategoryDal.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _workCategoryMediator.GetManyByPageAsync(paginationRequestBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.Results);
    }
    #endregion

    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _requestMdmWorkCategoryDal.Reset();

      // Clean-up data objects
      _fakeWorkCategories = null;
      _workCategoryMediator = null;
    }
    #endregion
  }
}

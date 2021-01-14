using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Categories;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Mediators.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Mediators.Fixes
{
  [TestClass]
  public class CategoriesTests : TestBase
  {
    private Mock<IMdmFixCategoryDal> _requestMdmFixCategoryDal;

    private IFixCategoryMediator _fixCategoryMediator;

    // Fake Data
    private IEnumerable<FixCategoryDto> _fakeFixCategories;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmFixCategoryDal = new Mock<IMdmFixCategoryDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeFixCategories = _fakeDtoSeederFactory.CreateFakeSeeder<FixCategoryDto>().SeedFakeDtos();

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmFixCategoryDal()).Returns(_requestMdmFixCategoryDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _fixCategoryMediator = _requestMediatorFactory.RequestFixCategoryMediator();
    }

    #region GetFixCategoryByIdAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_CategoryId")]
    public async Task GetFirstOrDefaultAsync_CategoryIdNotFound_ReturnsDefault(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid categoryIdGuid = new Guid(categoryId);

      _requestMdmFixCategoryDal.Setup(requestMdmFixCategoryDal => requestMdmFixCategoryDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(default(FixCategoryDto));

      // Act
      var actionResult = await _fixCategoryMediator.GetByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("51a1e09e-bf7e-48d1-ac51-538d6f1bb957", DisplayName = "Known_CategoryId")]
    public async Task GetFirstOrDefaultAsync_CategoryIdFound_ReturnsFixCategoryDto(string categoryId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid categoryIdGuid = new Guid(categoryId);

      _requestMdmFixCategoryDal.Setup(requestMdmFixCategoryDal => requestMdmFixCategoryDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeFixCategories.Where(fakeFixCategory => fakeFixCategory.Id.Equals(categoryIdGuid)).First());

      // Act
      var actionResult = await _fixCategoryMediator.GetByIdAsync(categoryIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("Bedroom", DisplayName = "Known_FilterByName")]
    public async Task GetManyAsync_CategoryNameFound_ReturnsMatchedFixCategoryDtos(string name)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _requestMdmFixCategoryDal.Setup(requestMdmFixCategoryDal => requestMdmFixCategoryDal.GetManyAsync(It.IsAny<CancellationToken>(), It.IsAny<string>(), null, null))
                               .ReturnsAsync(_fakeFixCategories.Where(fakeFixCategory => fakeFixCategory.Name.Equals(name)).ToList());
      
      // Act
      var actionResult = await _fixCategoryMediator.GetManyAsync(cancellationToken, name);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 1);
    }

    [TestMethod]
    [DataRow("Backyard", DisplayName = "Any_FilterByName")]
    public async Task GetManyAsync_CategoryNameNotFound_ReturnsDefault(string name)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _requestMdmFixCategoryDal.Setup(requestMdmFixCategoryDal => requestMdmFixCategoryDal.GetManyAsync(It.IsAny<CancellationToken>(), It.IsAny<string>(), null, null))
                               .ReturnsAsync(new List<FixCategoryDto>());

      // Act
      var actionResult = await _fixCategoryMediator.GetManyAsync(cancellationToken, name);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 0);
    }

    [TestMethod]
    [DataRow(DisplayName = "Any_Filter")]
    public async Task GetManyAsync_FilterNotSpecified_ReturnsAllFixCategoryDtos()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _requestMdmFixCategoryDal.Setup(requestMdmFixCategoryDal => requestMdmFixCategoryDal.GetManyAsync(It.IsAny<CancellationToken>(),null, null, null))
                               .ReturnsAsync(_fakeFixCategories);

      // Act
      var actionResult = await _fixCategoryMediator.GetManyAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeFixCategories.Count());
    }
    #endregion

    #region GetManyByPageAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<FixCategoryDto>()
      { 
        Results = _fakeFixCategories.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeFixCategories.Count()
      };

      _requestMdmFixCategoryDal.Setup(requestMdmFixCategoryDal => requestMdmFixCategoryDal.GetManyByPageAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), null, null, null, null))
                               .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _fixCategoryMediator.GetManyByPageAsync(currentPage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.Results);
    }
    #endregion
  }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Classifications;
using Master.Database.Management.DataLayer.DataAccess;
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
  public class WorkTypesTests : TestBase
  {
    private Mock<IMdmWorkTypeDal> _requestMdmWorkTypeDal;

    private IWorkTypeMediator _workTypeMediator;

    // Fake Data
    private IEnumerable<WorkTypeDto> _fakeWorkTypes;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmWorkTypeDal = new Mock<IMdmWorkTypeDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeWorkTypes = _fakeDtoSeederFactory.CreateSeederFactory(new WorkTypeDto());

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmWorkTypeDal()).Returns(_requestMdmWorkTypeDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _workTypeMediator = _requestMediatorFactory.RequestWorkTypeMediator();
    }

    #region GetFirstOrDefaultAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdNotFound_ReturnsDefault(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      _requestMdmWorkTypeDal.Setup(requestMdmWorkTypeDal => requestMdmWorkTypeDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(default(WorkTypeDto));

      // Act
      var actionResult = await _workTypeMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("96371910-43e3-4621-98c2-2396cd663e0c", DisplayName = "Known_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdFound_ReturnsWorkTypeDto(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      _requestMdmWorkTypeDal.Setup(requestMdmWorkTypeDal => requestMdmWorkTypeDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeWorkTypes.Where(fakeWorkType => fakeWorkType.Id.Equals(typeIdGuid)).First());

      // Act
      var actionResult = await _workTypeMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("Quick Fix", DisplayName = "Known_FilterByName")]
    public async Task GetFirstOrDefaultAsync_TypeNameFound_ReturnsMatchedWorkTypeDtos(string typeName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = typeName };
      _requestMdmWorkTypeDal.Setup(requestMdmWorkTypeDal => requestMdmWorkTypeDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeWorkTypes.Where(fakeWorkType => fakeWorkType.Name.Equals(typeName)).ToList());

      // Act
      var actionResult = await _workTypeMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 1);
    }

    [TestMethod]
    [DataRow("Old", DisplayName = "Any_FilterByName")]
    public async Task GetManyAsync_TypeNameNotFound_ReturnsDefault(string typeName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = typeName };
      _requestMdmWorkTypeDal.Setup(requestMdmWorkTypeDal => requestMdmWorkTypeDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new List<WorkTypeDto>());

      // Act
      var actionResult = await _workTypeMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 0);
    }

    [TestMethod]
    [DataRow(DisplayName = "Any_Filter")]
    public async Task GetManyAsync_FilterNotSpecified_ReturnsAllWorkTypeDtos()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto();
      _requestMdmWorkTypeDal.Setup(requestMdmWorkTypeDal => requestMdmWorkTypeDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeWorkTypes);

      // Act
      var actionResult = await _workTypeMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeWorkTypes.Count());
    }
    #endregion

    #region GetManyByPageAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<WorkTypeDto>()
      {
        Results = _fakeWorkTypes.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeWorkTypes.Count()
      };
      var paginationRequestBaseDto = new PaginationRequestBaseDto { PageNumber = currentPage };
      _requestMdmWorkTypeDal.Setup(requestMdmWorkTypeDal => requestMdmWorkTypeDal.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _workTypeMediator.GetManyByPageAsync(paginationRequestBaseDto, cancellationToken);

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
      _requestMdmWorkTypeDal.Reset();

      // Clean-up data objects
      _fakeWorkTypes = null;
      _workTypeMediator = null;
    }
    #endregion
  }
}

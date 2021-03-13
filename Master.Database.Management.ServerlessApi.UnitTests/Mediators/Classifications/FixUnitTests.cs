using System;
using System.Collections.Generic;
using System.Linq;
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
  public class FixUnitTests : TestBase
  {
    private Mock<IMdmFixUnitDal> _requestMdmFixUnitDal;

    private IFixUnitMediator _fixUnitMediator;

    // Fake Data
    private IEnumerable<FixUnitDto> _fakeFixUnits;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmFixUnitDal = new Mock<IMdmFixUnitDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeFixUnits = _fakeDtoSeederFactory.CreateSeederFactory(new FixUnitDto());

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmFixUnitDal()).Returns(_requestMdmFixUnitDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _fixUnitMediator = _requestMediatorFactory.RequestFixUnitMediator();
    }

    #region GetFirstOrDefaultAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdNotFound_ReturnsDefault(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      _requestMdmFixUnitDal.Setup(RequestMdmFixUnitDal => RequestMdmFixUnitDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(default(FixUnitDto));

      // Act
      var actionResult = await _fixUnitMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("727012a4-773c-4994-99c9-0ff83d9e8734", DisplayName = "Known_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdFound_ReturnsFixUnitDto(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      _requestMdmFixUnitDal.Setup(RequestMdmFixUnitDal => RequestMdmFixUnitDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeFixUnits.Where(fakeFixUnit => fakeFixUnit.Id.Equals(typeIdGuid)).First());

      // Act
      var actionResult = await _fixUnitMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("Living Room", DisplayName = "Known_FilterByName")]
    public async Task GetFirstOrDefaultAsync_TypeNameFound_ReturnsMatchedFixUnitDtos(string typeName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = typeName };
      _requestMdmFixUnitDal.Setup(RequestMdmFixUnitDal => RequestMdmFixUnitDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeFixUnits.Where(fakeFixUnit => fakeFixUnit.Name.Equals(typeName)).ToList());

      // Act
      var actionResult = await _fixUnitMediator.GetManyAsync(filterBaseDto, cancellationToken);

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
      _requestMdmFixUnitDal.Setup(RequestMdmFixUnitDal => RequestMdmFixUnitDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new List<FixUnitDto>());

      // Act
      var actionResult = await _fixUnitMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 0);
    }

    [TestMethod]
    [DataRow(DisplayName = "Any_Filter")]
    public async Task GetManyAsync_FilterNotSpecified_ReturnsAllFixUnitDtos()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto();
      _requestMdmFixUnitDal.Setup(RequestMdmFixUnitDal => RequestMdmFixUnitDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeFixUnits);

      // Act
      var actionResult = await _fixUnitMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeFixUnits.Count());
    }
    #endregion

    #region GetManyByPageAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<FixUnitDto>()
      {
        Results = _fakeFixUnits.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeFixUnits.Count()
      };
      var paginationRequestBaseDto = new PaginationRequestBaseDto { PageNumber = currentPage };
      _requestMdmFixUnitDal.Setup(RequestMdmFixUnitDal => RequestMdmFixUnitDal.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _fixUnitMediator.GetManyByPageAsync(paginationRequestBaseDto, cancellationToken);

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
      _requestMdmFixUnitDal.Reset();

      // Clean-up data objects
      _fakeFixUnits = null;
      _fixUnitMediator = null;
    }
    #endregion
  }
}

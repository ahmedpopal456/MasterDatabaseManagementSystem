using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Types;
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
  public class TypesTests : TestBase
  {
    private Mock<IMdmFixTypeDal> _requestMdmFixTypeDal;

    private IFixTypeMediator _fixTypeMediator;

    // Fake Data
    private IEnumerable<FixTypeDto> _fakeFixTypes;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmFixTypeDal = new Mock<IMdmFixTypeDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeFixTypes = _fakeDtoSeederFactory.CreateFakeSeeder<FixTypeDto>().SeedFakeDtos();

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmFixTypeDal()).Returns(_requestMdmFixTypeDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _fixTypeMediator = _requestMediatorFactory.RequestFixTypeMediator();
    }

    #region GetFirstOrDefaultAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdNotFound_ReturnsDefault(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid typeIdGuid = new Guid(typeId);

      _requestMdmFixTypeDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(default(FixTypeDto));

      // Act
      var actionResult = await _fixTypeMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("445e50d1-b2e7-4c25-a628-c610aed7a357", DisplayName = "Known_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdFound_ReturnsFixTypeDto(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid typeIdGuid = new Guid(typeId);

      _requestMdmFixTypeDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(_fakeFixTypes.Where(fakeFixType => fakeFixType.Id.Equals(typeIdGuid)).First());

      // Act
      var actionResult = await _fixTypeMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("New", DisplayName = "Known_FilterByName")]
    public async Task GetFirstOrDefaultAsync_TypeNameFound_ReturnsMatchedFixTypeDtos(string typeName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _requestMdmFixTypeDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>(), null, null))
                           .ReturnsAsync(_fakeFixTypes.Where(fakeFixType => fakeFixType.Name.Equals(typeName)).ToList());

      // Act
      var actionResult = await _fixTypeMediator.GetManyAsync(cancellationToken, typeName);

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

      _requestMdmFixTypeDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>(), null, null))
                           .ReturnsAsync(new List<FixTypeDto>());

      // Act
      var actionResult = await _fixTypeMediator.GetManyAsync(cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 0);
    }

    [TestMethod]
    [DataRow(DisplayName = "Any_Filter")]
    public async Task GetManyAsync_FilterNotSpecified_ReturnsAllFixTypeDtos()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _requestMdmFixTypeDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyAsync(null, It.IsAny<CancellationToken>(), null, null))
                           .ReturnsAsync(_fakeFixTypes);

      // Act
      var actionResult = await _fixTypeMediator.GetManyAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeFixTypes.Count());
    }
    #endregion

    #region GetManyByPageAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<FixTypeDto>()
      {
        Results = _fakeFixTypes.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeFixTypes.Count()
      };

      _requestMdmFixTypeDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyByPageAsync(currentPage, null, It.IsAny<CancellationToken>(), null, null, null))
                           .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _fixTypeMediator.GetManyByPageAsync(currentPage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.Results);
    }
    #endregion
  }
}

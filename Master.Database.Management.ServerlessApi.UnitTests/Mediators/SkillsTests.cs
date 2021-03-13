using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Users.Skills;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Mediators;
using Master.Database.Management.ServerlessApi.Mediators.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Mediators
{
  [TestClass]
  public class SkillsTests : TestBase
  {
    private Mock<IMdmSkillDal> _requestMdmSkillDal;

    private ISkillMediator _skillMediator;

    // Fake Data
    private IEnumerable<SkillDto> _fakeSkills;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmSkillDal = new Mock<IMdmSkillDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeSkills = _fakeDtoSeederFactory.CreateSeederFactory(new SkillDto());

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmSkillDal()).Returns(_requestMdmSkillDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _skillMediator = _requestMediatorFactory.RequestSkillMediator();
    }

    #region GetFirstOrDefaultAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdNotFound_ReturnsDefault(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      _requestMdmSkillDal.Setup(RequestMdmSkillDal => RequestMdmSkillDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(default(SkillDto));

      // Act
      var actionResult = await _skillMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("727012a4-773c-4994-99c9-0ff83d9e8734", DisplayName = "Known_TypeId")]
    public async Task GetFirstOrDefaultAsync_TypeIdFound_ReturnsSkillDto(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeIdGuid = new Guid(typeId);

      _requestMdmSkillDal.Setup(RequestMdmSkillDal => RequestMdmSkillDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_fakeSkills.Where(fakeSkill => fakeSkill.Id.Equals(typeIdGuid)).First());

      // Act
      var actionResult = await _skillMediator.GetByIdAsync(typeIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("Masonry", DisplayName = "Known_FilterByName")]
    public async Task GetFirstOrDefaultAsync_TypeNameFound_ReturnsMatchedSkillDtos(string typeName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = typeName };
      _requestMdmSkillDal.Setup(RequestMdmSkillDal => RequestMdmSkillDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(_fakeSkills.Where(fakeSkill => fakeSkill.Name.Equals(typeName)).ToList());

      // Act
      var actionResult = await _skillMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 1);
    }

    [TestMethod]
    [DataRow("Unknown SKill", DisplayName = "Any_FilterByName")]
    public async Task GetManyAsync_TypeNameNotFound_ReturnsDefault(string typeName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto { Name = typeName };
      _requestMdmSkillDal.Setup(RequestMdmSkillDal => RequestMdmSkillDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<SkillDto>());

      // Act
      var actionResult = await _skillMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 0);
    }

    [TestMethod]
    [DataRow(DisplayName = "Any_Filter")]
    public async Task GetManyAsync_FilterNotSpecified_ReturnsAllSkillDtos()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var filterBaseDto = new FilterBaseDto();
      _requestMdmSkillDal.Setup(RequestMdmSkillDal => RequestMdmSkillDal.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(_fakeSkills);

      // Act
      var actionResult = await _skillMediator.GetManyAsync(filterBaseDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeSkills.Count());
    }
    #endregion

    #region GetManyByPageAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<SkillDto>()
      {
        Results = _fakeSkills.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeSkills.Count()
      };
      var paginationRequestBaseDto = new PaginationRequestBaseDto { PageNumber = currentPage };
      _requestMdmSkillDal.Setup(RequestMdmSkillDal => RequestMdmSkillDal.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _skillMediator.GetManyByPageAsync(paginationRequestBaseDto, cancellationToken);

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
      _requestMdmSkillDal.Reset();

      // Clean-up data objects
      _fakeSkills = null;
      _skillMediator = null;
    }
    #endregion
  }
}

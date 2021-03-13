using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Users.Skills;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.ServerlessApi.Functions.Skills;
using Master.Database.Management.ServerlessApi.Mediators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Master.Database.Management.ServerlessApi.UnitTests.Functions
{
  [TestClass]
  public class SkillsTests : TestBase
  {
    private Mock<ISkillMediator> _skillMediator;

    // Fake Data
    private IEnumerable<SkillDto> _fakeSkills;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _fakeRequestMediatorFactory = new Mock<IRequestMediatorFactory>();
      _skillMediator = new Mock<ISkillMediator>();

      _fakeSkills = _fakeDtoSeederFactory.CreateSeederFactory(new SkillDto());

      _fakeRequestMediatorFactory.Setup(requestMediatorFactory => requestMediatorFactory.RequestSkillMediator()).Returns(_skillMediator.Object);
    }

    #region GetSkillByIdAsyncTests
    [TestMethod]
    [DataRow("727012a4-773c-4994-99c9-0ff83d9e8734", DisplayName = "Known_TypeId")]
    public async Task GetSkillByIdAsync_TypeIdFound_ReturnsOkObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var skillIdGuid = new Guid(typeId);

      var GetSkillByIdFunction = new GetSkillById(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_fakeSkills.Where(fakeSkill => fakeSkill.Id.Equals(skillIdGuid)).First());

      // Act
      var actionResult = await GetSkillByIdFunction.GetSkillByIdAsync(skillIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_TypeId")]
    public async Task GetSkillByIdAsync_TypeIdNotFound_ReturnsNotFoundObjectResult(string typeId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var skillIdGuid = new Guid(typeId);

      var GetSkillByIdFunction = new GetSkillById(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(default(SkillDto));

      // Act
      var actionResult = await GetSkillByIdFunction.GetSkillByIdAsync(skillIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
    #endregion

    #region GetSkillsAsyncTests
    [TestMethod]
    public async Task GetSkillsAsync_FiltersFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      var GetSkillsFunction = new GetSkills(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_fakeSkills);

      // Act
      var actionResult = await GetSkillsFunction.GetSkillsAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetSkillsAsync_FiltersNotFound_ReturnsOkObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";

      var GetSkillsFunction = new GetSkills(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new List<SkillDto>());

      // Act
      var actionResult = await GetSkillsFunction.GetSkillsAsync(cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out List<SkillDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Any());
    }

    [TestMethod]
    public async Task GetSkillsAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult()
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var GetSkillsFunction = new GetSkills(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetManyAsync(It.IsAny<FilterBaseDto>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_fakeSkills);

      // Act
      var actionResult = await GetSkillsFunction.GetSkillsAsync(cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
    }
    #endregion

    #region GetPagedSkillsAsyncTests
    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedSkillsAsync_FiltersFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var pagedModelCollectionDto = new PagedModelCollectionDto<SkillDto>()
      {
        Results = _fakeSkills.ToList(),
        PageNumber = pageNumber,
        TotalModelCount = _fakeSkills.Count()
      };

      var GetPagedSkillsFunction = new GetPagedSkills(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(pagedModelCollectionDto);

      // Act
      var actionResult = await GetPagedSkillsFunction.GetPagedSkillsAsync(pageSize, pageNumber, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedSkillsAsync_FiltersNotFound_ReturnsOkObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var typeName = "A name that doesn't exist";
      var pageSize = 0;

      var GetPagedSkillsFunction = new GetPagedSkills(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new PagedModelCollectionDto<SkillDto>());

      // Act
      var actionResult = await GetPagedSkillsFunction.GetPagedSkillsAsync(pageSize, pageNumber, cancellationToken, typeName);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(actionResult, out OkObjectResult objectResult));
      Assert.IsTrue(_fakeHttpRequestMessageAdapterBase.IsResponseObjectTypeOf(objectResult.Value, out PagedModelCollectionDto<SkillDto> objectResultValue));
      Assert.IsFalse(objectResultValue.Results.Any());
      Assert.IsTrue(objectResultValue.PageNumber == 0);
      Assert.IsTrue(objectResultValue.TotalModelCount == 0);
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Any_PageNumber")]
    public async Task GetPagedSkillsAsync_InvalidTimestampUtc_ReturnsBadRequestObjectResult(int pageNumber)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pageSize = 0;
      var minTimestampUtcString = "oct10twentyfifteen";
      var maxTimestampUtcString = "1609636182";

      var GetPagedSkillsFunction = new GetPagedSkills(_fakeRequestMediatorFactory.Object);

      _skillMediator.Setup(skillMediator => skillMediator.GetManyByPageAsync(It.IsAny<PaginationRequestBaseDto>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new PagedModelCollectionDto<SkillDto>());

      // Act
      var actionResult = await GetPagedSkillsFunction.GetPagedSkillsAsync(pageSize, pageNumber, cancellationToken, null, minTimestampUtcString, maxTimestampUtcString);

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
      _skillMediator.Reset();
      _fakeConfiguration.Reset();
      _fakeRequestMediatorFactory.Reset();

      // Clean-up data objects
      _fakeSkills = null;
    }
    #endregion
  }
}

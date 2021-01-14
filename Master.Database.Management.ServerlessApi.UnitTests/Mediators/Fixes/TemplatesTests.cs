using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;
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
  public class TemplatesTests : TestBase
  {
    private Mock<IMdmFixTemplateDal> _requestMdmFixTemplateDal;

    private IFixTemplateMediator _fixTemplateMediator;

    // Fake Data
    private IEnumerable<FixTemplateDto> _fakeFixTemplates;

    [TestInitialize]
    public void TestInitialize()
    {
      _fakeConfiguration = new Mock<IConfiguration>();
      _requestMdmFixTemplateDal = new Mock<IMdmFixTemplateDal>();
      _fakeRequestMdmDalFactory = new Mock<IRequestMdmDalFactory>();

      _fakeFixTemplates = _fakeDtoSeederFactory.CreateFakeSeeder<FixTemplateDto>().SeedFakeDtos();
      
      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmFixTemplateDal()).Returns(_requestMdmFixTemplateDal.Object);
      _requestMediatorFactory = new RequestMediatorFactory(_mapperConfiguration.CreateMapper(), _fakeRequestMdmDalFactory.Object);
      _fixTemplateMediator = _requestMediatorFactory.RequestFixTemplateMediator();
    }

    #region CreateAsyncTests
    [TestMethod]
    public async Task CreateAsync_SuccessfulCreation_ReturnsFixTemplateDto()
    {
      // Arrange
      var requestMdmSectionDal = new Mock<IMdmSectionDal>();
      var requestMdmFieldDal = new Mock<IMdmFieldDal>();

      var fakeFixTemplateCreateRequestDtos = _fakeDtoSeederFactory.CreateFakeSeeder<FixTemplateCreateRequestDto>().SeedFakeDtos();
      var fakeSections = _fakeDtoSeederFactory.CreateFakeSeeder<SectionDto>().SeedFakeDtos();
      var fakeFields = _fakeDtoSeederFactory.CreateFakeSeeder<FieldDto>().SeedFakeDtos();

      var cancellationToken = CancellationToken.None;
      var mdmSectionResponseDto = new MdmResponseDto<List<SectionDto>>(true, fakeSections.ToList());
      var mdmfieldResponseDto = new MdmResponseDto<List<FieldDto>>(true, fakeFields.ToList());

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.CreateAsync(It.IsAny<FixTemplateDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeFixTemplates.First());

      requestMdmSectionDal.Setup(requestMdmSectionDal => requestMdmSectionDal.GetOrCreateManyAsync(It.IsAny<IEnumerable<SectionCreateRequestDto>>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mdmSectionResponseDto);

      requestMdmFieldDal.Setup(requestMdmFieldDal => requestMdmFieldDal.GetOrCreateManyAsync(It.IsAny<IEnumerable<FieldCreateRequestDto>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(mdmfieldResponseDto);

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmSectionDal()).Returns(requestMdmSectionDal.Object);
      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmFieldDal()).Returns(requestMdmFieldDal.Object);

      // Act
      var actionResult = await _fixTemplateMediator.CreateAsync(fakeFixTemplateCreateRequestDtos.First(), cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetFirstOrDefaultAsyncTests
    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_FixTemplateId")]
    public async Task GetFirstOrDefaultAsync_TypeIdNotFound_ReturnsDefault(string fixTemplateId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid fixTemplateIdGuid = new Guid(fixTemplateId);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await _fixTemplateMediator.GetByIdAsync(fixTemplateIdGuid, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("ba60c506-05ae-4d7f-a68c-48a60599bdf6", DisplayName = "Known_FixTemplateId")]
    public async Task GetFirstOrDefaultAsync_TypeIdFound_ReturnsFixTemplateDto(string fixTemplateId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      Guid fixTemplateIdGuid = new Guid(fixTemplateId);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeFixTemplates.Where(fakeFixTemplate => fakeFixTemplate.Id.Equals(fixTemplateIdGuid)).First());

      // Act
      var actionResult = await _fixTemplateMediator.GetByIdAsync(fixTemplateIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region GetManyAsyncTests
    [TestMethod]
    [DataRow("Public", DisplayName = "Known_FilterByStatus")]
    public async Task GetManyAsync_StatusFound_ReturnsMatchedFixTypeDtos(string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateStatus = (FixTemplateStatus)Enum.Parse(typeof(FixTemplateStatus), status, true);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyAsync(It.IsAny<CancellationToken>(), null, null, It.IsAny<FixTemplateStatus>(), null, null, null, null))
                               .ReturnsAsync(_fakeFixTemplates.Where(fakeFixTemplate => fakeFixTemplate.Status.Equals(fixTemplateStatus)).ToList());

      // Act
      var actionResult = await _fixTemplateMediator.GetManyAsync(cancellationToken, null, null, fixTemplateStatus);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == 1);
    }

    [TestMethod]
    [DataRow("Private", DisplayName = "Any_FilterByName")]
    public async Task GetManyAsync_StatusNotFound_ReturnsDefault(string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateStatus = (FixTemplateStatus)Enum.Parse(typeof(FixTemplateStatus), status, true);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyAsync(It.IsAny<CancellationToken>(), null, null, It.IsAny<FixTemplateStatus>(), null, null, null, null))
                               .ReturnsAsync(new List<FixTemplateDto>());

      // Act
      var actionResult = await _fixTemplateMediator.GetManyAsync(cancellationToken, null, null, fixTemplateStatus);

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

      _requestMdmFixTemplateDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyAsync(It.IsAny<CancellationToken>(), null, null, null, null, null, null, null))
                               .ReturnsAsync(_fakeFixTemplates);

      // Act
      var actionResult = await _fixTemplateMediator.GetManyAsync(cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Count() == _fakeFixTemplates.Count());
    }
    #endregion

    #region GetManyByPageAsync
    [TestMethod]
    [DataRow(1, DisplayName = "Any_Filter")]
    public async Task GetManyByPageAsync_FilterNotSpecified_ReturnsPagedModelCollectionDto(int currentPage)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var pagedModelcollectionDto = new PagedModelCollectionDto<FixTemplateDto>()
      {
        Results = _fakeFixTemplates.ToList(),
        PageNumber = currentPage,
        TotalModelCount = _fakeFixTemplates.Count()
      };

      _requestMdmFixTemplateDal.Setup(requestMdmFixTypeDal => requestMdmFixTypeDal.GetManyByPageAsync(currentPage, It.IsAny<CancellationToken>(), null, null, null, null, null, null, null, null))
                               .ReturnsAsync(pagedModelcollectionDto);

      // Act
      var actionResult = await _fixTemplateMediator.GetManyByPageAsync(currentPage, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.Results);
    }
    #endregion

    #region UpdateAsyncTests
    [TestMethod]
    [DataRow("ba60c506-05ae-4d7f-a68c-48a60599bdf6", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateAsync_SuccessfulModification_ReturnsFixTemplateDto(string fixTemplateId)
    {
      // Arrange
      var requestMdmSectionDal = new Mock<IMdmSectionDal>();
      var requestMdmFieldDal = new Mock<IMdmFieldDal>();

      var fakeFixTemplateUpdateRequestDto = _fakeDtoSeederFactory.CreateFakeSeeder<FixTemplateUpdateRequestDto>().SeedFakeDtos().First();
      var fakeSections = _fakeDtoSeederFactory.CreateFakeSeeder<SectionDto>().SeedFakeDtos();
      var fakeFields = _fakeDtoSeederFactory.CreateFakeSeeder<FieldDto>().SeedFakeDtos();

      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);
      var fakeSectionsTrimmed = fakeSections.Select(fakeSection => new SectionDto() { Id = fakeSection.Id, Name = fakeSection.Name.ToLower().Trim() }) ;
      var mdmSectionResponseDto = new MdmResponseDto<List<SectionDto>>(true, fakeSectionsTrimmed.ToList());
      var mdmfieldResponseDto = new MdmResponseDto<List<FieldDto>>(true, fakeFields.ToList());

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.UpdateAsync(It.IsAny<FixTemplateDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeFixTemplates.First());

      requestMdmSectionDal.Setup(requestMdmSectionDal => requestMdmSectionDal.GetOrCreateManyAsync(It.IsAny<IEnumerable<SectionCreateRequestDto>>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mdmSectionResponseDto);

      requestMdmFieldDal.Setup(requestMdmFieldDal => requestMdmFieldDal.GetOrCreateManyAsync(It.IsAny<IEnumerable<FieldCreateRequestDto>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(mdmfieldResponseDto);

      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmSectionDal()).Returns(requestMdmSectionDal.Object);
      _fakeRequestMdmDalFactory.Setup(requestMdmDalFactory => requestMdmDalFactory.RequestMdmFieldDal()).Returns(requestMdmFieldDal.Object);

      // Act
      var actionResult = await _fixTemplateMediator.UpdateAsync(fixTemplateIdGuid, fakeFixTemplateUpdateRequestDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region UpdateCostAsyncTests
    [TestMethod]
    [DataRow("ba60c506-05ae-4d7f-a68c-48a60599bdf6", 987.65, DisplayName = "Known_FixTemplateId")]
    public async Task UpdateCostAsync_SuccessfulModification_ReturnsFixTemplateDto(string fixTemplateId, double cost)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);
      var updatedFakeFixTemplate = _fakeFixTemplates.First();
      updatedFakeFixTemplate.SystemCostEstimate = cost;

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeFixTemplates.First());

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.UpdateAsync(It.IsAny<FixTemplateDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(updatedFakeFixTemplate);

      // Act
      var actionResult = await _fixTemplateMediator.UpdateCostAsync(fixTemplateIdGuid, cost, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.SystemCostEstimate.Equals(cost));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", 987.65, DisplayName = "Known_FixTemplateId")]
    public async Task UpdateCostAsync_fixTemplateIdNotFound_ReturnsDefault(string fixTemplateId, double cost)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await _fixTemplateMediator.UpdateCostAsync(fixTemplateIdGuid, cost, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }
    #endregion

    #region UpdateStatusAsyncTests
    [TestMethod]
    [DataRow("ba60c506-05ae-4d7f-a68c-48a60599bdf6", "Public", DisplayName = "Known_FixTemplateId")]
    public async Task UpdateStatusAsync_fixTemplateIdFound_ReturnsFixTemplateDto(string fixTemplateId, string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);
      var fixTemplateStatus = (FixTemplateStatus)Enum.Parse(typeof(FixTemplateStatus), status, true);
      var updatedFakeFixTemplate = _fakeFixTemplates.First();
      updatedFakeFixTemplate.Status = fixTemplateStatus;

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(_fakeFixTemplates.First());

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.UpdateAsync(It.IsAny<FixTemplateDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(updatedFakeFixTemplate);

      // Act
      var actionResult = await _fixTemplateMediator.UpdateStatusAsync(fixTemplateIdGuid, fixTemplateStatus, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.Status.Equals(fixTemplateStatus));
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", "Public", DisplayName = "Any_FixTemplateId")]
    public async Task UpdateStatusAsync_fixTemplateIdNotFound_ReturnsDefault(string fixTemplateId, string status)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);
      var fixTemplateStatus = (FixTemplateStatus)Enum.Parse(typeof(FixTemplateStatus), status, true);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(default(FixTemplateDto));

      // Act
      var actionResult = await _fixTemplateMediator.UpdateStatusAsync(fixTemplateIdGuid, fixTemplateStatus, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }
    #endregion

    #region DeleteAsyncTests
    [TestMethod]
    [DataRow("ba60c506-05ae-4d7f-a68c-48a60599bdf6", DisplayName = "Known_FixTemplateId")]
    public async Task DeleteAsync_fixTemplateIdFound_ReturnsDefault(string fixTemplateId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);
      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true
      };

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                               .ReturnsAsync(operationStatus);

      // Act
      var actionResult = await _fixTemplateMediator.DeleteAsync(fixTemplateIdGuid, true, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("db53bc7b-2892-47a9-9134-3696579878df", DisplayName = "Any_FixTemplateId")]
    public async Task DeleteAsync_fixTemplateIdNotFound_ReturnsDefault(string fixTemplateId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                               .ReturnsAsync(default(OperationStatus));

      // Act
      var actionResult = await _fixTemplateMediator.DeleteAsync(fixTemplateIdGuid, true, cancellationToken);

      // Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("ba60c506-05ae-4d7f-a68c-48a60599bdf6", DisplayName = "Known_FixTemplateId")]
    public async Task DeleteAsync_fixTemplateIdFoundFailedToRemove_ReturnsDefault(string fixTemplateId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      var fixTemplateIdGuid = new Guid(fixTemplateId);
      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false
      };

      _requestMdmFixTemplateDal.Setup(requestMdmFixTemplateDal => requestMdmFixTemplateDal.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                               .ReturnsAsync(operationStatus);

      // Act
      var actionResult = await _fixTemplateMediator.DeleteAsync(fixTemplateIdGuid, true, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsFalse(actionResult.IsOperationSuccessful);

    }
    #endregion
  }
}

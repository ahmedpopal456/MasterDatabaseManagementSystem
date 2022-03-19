using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;
using Fixit.Core.DataContracts;

namespace Master.Database.Management.ServerlessApi.Mediators.Internal.FixTemplates
{
  internal class FixTemplateMediator : IFixTemplateMediator
  {
    private readonly IMapper _mapper;
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public FixTemplateMediator(IMapper mapper, IRequestMdmDalFactory requestMdmDalFactory)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixTemplateMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(FixTemplateMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<FixTemplateDto> CreateAsync(FixTemplateCreateRequestDto fixTemplateCreateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixTemplateDto);

      var operationStatus = new OperationStatus() { IsOperationSuccessful = true }; 

      // Create a FixTemplateDto
      var fixTemplateDto = _mapper.Map<FixTemplateCreateRequestDto, FixTemplateDto>(fixTemplateCreateRequestDto);
      result = fixTemplateDto;

      // Extract the SectionCreateRequestDtos
      var sectionCreateRequestDtos = fixTemplateDto.Sections?.Select(fixTemplateSectionDto => new SectionCreateRequestDto { Name = fixTemplateSectionDto.Name });
      if (sectionCreateRequestDtos is { } && sectionCreateRequestDtos.Any())
      {
        // Get or Create Sections in DB
        var sectionResponseDto = await _requestMdmDalFactory.RequestMdmSectionDal().GetOrCreateManyAsync(sectionCreateRequestDtos, cancellationToken);
        if (sectionResponseDto != null && sectionResponseDto.IsOperationSuccessful && sectionResponseDto.Content.Any() && sectionResponseDto.Content.All(sectionDto => sectionDto != null && !sectionDto.Id.Equals(Guid.Empty)))
        {
          // If sections are created successfully,
          operationStatus.IsOperationSuccessful = sectionResponseDto.IsOperationSuccessful;
          var createdSections = sectionResponseDto.Content;
          fixTemplateDto.Sections = createdSections.Select((createdSectionDto, index) => _mapper.Map<SectionDto, FixTemplateSectionDto>(createdSectionDto, fixTemplateDto.Sections.ElementAt(index))).ToList();

          // then extract the FieldCreateRequestDtos
          var fieldCreateRequestDtos = fixTemplateDto.Sections.SelectMany(fixTemplateSectionDto => fixTemplateSectionDto.Fields).Distinct().Select(fixTemplateFieldDto => new FieldCreateRequestDto { Name = fixTemplateFieldDto.Name });

          // and get or create them in DB.
          var fieldResponseDto = await _requestMdmDalFactory.RequestMdmFieldDal().GetOrCreateManyAsync(fieldCreateRequestDtos, cancellationToken);
          if (fieldResponseDto != null && fieldResponseDto.IsOperationSuccessful && fieldResponseDto.Content.All(fieldDto => fieldDto != null && !fieldDto.Id.Equals(Guid.Empty)))
          {
            // If fields are created successfully,
            operationStatus.IsOperationSuccessful = fieldResponseDto.IsOperationSuccessful;
            foreach (var fixTemplateSectionDto in fixTemplateDto.Sections)
            {
              var createdFieldDtos = fieldResponseDto.Content;

              // then extract the field ids and map them to the FixTemplateRequest
              fixTemplateSectionDto.Fields = fixTemplateSectionDto.Fields.Select(fixTemplateFieldDto =>
              {
                var createdFieldDto = createdFieldDtos.FirstOrDefault(createdFieldDto => createdFieldDto.Name.Equals(fixTemplateFieldDto.Name));
                fixTemplateFieldDto.Id = createdFieldDto.Id;

                return fixTemplateFieldDto;
              }).ToList();
            }
          }
        }
      }

      if (operationStatus.IsOperationSuccessful)
      {
        result = await _requestMdmDalFactory.RequestMdmFixTemplateDal().CreateAsync(fixTemplateDto, cancellationToken);
      }

      return result;
    }

    public async Task<FixTemplateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTemplateDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<FixTemplateDto>> GetManyAsync(FixTemplateFilterDto fixTemplateFilterDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTemplateDal().GetManyAsync(fixTemplateFilterDto, cancellationToken);

      return result;
    }

    public async Task<PagedModelCollectionDto<FixTemplateDto>> GetManyByPageAsync(FixTemplatePaginationRequestDto fixTemplatePaginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTemplateDal().GetManyByPageAsync(fixTemplatePaginationRequestDto, cancellationToken);

      return result;
    }

    public async Task<FixTemplateDto> UpdateAsync(Guid id, FixTemplateUpdateRequestDto fixTemplateUpdateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixTemplateDto);

      // Create a FixTemplateDto
      var fixTemplateDto = _mapper.Map<FixTemplateUpdateRequestDto, FixTemplateDto>(fixTemplateUpdateRequestDto);
      fixTemplateDto.Id = id;
      result = fixTemplateDto;

      // Extract the SectionCreateRequestDtos
      var sectionCreateRequestDtos = fixTemplateDto.Sections.Select(section => new SectionCreateRequestDto { Name = section.Name });
      if (sectionCreateRequestDtos.Any())
      {
        // Get or Create Sections in DB
        var sectionResponseDto = await _requestMdmDalFactory.RequestMdmSectionDal().GetOrCreateManyAsync(sectionCreateRequestDtos, cancellationToken);
        if (sectionResponseDto != null && sectionResponseDto.IsOperationSuccessful && sectionResponseDto.Content.Any() && sectionResponseDto.Content.All(section => section != null && !section.Id.Equals(Guid.Empty)))
        {
          var createdSections = sectionResponseDto.Content;
          // If sections are created successfully,
          fixTemplateDto.Sections = createdSections.Select(createdSection => _mapper.Map<SectionDto, FixTemplateSectionDto>(createdSection, fixTemplateDto.Sections.FirstOrDefault(fixTemplateSectionDto => fixTemplateSectionDto.Name.ToLower().Trim().Equals(createdSection.Name)))).ToList();

          // then extract the FieldCreateRequestDtos
          var fieldCreateRequestDtos = fixTemplateDto.Sections.SelectMany(section => section.Fields).Distinct().Select(field => new FieldCreateRequestDto { Name = field.Name });

          // and get or create them in DB.
          var fieldResponseDto = await _requestMdmDalFactory.RequestMdmFieldDal().GetOrCreateManyAsync(fieldCreateRequestDtos, cancellationToken);
          if (fieldResponseDto != null && fieldResponseDto.IsOperationSuccessful && fieldResponseDto.Content.All(fieldDto => fieldDto != null && !fieldDto.Id.Equals(Guid.Empty)))
          {
            var createdFieldDtos = fieldResponseDto.Content;

            // If fields are created successfully,
            foreach (var fixTemplateSectionDto in fixTemplateDto.Sections)
            {
              // then extract the field ids and map them to the FixTemplateRequest
              fixTemplateSectionDto.Fields = fixTemplateSectionDto.Fields.Select(fixTemplateFieldDto =>
              {
                var createdFieldDto = createdFieldDtos.FirstOrDefault(createdFieldDto => createdFieldDto.Name.Equals(fixTemplateFieldDto.Name));
                fixTemplateFieldDto.Id = createdFieldDto.Id;

                return fixTemplateFieldDto;
              }).ToList();
            }
            result = await _requestMdmDalFactory.RequestMdmFixTemplateDal().UpdateAsync(fixTemplateDto, cancellationToken);
          }
        }
      }

      return result;
    }

    public async Task<FixTemplateDto> UpdateCostAsync(Guid id, double cost, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixTemplateDto);

      var fixTemplateDto = await _requestMdmDalFactory.RequestMdmFixTemplateDal().GetByIdAsync(id, cancellationToken);
      if (fixTemplateDto != null)
      {
        fixTemplateDto.SystemCostEstimate = cost;

        var updatedFixTemplate = await _requestMdmDalFactory.RequestMdmFixTemplateDal().UpdateAsync(fixTemplateDto, cancellationToken);
        if (updatedFixTemplate != null)
        {
          result = updatedFixTemplate;
        }
      }

      return result;
    }

    public async Task<FixTemplateDto> UpdateStatusAsync(Guid id, FixTemplateStatus fixTemplateStatus, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixTemplateDto);

      var fixTemplateDto = await _requestMdmDalFactory.RequestMdmFixTemplateDal().GetByIdAsync(id, cancellationToken);
      if (fixTemplateDto != null)
      {
        fixTemplateDto.Status = fixTemplateStatus;

        var updatedFixTemplate = await _requestMdmDalFactory.RequestMdmFixTemplateDal().UpdateAsync(fixTemplateDto, cancellationToken);
        if (updatedFixTemplate != null)
        {
          result = updatedFixTemplate;
        }
      }

      return result;
    }

    public async Task<OperationStatus> DeleteAsync(Guid id, bool softDelete, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTemplateDal().DeleteAsync(id, cancellationToken, softDelete);

      return result;
    }
  }
}

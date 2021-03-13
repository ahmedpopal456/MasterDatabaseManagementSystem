using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.DataLayer.Models.FixTemplates;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates
{
  internal class MdmFixTemplateDal : IMdmFixTemplateDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmFixTemplateDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmFixTemplateDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmFixTemplateDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<FixTemplateDto> CreateAsync(FixTemplateDto fixTemplateDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixTemplateDto == null)
      {
        throw new ArgumentNullException($"{nameof(CreateAsync)} expects a value for {nameof(fixTemplateDto)}... null argument was provided");
      }

      var fixTemplateDtoResult = default(FixTemplateDto);

      var fixTemplate = _mapper.Map<FixTemplateDto, FixTemplate>(fixTemplateDto);
      if (fixTemplate != null)
      {
        await _mdmContext.FixTemplates.AddAsync(fixTemplate);
        if (Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken)))
        {
          fixTemplateDtoResult = await GetByIdAsync(fixTemplate.Id, cancellationToken);
        }

      }
      return fixTemplateDtoResult;
    }

    public async Task<FixTemplateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects a value for {nameof(Guid)}... null argument was provided");
      }

      var fixTemplateDto = default(FixTemplateDto);

      var fixTemplateResult = await _mdmContext.FixTemplates.Include(fixTemplate => fixTemplate.Tags)
                                                            .Include(fixTemplate => fixTemplate.WorkType)
                                                            .Include(fixTemplate => fixTemplate.WorkCategory)
                                                            .Include(fixTemplate => fixTemplate.WorkCategory).ThenInclude(workCategory => workCategory.WorkCategorySkills).ThenInclude(crossReference => crossReference.Skill)
                                                            .Include(fixTemplate => fixTemplate.FixUnit)
                                                            .Include(fixTemplate => fixTemplate.FixTemplateSections).ThenInclude(fixTemplateSection => fixTemplateSection.Section)
                                                            .Include(fixTemplate => fixTemplate.FixTemplateSections).ThenInclude(fixTemplateSection => fixTemplateSection.FixTemplateSectionFields).ThenInclude(fixTemplateSectionField => fixTemplateSectionField.Field)
                                                            .FirstOrDefaultAsync(fixTemplate => fixTemplate.Id.Equals(id), cancellationToken);

      if (fixTemplateResult != null)
      {
        fixTemplateDto = _mapper.Map<FixTemplate, FixTemplateDto>(fixTemplateResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return fixTemplateDto;
    }

    public async Task<IEnumerable<FixTemplateDto>> GetManyAsync(FixTemplateFilterDto fixTemplateFilterDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTemplatesQuery = _mdmContext.FixTemplates.Where(fixTemplate => (fixTemplateFilterDto.UserId == null || fixTemplate.CreatedByUserId.Equals(fixTemplateFilterDto.UserId))
                                                                             && (fixTemplateFilterDto.Tags == null || fixTemplate.Tags.Any(fixTemplateTag => fixTemplateFilterDto.Tags.Any(tagValue => fixTemplateTag.Name.Equals(tagValue))))
                                                                             && (fixTemplateFilterDto.Name == null || fixTemplate.Name.ToLower().Contains(fixTemplateFilterDto.Name.ToLower()))
                                                                             && (fixTemplateFilterDto.TypeName == null || fixTemplate.WorkType.Name.ToLower().Contains(fixTemplateFilterDto.Name.ToLower()))
                                                                             && (fixTemplateFilterDto.CategoryName == null || fixTemplate.WorkCategory.Name.ToLower().Contains(fixTemplateFilterDto.CategoryName.ToLower()))
                                                                             && (fixTemplateFilterDto.UnitName == null || fixTemplate.FixUnit.Name.ToLower().Contains(fixTemplateFilterDto.UnitName.ToLower()))
                                                                             && (fixTemplateFilterDto.Status == null || fixTemplate.Status.Equals(fixTemplateFilterDto.Status))
                                                                             && (fixTemplateFilterDto.MinTimestampUtc == null || fixTemplate.CreatedTimestampUtc >= fixTemplateFilterDto.MinTimestampUtc)
                                                                             && (fixTemplateFilterDto.MaxTimestampUtc == null || fixTemplate.CreatedTimestampUtc <= fixTemplateFilterDto.MaxTimestampUtc));

      var fixTemplatesResult = await fixTemplatesQuery.Include(fixTemplate => fixTemplate.Tags)
                                                      .Include(fixTemplate => fixTemplate.WorkType)
                                                      .Include(fixTemplate => fixTemplate.WorkCategory)
                                                      .Include(fixTemplate => fixTemplate.WorkCategory).ThenInclude(workCategory => workCategory.WorkCategorySkills).ThenInclude(crossReference => crossReference.Skill)
                                                      .Include(fixTemplate => fixTemplate.FixUnit)
                                                      .ToListAsync();

      var fixTemplateDtos = fixTemplatesResult.Select(fixTemplate => _mapper.Map<FixTemplate, FixTemplateDto>(fixTemplate));
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixTemplateDtos;
    }

    public async Task<PagedModelCollectionDto<FixTemplateDto>> GetManyByPageAsync(FixTemplatePaginationRequestDto fixTemplatePaginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTemplateResult = new PagedModelCollectionDto<FixTemplateDto>();

      var fixTemplatesQuery = _mdmContext.FixTemplates.Where(fixTemplate => (fixTemplatePaginationRequestDto.UserId == null || fixTemplate.CreatedByUserId.Equals(fixTemplatePaginationRequestDto.UserId))
                                                                             && (fixTemplatePaginationRequestDto.Tags == null || fixTemplate.Tags.Any(fixTemplateTag => fixTemplatePaginationRequestDto.Tags.Any(tagValue => fixTemplateTag.Name.Equals(tagValue))))
                                                                             && (fixTemplatePaginationRequestDto.Name == null || fixTemplate.Name.ToLower().Contains(fixTemplatePaginationRequestDto.Name.ToLower()))
                                                                             && (fixTemplatePaginationRequestDto.TypeName == null || fixTemplate.WorkType.Name.ToLower().Contains(fixTemplatePaginationRequestDto.Name.ToLower()))
                                                                             && (fixTemplatePaginationRequestDto.CategoryName == null || fixTemplate.WorkCategory.Name.ToLower().Contains(fixTemplatePaginationRequestDto.CategoryName.ToLower()))
                                                                             && (fixTemplatePaginationRequestDto.UnitName == null || fixTemplate.FixUnit.Name.ToLower().Contains(fixTemplatePaginationRequestDto.UnitName.ToLower()))
                                                                             && (fixTemplatePaginationRequestDto.Status == null || fixTemplate.Status.Equals(fixTemplatePaginationRequestDto.Status))
                                                                             && (fixTemplatePaginationRequestDto.MinTimestampUtc == null || fixTemplate.CreatedTimestampUtc >= fixTemplatePaginationRequestDto.MinTimestampUtc)
                                                                             && (fixTemplatePaginationRequestDto.MaxTimestampUtc == null || fixTemplate.CreatedTimestampUtc <= fixTemplatePaginationRequestDto.MaxTimestampUtc));

      int validPageSize = fixTemplatePaginationRequestDto.PageSize.Equals(default(int)) ? PageSize : fixTemplatePaginationRequestDto.PageSize.Value;
      var fixTemplatesByPage = await fixTemplatesQuery.Include(fixTemplate => fixTemplate.Tags)
                                                      .Include(fixTemplate => fixTemplate.WorkType)
                                                      .Include(fixTemplate => fixTemplate.WorkCategory)
                                                      .Include(fixTemplate => fixTemplate.WorkCategory).ThenInclude(workCategory => workCategory.WorkCategorySkills).ThenInclude(crossReference => crossReference.Skill)
                                                      .Include(fixTemplate => fixTemplate.FixUnit)
                                                      .ToPagedListAsync(validPageSize, fixTemplatePaginationRequestDto.PageNumber, cancellationToken);

      if (fixTemplatesByPage != null && fixTemplatesByPage.Any())
      {
        fixTemplateResult = new PagedModelCollectionDto<FixTemplateDto>
        {
          Results = fixTemplatesByPage.Select(fixTemplate => _mapper.Map<FixTemplate, FixTemplateDto>(fixTemplate)).ToList(),
          TotalModelCount = fixTemplatesByPage.TotalCount,
          PageNumber = fixTemplatesByPage.PageIndex
        };
      }

      return fixTemplateResult;
    }

    public async Task<FixTemplateDto> UpdateAsync(FixTemplateDto fixTemplateDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixTemplateDto == null)
      {
        throw new ArgumentNullException($"{nameof(UpdateAsync)} expects a value for {nameof(fixTemplateDto)}... null argument was provided");
      }

      var fixTemplateDtoResult = default(FixTemplateDto);

      var fixTemplate = await _mdmContext.FixTemplates.Include(fixTemplate => fixTemplate.Tags)
                                                      .Include(fixTemplate => fixTemplate.WorkType)
                                                      .Include(fixTemplate => fixTemplate.WorkCategory)
                                                      .Include(fixTemplate => fixTemplate.WorkCategory).ThenInclude(workCategory => workCategory.WorkCategorySkills).ThenInclude(crossReference => crossReference.Skill)
                                                      .Include(fixTemplate => fixTemplate.FixUnit)
                                                      .Include(fixTemplate => fixTemplate.FixTemplateSections).ThenInclude(fixTemplateSection => fixTemplateSection.Section)
                                                      .Include(fixTemplate => fixTemplate.FixTemplateSections).ThenInclude(fixTemplateSection => fixTemplateSection.FixTemplateSectionFields).ThenInclude(fixTemplateSectionField => fixTemplateSectionField.Field)
                                                      .FirstOrDefaultAsync(f => f.Id.Equals(fixTemplateDto.Id));
      if (fixTemplate != null)
      {
        fixTemplate.Tags.Clear();
        fixTemplate.FixTemplateSections.Clear();

        var updatedFixTemplate = _mapper.Map<FixTemplateDto, FixTemplate>(fixTemplateDto, fixTemplate);
        foreach (var updateTag in updatedFixTemplate.Tags)
        {
          fixTemplate.Tags.Add(updateTag);
        }
        foreach (var updateSection in updatedFixTemplate.FixTemplateSections)
        {
          fixTemplate.FixTemplateSections.Add(updateSection);
        }

        _mdmContext.FixTemplates.Update(updatedFixTemplate);
        if (Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken)))
        {
          fixTemplateDtoResult = await GetByIdAsync(updatedFixTemplate.Id, cancellationToken);
        }
      }
      return fixTemplateDtoResult;
    }

    public async Task<OperationStatus> DeleteAsync(Guid id, CancellationToken cancellationToken, bool isSoftDeleted = true)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects a value for {nameof(Guid)}... null argument was provided");
      }

      var result = default(OperationStatus);

      var fixTemplate = _mdmContext.FixTemplates.FirstOrDefault(fixTemplate => fixTemplate.Id.Equals(id));
      if (fixTemplate != null)
      {
        fixTemplate.IsDeleted = isSoftDeleted;
        _mdmContext.FixTemplates.Remove(fixTemplate);
        result = new OperationStatus()
        {
          IsOperationSuccessful = Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken))
        };
      }

      return result;
    }
  }
}

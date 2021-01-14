using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models.FixTemplates;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal
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
        throw new ArgumentNullException($"{nameof(CreateAsync)} expects {nameof(fixTemplateDto)} to have a value, null was provided...");
      }

      var fixTemplateDtoResult = default(FixTemplateDto);

      var fixTemplate = _mapper.Map<FixTemplateDto, FixTemplate>(fixTemplateDto);
      if (fixTemplate != null)
      {
        await _mdmContext.FixTemplates.AddAsync(fixTemplate);
        if (Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken)))
        {
          fixTemplateDtoResult = await GetByIdAsync(fixTemplate.Id, cancellationToken);
        };

      }
      return fixTemplateDtoResult;
    }

    public async Task<FixTemplateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}, {id} was provided...");
      }

      var fixTemplateDto = default(FixTemplateDto);

      var fixTemplateResult = await _mdmContext.FixTemplates.Include(fixTemplate => fixTemplate.Tags)
                                                            .Include(fixTemplate => fixTemplate.Category)
                                                            .Include(fixTemplate => fixTemplate.Type)
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

    public async Task<IEnumerable<FixTemplateDto>> GetManyAsync(CancellationToken cancellationToken,
                                                                Guid? userId = null,
                                                                string[] tags = null,
                                                                FixTemplateStatus? status = null,
                                                                string typeName = null,
                                                                string categoryName = null,
                                                                long? minTimestampUtc = null,
                                                                long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTemplatesQuery = _mdmContext.FixTemplates.Where(fixTemplate => (userId == null || fixTemplate.CreatedByUserId.Equals(userId))
                                                                              && (tags == null || fixTemplate.Tags.Any(fixTemplateTag => tags.Any(tagValue => fixTemplateTag.Name.Equals(tagValue))))
                                                                              && (typeName == null || fixTemplate.Name.ToLower().Contains(typeName.ToLower()))
                                                                              && (categoryName == null || fixTemplate.Name.ToLower().Contains(categoryName.ToLower()))
                                                                              && (status == null || fixTemplate.Status.Equals(status))
                                                                              && (minTimestampUtc == null || fixTemplate.CreatedTimestampUtc >= minTimestampUtc)
                                                                              && (maxTimestampUtc == null || fixTemplate.CreatedTimestampUtc <= maxTimestampUtc));

      var fixTemplatesResult = await fixTemplatesQuery.Include(fixTemplate => fixTemplate.Tags)
                                                      .Include(fixTemplate => fixTemplate.Category)
                                                      .Include(fixTemplate => fixTemplate.Type)
                                                      .ToListAsync();

      var fixTemplateDtos = fixTemplatesResult.Select(fixTemplate => _mapper.Map<FixTemplate, FixTemplateDto>(fixTemplate));
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixTemplateDtos;
    }

    public async Task<PagedModelCollectionDto<FixTemplateDto>> GetManyByPageAsync(int currentPage,
                                                                                   CancellationToken cancellationToken,
                                                                                   int? pageSize = null,
                                                                                   Guid? userId = null,
                                                                                   string[] tags = null,
                                                                                   FixTemplateStatus? status = null,
                                                                                   string typeName = null,
                                                                                   string categoryName = null,
                                                                                   long? minTimestampUtc = null,
                                                                                   long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTemplateResult = new PagedModelCollectionDto<FixTemplateDto>();

      var fixTemplatesQuery = _mdmContext.FixTemplates.Where(fixTemplate => (userId == null || fixTemplate.CreatedByUserId.Equals(userId))
                                                                             && (tags == null || fixTemplate.Tags.Any(fixTemplateTag => tags.Any(tagValue => fixTemplateTag.Name.Equals(tagValue))))
                                                                             && (typeName == null || fixTemplate.Name.ToLower().Contains(typeName.ToLower()))
                                                                             && (categoryName == null || fixTemplate.Name.ToLower().Contains(categoryName.ToLower()))
                                                                             && (status == null || fixTemplate.Status.Equals(status))
                                                                             && (minTimestampUtc == null || fixTemplate.CreatedTimestampUtc >= minTimestampUtc)
                                                                             && (maxTimestampUtc == null || fixTemplate.CreatedTimestampUtc <= maxTimestampUtc));

      int validPageSize = pageSize.Equals(default) ? PageSize : pageSize.Value;
      var fixTemplatesByPage = await fixTemplatesQuery.Include(fixTemplate => fixTemplate.Tags)
                                                      .Include(fixTemplate => fixTemplate.Category)
                                                      .Include(fixTemplate => fixTemplate.Type)
                                                      .ToPagedListAsync(validPageSize, currentPage, cancellationToken);

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
        throw new ArgumentNullException($"{nameof(UpdateAsync)} expects {nameof(fixTemplateDto)} to have a value, null was provided...");
      }

      var fixTemplateDtoResult = default(FixTemplateDto);

      var fixTemplate = await _mdmContext.FixTemplates.Include(fixTemplate => fixTemplate.Tags)
                                                      .Include(fixTemplate => fixTemplate.Category)
                                                      .Include(fixTemplate => fixTemplate.Type)
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
        };
      }
      return fixTemplateDtoResult;
    }

    public async Task<OperationStatus> DeleteAsync(Guid id, CancellationToken cancellationToken, bool isSoftDeleted = true)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}, {id} was provided...");
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

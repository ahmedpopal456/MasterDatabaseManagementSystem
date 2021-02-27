using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;
using AutoMapper;
using System.Collections.Generic;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal.Sections
{
  internal class MdmSectionDal : IMdmSectionDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;

    public MdmSectionDal(MdmContext mdmContext, IMapper mapper)
    {
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmSectionDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmSectionDal)} expects a value for {nameof(mapper)}... null argument was provided");
    }

    public async Task<SectionDto> CreateAsync(SectionCreateRequestDto sectionCreateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (sectionCreateRequestDto == null)
      {
        throw new ArgumentNullException($"{nameof(CreateAsync)} expects {nameof(sectionCreateRequestDto)} to have a value, null was provided...");
      }

      var createdSectionDto = default(SectionDto);

      Section section = _mapper.Map<SectionCreateRequestDto, Section>(sectionCreateRequestDto);
      if (section != null)
      {
        await _mdmContext.Sections.AddAsync(section);
        if (Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken)))
        {
          createdSectionDto = _mapper.Map<Section, SectionDto>(section);
        }
      }

      return createdSectionDto;
    }

    public async Task<MdmResponseDto<List<SectionDto>>> GetOrCreateManyAsync(IEnumerable<SectionCreateRequestDto> sectionCreateRequestDtos, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (sectionCreateRequestDtos == null || !sectionCreateRequestDtos.Any())
      {
        throw new ArgumentNullException($"Either {nameof(GetOrCreateManyAsync)} received null for {nameof(IEnumerable<SectionCreateRequestDto>)} or is empty...");
      }
      var mdmResponseDto = new MdmResponseDto<List<SectionDto>>(true, new List<SectionDto>());

      // Map SectionCreateRequestDtos to Sections,
      var sections = sectionCreateRequestDtos.Select(fieldCreateRequestDto => _mapper.Map<SectionCreateRequestDto, Section>(fieldCreateRequestDto)).ToList();
      if (sections != null && sections.Any())
      {
        // then extract existing Sections from the MdmContext,
        var existingSections = _mdmContext.Sections.Where(mdmSection => sections.Select(section => section.Name).Contains(mdmSection.Name)).ToList();
        // and add it to the return list -> result of mdmResponseDto.
        mdmResponseDto.Content.AddRange(existingSections.Select(existingSection => _mapper.Map<Section, SectionDto>(existingSection)));

        // Extract the list of new Sections to create.
        var existingSectionNames = existingSections.Select(section => section.Name);
        var newSections = sections.Where(section => !existingSectionNames.Any(existingSectionName => section.Name.Equals(existingSectionName))).ToList();

        if (newSections.Any())
        {
          try
          {
            // Add the new Sections to the MdmContext.
            await _mdmContext.Sections.AddRangeAsync(newSections, cancellationToken);
            mdmResponseDto.IsOperationSuccessful = Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken));
            if (mdmResponseDto.IsOperationSuccessful)
            {
              mdmResponseDto.Content.AddRange(newSections.Select(section => _mapper.Map<Section, SectionDto>(section, mdmResponseDto.Content.FirstOrDefault(mdmResponseDtoResult => mdmResponseDtoResult.Name.ToLower().Trim().Equals(section.Name)))));
            }
          }
          catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
          {
            mdmResponseDto.IsOperationSuccessful = false;
            mdmResponseDto.OperationException = dbUpdateConcurrencyException;
          }
          catch (DbUpdateException dbUpdateException)
          {
            mdmResponseDto.IsOperationSuccessful = false;
            mdmResponseDto.OperationException = dbUpdateException;
          }
          catch (Exception exception)
          {
            mdmResponseDto.IsOperationSuccessful = false;
            mdmResponseDto.OperationException = exception;
          }
        }

      }
      return mdmResponseDto;
    }

    public async Task<SectionDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(MdmSectionDal)} expects id to be a valid {nameof(Guid)}, {id} was provided...");
      }

      var sectionDto = default(SectionDto);

      var section = await _mdmContext.Sections.FirstOrDefaultAsync(section => section.Id.Equals(id), cancellationToken);

      if (section != null)
      {
        sectionDto = _mapper.Map<Section, SectionDto>(section);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return sectionDto;
    }

    public async Task<IEnumerable<SectionDto>> GetManyAsync(string sectionName, CancellationToken cancellationToken, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var sectionResult = _mdmContext.Sections.Where(section => (sectionName == null || section.Name.Trim().ToLower().Contains(sectionName.ToLower().Trim()))
                                                     && (minTimestampUtc == null || section.CreatedTimestampUtc >= minTimestampUtc)
                                                     && (maxTimestampUtc == null || section.CreatedTimestampUtc <= maxTimestampUtc)).AsEnumerable();

      var sectionDtos = sectionResult.Select(section => _mapper.Map<Section, SectionDto>(section));
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return sectionDtos;
    }
  }
}

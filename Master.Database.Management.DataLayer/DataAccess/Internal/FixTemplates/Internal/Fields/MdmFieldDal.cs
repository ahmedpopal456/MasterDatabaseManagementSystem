using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;
using AutoMapper;
using System.Collections.Generic;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal.Fields
{
  internal class MdmFieldDal : IMdmFieldDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;

    public MdmFieldDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmFieldDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmFieldDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<FieldDto> CreateAsync(FieldCreateRequestDto fieldCreateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fieldCreateRequestDto == null)
      {
        throw new ArgumentNullException($"{nameof(CreateAsync)} expects {nameof(fieldCreateRequestDto)} to have a value, null was provided...");
      }

      var createdFieldDto = default(FieldDto);

      var field = _mapper.Map<FieldCreateRequestDto, Field>(fieldCreateRequestDto);
      if (field != null)
      {
        await _mdmContext.Fields.AddAsync(field);
        if (Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken)))
        {
          createdFieldDto = _mapper.Map<Field, FieldDto>(field);
        };

      }
      return createdFieldDto;
    }

    public async Task<MdmResponseDto<List<FieldDto>>> GetOrCreateManyAsync(IEnumerable<FieldCreateRequestDto> fieldCreateRequestDtos, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fieldCreateRequestDtos == null || !fieldCreateRequestDtos.Any())
      {
        throw new ArgumentNullException($"{nameof(GetOrCreateManyAsync)} expects {nameof(IEnumerable<FieldCreateRequestDto>)} to have a value, it is either null or empty...");
      }

      var mdmResponseDto = new MdmResponseDto<List<FieldDto>>(true, new List<FieldDto>());

      // Map FieldCreateRequestDtos to Sections,
      var fields = fieldCreateRequestDtos.Select(fieldCreateRequestDto => _mapper.Map<FieldCreateRequestDto, Field>(fieldCreateRequestDto)).ToList();
      if (fields != null && fields.Any())
      {
        // then extract existing Fields from the MdmContext,
        var existingFields = _mdmContext.Fields.Where(mdmField => fields.Select(field => field.Name).Contains(mdmField.Name)).ToList();
        // and add it to the return list -> result of mdmResponseDto.
        mdmResponseDto.Content.AddRange(existingFields.Select(existingField => _mapper.Map<Field, FieldDto>(existingField)));

        // Extract the list of new Fields to create.
        var existingFieldNames = existingFields.Select(field => field.Name);
        var newFields = fields.Where(field => !existingFieldNames.Any(existingFieldName => field.Name.Equals(existingFieldName))).ToList();

        if (newFields.Any())
        {
          try
          {
            // Add the new Fields to the MdmContext.
            await _mdmContext.Fields.AddRangeAsync(newFields, cancellationToken);
            mdmResponseDto.IsOperationSuccessful = Convert.ToBoolean(await _mdmContext.SaveChangesAsync(true, cancellationToken));
            if (mdmResponseDto.IsOperationSuccessful)
            {
              mdmResponseDto.Content.AddRange(newFields.Select(field => _mapper.Map<Field, FieldDto>(field, mdmResponseDto.Content.FirstOrDefault(mdmResponseDtoResult => mdmResponseDtoResult.Name.ToLower().Trim().Equals(field.Name)))));
            };
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

    public async Task<FieldDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}, {id} was provided...");
      }

      var fieldResultDto = default(FieldDto);

      var fixFieldResult = await _mdmContext.Fields.FirstOrDefaultAsync(field => field.Id.Equals(id), cancellationToken);

      if (fixFieldResult != null)
      {
        fieldResultDto = _mapper.Map<Field, FieldDto>(fixFieldResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return fieldResultDto;
    }
  }
}

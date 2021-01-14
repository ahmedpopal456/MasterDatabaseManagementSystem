using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts.Fixes.Types;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal
{
  internal class MdmFixTypeDal : IMdmFixTypeDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmFixTypeDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmFixTypeDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmFixTypeDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<FixTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTypeDto = default(FixTypeDto);

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(MdmFixTypeDal)} expects id to be a valid {nameof(Guid)}, {id} was provided...");
      }

      var fixTypeResult = await _mdmContext.FixTypes.FirstOrDefaultAsync(fixType => fixType.Id.Equals(id), cancellationToken);
      if (fixTypeResult != null)
      {
        fixTypeDto = _mapper.Map<FixType, FixTypeDto>(fixTypeResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return fixTypeDto;
    }

    public async Task<IEnumerable<FixTypeDto>> GetManyAsync(string fixTypeName, CancellationToken cancellationToken, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTypeResult = _mdmContext.FixTypes.Where(fixType => (fixTypeName == null || fixType.Name.Trim().ToLower().Contains(fixTypeName.ToLower().Trim()))
                                                                 && (minTimestampUtc == null || fixType.CreatedTimestampUtc >= minTimestampUtc)
                                                                 && (maxTimestampUtc == null || fixType.CreatedTimestampUtc <= maxTimestampUtc)).AsEnumerable();

      var fixTypeDtos = fixTypeResult.Select(fixType => _mapper.Map<FixType, FixTypeDto>(fixType)).ToList();
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixTypeDtos;
    }

    public async Task<PagedModelCollectionDto<FixTypeDto>> GetManyByPageAsync(int currentPage, string fixTypeName, CancellationToken cancellationToken, int? pageSize = null, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTypeResults = new PagedModelCollectionDto<FixTypeDto>();

      var fixTypeQuery = _mdmContext.FixTypes.Where(fixType => (fixTypeName == null || fixType.Name.Trim().ToLower().Contains(fixTypeName.ToLower().Trim()))
                                                                && (minTimestampUtc == null || fixType.CreatedTimestampUtc >= minTimestampUtc)
                                                                && (maxTimestampUtc == null || fixType.CreatedTimestampUtc <= maxTimestampUtc));

      int validPageSize = pageSize.Equals(default) ? PageSize : pageSize.Value;
      var fixTypePagedResults = await fixTypeQuery.ToPagedListAsync(validPageSize, currentPage, cancellationToken);

      if (fixTypePagedResults != null && fixTypePagedResults.Any())
      {
        fixTypeResults = new PagedModelCollectionDto<FixTypeDto>
        {
          Results = fixTypePagedResults.Select(fixType => _mapper.Map<FixType, FixTypeDto>(fixType)).ToList(),
          TotalModelCount = fixTypePagedResults.TotalCount,
          PageNumber = fixTypePagedResults.PageIndex
        };
      }

      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixTypeResults;
    }
  }
}

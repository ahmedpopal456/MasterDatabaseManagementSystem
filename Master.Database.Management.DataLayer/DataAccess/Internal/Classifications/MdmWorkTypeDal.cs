using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;
using Master.Database.Management.DataLayer.Models.Classifications;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.Classifications
{
  internal class MdmWorkTypeDal : IMdmWorkTypeDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmWorkTypeDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmWorkTypeDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmWorkTypeDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<WorkTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTypeDto = default(WorkTypeDto);

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(MdmWorkTypeDal)} expects id to be a valid {nameof(Guid)}... null argument was provided");
      }

      var fixTypeResult = await _mdmContext.WorkTypes.FirstOrDefaultAsync(fixType => fixType.Id.Equals(id), cancellationToken);
      if (fixTypeResult != null)
      {
        fixTypeDto = _mapper.Map<WorkType, WorkTypeDto>(fixTypeResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return fixTypeDto;
    }

    public async Task<IEnumerable<WorkTypeDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTypeResult = _mdmContext.WorkTypes.Where(fixType => (filterBaseDto.Name == null || fixType.Name.Trim().ToLower().Contains(filterBaseDto.Name.ToLower().Trim()))
                                                                  && (filterBaseDto.MinTimestampUtc == null || fixType.CreatedTimestampUtc >= filterBaseDto.MinTimestampUtc)
                                                                  && (filterBaseDto.MaxTimestampUtc == null || fixType.CreatedTimestampUtc <= filterBaseDto.MaxTimestampUtc)).AsEnumerable();

      var fixTypeDtos = fixTypeResult.Select(fixType => _mapper.Map<WorkType, WorkTypeDto>(fixType)).ToList();
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixTypeDtos;
    }

    public async Task<PagedModelCollectionDto<WorkTypeDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixTypeResults = new PagedModelCollectionDto<WorkTypeDto>();

      var fixTypeQuery = _mdmContext.WorkTypes.Where(fixType => (paginationRequestDto.Name == null || fixType.Name.Trim().ToLower().Contains(paginationRequestDto.Name.ToLower().Trim()))
                                                                 && (paginationRequestDto.MinTimestampUtc == null || fixType.CreatedTimestampUtc >= paginationRequestDto.MinTimestampUtc)
                                                                 && (paginationRequestDto.MaxTimestampUtc == null || fixType.CreatedTimestampUtc <= paginationRequestDto.MaxTimestampUtc));

      int validPageSize = paginationRequestDto.PageSize.Equals(default(int)) ? PageSize : paginationRequestDto.PageSize.Value;
      var fixTypePagedResults = await fixTypeQuery.ToPagedListAsync(validPageSize, paginationRequestDto.PageNumber, cancellationToken);

      if (fixTypePagedResults != null && fixTypePagedResults.Any())
      {
        fixTypeResults = new PagedModelCollectionDto<WorkTypeDto>
        {
          Results = fixTypePagedResults.Select(fixType => _mapper.Map<WorkType, WorkTypeDto>(fixType)).ToList(),
          TotalModelCount = fixTypePagedResults.TotalCount,
          PageNumber = fixTypePagedResults.PageIndex
        };
      }

      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixTypeResults;
    }
  }
}

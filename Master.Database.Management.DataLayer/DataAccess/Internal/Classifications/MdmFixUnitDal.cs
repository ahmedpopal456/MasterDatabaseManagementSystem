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
  internal class MdmFixUnitDal : IMdmFixUnitDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmFixUnitDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmFixUnitDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmFixUnitDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<FixUnitDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}... null argument was provided");
      }

      var fixUnitDto = default(FixUnitDto);

      var fixUnitResult = await _mdmContext.FixUnits.FirstOrDefaultAsync(fixUnit => fixUnit.Id.Equals(id), cancellationToken);

      if (fixUnitResult != null)
      {
        fixUnitDto = _mapper.Map<FixUnit, FixUnitDto>(fixUnitResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return fixUnitDto;
    }

    public async Task<IEnumerable<FixUnitDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixUnitResult = _mdmContext.FixUnits.Where(fixUnit => (filterBaseDto.Name == null || fixUnit.Name.ToLower().Contains(filterBaseDto.Name.ToLower()))
                                                                 && (filterBaseDto.MinTimestampUtc == null || fixUnit.CreatedTimestampUtc >= filterBaseDto.MinTimestampUtc)
                                                                 && (filterBaseDto.MaxTimestampUtc == null || fixUnit.CreatedTimestampUtc <= filterBaseDto.MaxTimestampUtc)).AsEnumerable();

      var fixUnitDtos = fixUnitResult.Select(fixUnit => _mapper.Map<FixUnit, FixUnitDto>(fixUnit)).ToList();
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixUnitDtos;
    }

    public async Task<PagedModelCollectionDto<FixUnitDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixUnitResult = new PagedModelCollectionDto<FixUnitDto>();

      var fixUnitQuery = _mdmContext.FixUnits.Where(fixUnit => (paginationRequestDto.Name == null || fixUnit.Name.ToLower().Contains(paginationRequestDto.Name.ToLower()))
                                                                && (paginationRequestDto.MinTimestampUtc == null || fixUnit.CreatedTimestampUtc >= paginationRequestDto.MinTimestampUtc)
                                                                && (paginationRequestDto.MaxTimestampUtc == null || fixUnit.CreatedTimestampUtc <= paginationRequestDto.MaxTimestampUtc));

      int validPageSize = paginationRequestDto.PageSize.Equals(default(int)) ? PageSize : paginationRequestDto.PageSize.Value;
      var fixUnitByPage = await fixUnitQuery.ToPagedListAsync(validPageSize, paginationRequestDto.PageNumber, cancellationToken);
      if (fixUnitByPage != null && fixUnitByPage.Any())
      {
        fixUnitResult = new PagedModelCollectionDto<FixUnitDto>
        {
          Results = fixUnitByPage.Select(fixUnit => _mapper.Map<FixUnit, FixUnitDto>(fixUnit)).ToList(),
          TotalModelCount = fixUnitByPage.TotalCount,
          PageNumber = fixUnitByPage.PageIndex
        };
      }

      await _mdmContext.SaveChangesAsync(true, cancellationToken);
      return fixUnitResult;
    }
  }
}

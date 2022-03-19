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
  internal class MdmWorkCategoryDal : IMdmWorkCategoryDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmWorkCategoryDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmWorkCategoryDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmWorkCategoryDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<WorkCategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}... null argument was provided");
      }

      var workCategoryDto = default(WorkCategoryDto);

      var workCategoryResult = await _mdmContext.WorkCategories.FirstOrDefaultAsync(workCategory => workCategory.Id.Equals(id), cancellationToken);

      if (workCategoryResult != null)
      {
        workCategoryDto = _mapper.Map<WorkCategory, WorkCategoryDto>(workCategoryResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return workCategoryDto;
    }

    public async Task<IEnumerable<WorkCategoryDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var workCategoriesQuery = _mdmContext.WorkCategories.Where(workCategory => (filterBaseDto.Name == null || workCategory.Name.ToLower().Contains(filterBaseDto.Name.ToLower()))
                                                                                  && (filterBaseDto.MinTimestampUtc == null || workCategory.CreatedTimestampUtc >= filterBaseDto.MinTimestampUtc)
                                                                                  && (filterBaseDto.MaxTimestampUtc == null || workCategory.CreatedTimestampUtc <= filterBaseDto.MaxTimestampUtc));

      var workCategoriesResult = await workCategoriesQuery.ToListAsync();

      var workCategoryDtos = workCategoriesResult.Select(workCategory => _mapper.Map<WorkCategory, WorkCategoryDto>(workCategory)).ToList();
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return workCategoryDtos;
    }

    public async Task<PagedModelCollectionDto<WorkCategoryDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var workCategoriesResult = new PagedModelCollectionDto<WorkCategoryDto>();

      var workCategoryQuery = _mdmContext.WorkCategories.Where(workCategory => (paginationRequestDto.Name == null || workCategory.Name.ToLower().Contains(paginationRequestDto.Name.ToLower()))
                                                                                && (paginationRequestDto.MinTimestampUtc == null || workCategory.CreatedTimestampUtc >= paginationRequestDto.MinTimestampUtc)
                                                                                && (paginationRequestDto.MaxTimestampUtc == null || workCategory.CreatedTimestampUtc <= paginationRequestDto.MaxTimestampUtc));

      var validPageSize = paginationRequestDto.PageSize.Equals(default(int)) ? PageSize : paginationRequestDto.PageSize.Value;
      var workCategoriesByPage = await workCategoryQuery.ToPagedListAsync(validPageSize, paginationRequestDto.PageNumber, cancellationToken);

      if (workCategoriesByPage != null && workCategoriesByPage.Any())
      {
        workCategoriesResult = new PagedModelCollectionDto<WorkCategoryDto>
        {
          Results = workCategoriesByPage.Select(workCategory => _mapper.Map<WorkCategory, WorkCategoryDto>(workCategory)).ToList(),
          TotalModelCount = workCategoriesByPage.TotalCount,
          PageNumber = workCategoriesByPage.PageIndex
        };
      }

      await _mdmContext.SaveChangesAsync(true, cancellationToken);
      return workCategoriesResult;
    }
  }
}

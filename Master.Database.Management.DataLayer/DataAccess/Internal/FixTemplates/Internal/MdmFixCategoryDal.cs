using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts.Fixes.Categories;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal
{
  internal class MdmFixCategoryDal : IMdmFixCategoryDal
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;
    private const int PageSize = 20;

    public MdmFixCategoryDal(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(MdmFixCategoryDal)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(MdmFixCategoryDal)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public async Task<FixCategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (id.Equals(Guid.Empty))
      {
        throw new ArgumentNullException($"{nameof(GetByIdAsync)} expects id to be a valid {nameof(Guid)}, {id} was provided...");
      }

      var fixCategoryDto = default(FixCategoryDto);

      var fixCategoryResult = await _mdmContext.FixCategories.FirstOrDefaultAsync(fixCategory => fixCategory.Id.Equals(id), cancellationToken);

      if (fixCategoryResult != null)
      {
        fixCategoryDto = _mapper.Map<FixCategory, FixCategoryDto>(fixCategoryResult);
        await _mdmContext.SaveChangesAsync(true, cancellationToken);
      }

      return fixCategoryDto;
    }

    public async Task<IEnumerable<FixCategoryDto>> GetManyAsync(CancellationToken cancellationToken, string categoryName = null, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixCategoriesResult = _mdmContext.FixCategories.Where(fixCategory => (categoryName == null || fixCategory.Name.ToLower().Contains(categoryName.ToLower()))
                                                                                && (minTimestampUtc == null || fixCategory.CreatedTimestampUtc >= minTimestampUtc)
                                                                                && (maxTimestampUtc == null || fixCategory.CreatedTimestampUtc <= maxTimestampUtc)).AsEnumerable();

      var fixCategoryDtos = fixCategoriesResult.Select(fixCategory => _mapper.Map<FixCategory, FixCategoryDto>(fixCategory)).ToList();
      await _mdmContext.SaveChangesAsync(true, cancellationToken);

      return fixCategoryDtos;
    }

    public async Task<PagedModelCollectionDto<FixCategoryDto>> GetManyByPageAsync(CancellationToken cancellationToken, int currentPage, int? pageSize = null, string categoryName = null, long? startTimestampUtc = null, long? endTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var fixCategoriesResult = new PagedModelCollectionDto<FixCategoryDto>();

      var fixCategoryQuery = _mdmContext.FixCategories.Where(fixCategory => (categoryName == null || fixCategory.Name.ToLower().Equals(categoryName.ToLower()))
                                                                            && (startTimestampUtc == null || fixCategory.CreatedTimestampUtc > startTimestampUtc)
                                                                            && (endTimestampUtc == null || fixCategory.CreatedTimestampUtc < endTimestampUtc));

      int validPageSize = pageSize.Equals(default) ? PageSize : pageSize.Value;
      var fixCategoriesByPage = await fixCategoryQuery.ToPagedListAsync(validPageSize, currentPage, cancellationToken);
      if (fixCategoriesByPage != null && fixCategoriesByPage.Any())
      {
        fixCategoriesResult = new PagedModelCollectionDto<FixCategoryDto>
        {
          Results = fixCategoriesByPage.Select(fixCategory => _mapper.Map<FixCategory, FixCategoryDto>(fixCategory)).ToList(),
          TotalModelCount = fixCategoriesByPage.TotalCount,
          PageNumber = fixCategoriesByPage.PageIndex
        };
      }

      await _mdmContext.SaveChangesAsync(true, cancellationToken);
      return fixCategoriesResult;
    }
  }
}

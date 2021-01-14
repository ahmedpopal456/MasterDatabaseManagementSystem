using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Categories;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;

namespace Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Categories
{
  internal class FixCategoryMediator : IFixCategoryMediator
  {
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public FixCategoryMediator(IRequestMdmDalFactory requestMdmDalFactory)
    {
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(FixCategoryMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<FixCategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixCategoryDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<FixCategoryDto>> GetManyAsync(CancellationToken cancellationToken, string name = null, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixCategoryDal().GetManyAsync(cancellationToken, categoryName: name, minTimestampUtc, maxTimestampUtc);

      return result;
    }

    public async Task<PagedModelCollectionDto<FixCategoryDto>> GetManyByPageAsync(int currentPage, CancellationToken cancellationToken, int? pageSize = null, string name = null, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixCategoryDal().GetManyByPageAsync(cancellationToken, currentPage, pageSize, categoryName: name, minTimestampUtc, maxTimestampUtc);

      return result;
    }
  }
}

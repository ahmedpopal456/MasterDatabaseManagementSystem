using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;

namespace Master.Database.Management.ServerlessApi.Mediators.Internal.Classifications
{
  internal class WorkCategoryMediator : IWorkCategoryMediator
  {
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public WorkCategoryMediator(IRequestMdmDalFactory requestMdmDalFactory)
    {
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(WorkCategoryMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<WorkCategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmWorkCategoryDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<WorkCategoryDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmWorkCategoryDal().GetManyAsync(filterBaseDto, cancellationToken);

      return result;
    }

    public async Task<PagedModelCollectionDto<WorkCategoryDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmWorkCategoryDal().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return result;
    }
  }
}

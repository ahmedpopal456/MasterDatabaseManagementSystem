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
  internal class WorkTypeMediator : IWorkTypeMediator
  {
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public WorkTypeMediator(IRequestMdmDalFactory requestMdmDalFactory)
    {
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(WorkTypeMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<WorkTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmWorkTypeDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<WorkTypeDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmWorkTypeDal().GetManyAsync(filterBaseDto, cancellationToken);

      return result;
    }

    public async Task<PagedModelCollectionDto<WorkTypeDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmWorkTypeDal().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return result;
    }
  }
}

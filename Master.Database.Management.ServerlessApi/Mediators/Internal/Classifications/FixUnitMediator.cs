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
  internal class FixUnitMediator : IFixUnitMediator
  {
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public FixUnitMediator(IRequestMdmDalFactory requestMdmDalFactory)
    {
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(FixUnitMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<FixUnitDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixUnitDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<FixUnitDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixUnitDal().GetManyAsync(filterBaseDto, cancellationToken);

      return result;
    }

    public async Task<PagedModelCollectionDto<FixUnitDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixUnitDal().GetManyByPageAsync(paginationRequestDto, cancellationToken);

      return result;
    }
  }
}

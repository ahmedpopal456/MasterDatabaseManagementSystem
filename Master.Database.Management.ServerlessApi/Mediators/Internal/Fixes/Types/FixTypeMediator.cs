using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Types;
using Master.Database.Management.DataLayer.DataAccess;
using Master.Database.Management.DataLayer.DataAccess.Models;

namespace Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Types
{
  internal class FixTypeMediator : IFixTypeMediator
  {
    private readonly IRequestMdmDalFactory _requestMdmDalFactory;

    public FixTypeMediator(IRequestMdmDalFactory requestMdmDalFactory)
    {
      _requestMdmDalFactory = requestMdmDalFactory ?? throw new ArgumentNullException($"{nameof(FixTypeMediator)} expects a value for {nameof(requestMdmDalFactory)}... null argument was provided");
    }

    public async Task<FixTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTypeDal().GetByIdAsync(id, cancellationToken);

      return result;
    }

    public async Task<IEnumerable<FixTypeDto>> GetManyAsync(CancellationToken cancellationToken, string name = null, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTypeDal().GetManyAsync(fixTypeName: name, cancellationToken, minTimestampUtc, maxTimestampUtc);

      return result;
    }

    public async Task<PagedModelCollectionDto<FixTypeDto>> GetManyByPageAsync(int currentPage, CancellationToken cancellationToken, int? pageSize = null, string name = null, long? minTimestampUtc = null, long? maxTimestampUtc = null)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _requestMdmDalFactory.RequestMdmFixTypeDal().GetManyByPageAsync(currentPage, fixTypeName: name, cancellationToken, pageSize, minTimestampUtc, maxTimestampUtc);

      return result;
    }
  }
}

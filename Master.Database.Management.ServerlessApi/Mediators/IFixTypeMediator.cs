using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Types;
using Master.Database.Management.DataLayer.DataAccess.Models;

namespace Master.Database.Management.ServerlessApi.Mediators
{
  /// <summary>
  /// Handles all communication with the <see cref="Master.Database.Management.DataLayer.DataAccess.IMdmFixTypeDal"/>.
  /// </summary>
  public interface IFixTypeMediator
  {
    /// <summary>
    /// Finds the first <see cref="FixTypeDto"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixTypeDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<FixTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds all the <see cref="FixTypeDto"/> that matches the specified params.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="name"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="FixTypeDto"/>s, if found. 
    /// Otherwise returns an empty <see cref="IEnumerable{T}"/>.</returns>
    public Task<IEnumerable<FixTypeDto>> GetManyAsync(CancellationToken cancellationToken, string name = null, long? minTimestampUtc = null, long? maxTimestampUtc = null);

    /// <summary>
    /// Finds all the <see cref="FixTypeDto"/> that matches the specified params.
    /// </summary>
    /// <param name="currentPage"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageSize"></param>
    /// <param name="name"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="FixTypeDto"/>s, if found. 
    /// Otherwise returns a <see cref="PagedModelCollectionDto{T}"/> with default member values.</returns>
    public Task<PagedModelCollectionDto<FixTypeDto>> GetManyByPageAsync(int currentPage, CancellationToken cancellationToken, int? pageSize = null, string name = null, long? minTimestampUtc = null, long? maxTimestampUtc = null);
  }
}

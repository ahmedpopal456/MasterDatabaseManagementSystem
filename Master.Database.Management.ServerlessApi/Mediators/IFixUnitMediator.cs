using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;

namespace Master.Database.Management.ServerlessApi.Mediators
{
  /// <summary>
  /// Handles all communication with the <see cref="DataLayer.DataAccess.Classifications.IMdmFixUnitDal"/>.
  /// </summary>
  public interface IFixUnitMediator
  {
    /// <summary>
    /// Finds the first <see cref="FixUnitDto"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixUnitDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<FixUnitDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds all the <see cref="FixUnitDto"/> that matches the specified params.
    /// </summary>
    /// <param name="filterBaseDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="FixUnitDto"/>s, if found. 
    /// Otherwise returns an empty <see cref="IEnumerable{T}"/>.</returns>
    public Task<IEnumerable<FixUnitDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken);

    /// <summary>
    /// Finds all the <see cref="FixUnitDto"/> that matches the specified params.
    /// </summary>
    /// <param name="paginationRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="FixUnitDto"/>s, if found. 
    /// Otherwise returns a <see cref="PagedModelCollectionDto{T}"/> with default member values.</returns>
    public Task<PagedModelCollectionDto<FixUnitDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken);
  }
}

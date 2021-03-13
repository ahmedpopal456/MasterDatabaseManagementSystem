using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.DataAccess.Models.Filters;
using Master.Database.Management.DataLayer.DataAccess.Models.Paginations;

namespace Master.Database.Management.DataLayer.DataAccess.Classifications
{
  /// <summary>
  /// <para>A data access layer for the <see cref="DataLayer.Models.Classifications.WorkType"/> model.</para>
  /// <para>Provides an interface that allows for an asynchronous 'Get' (with the option of pagination).</para>
  /// </summary>
  public interface IMdmWorkTypeDal
  {
    /// <summary>
    /// Finds the first <see cref="DataLayer.Models.Classifications.WorkType"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of the 
    /// <see cref="FixType"/> model: <see cref="FixCategoryDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<WorkTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the all <see cref="DataLayer.Models.Classifications.WorkType"/> that matches the specified params.
    /// </summary>
    /// <param name="filterBaseDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="WorkTypeDto"/>s, if found. 
    /// Otherwise returns an empty <see cref="IEnumerable{T}"/>.</returns>
    public Task<IEnumerable<WorkTypeDto>> GetManyAsync(FilterBaseDto filterBaseDto, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Finds the all <see cref="DataLayer.Models.Classifications.WorkType"/> that matches the specified params.</para>
    /// <para>Then wraps the <see cref="List{T}"/> of <see cref="FixTypeDto"/>s to a <see cref="PagedModelCollectionDto{T}"/>.</para>
    /// </summary>
    /// <param name="paginationRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="WorkTypeDto"/>s, if found. 
    /// Otherwise returns the default value.</returns>
    public Task<PagedModelCollectionDto<WorkTypeDto>> GetManyByPageAsync(PaginationRequestBaseDto paginationRequestDto, CancellationToken cancellationToken);
  }
}

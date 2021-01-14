using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Types;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models;

namespace Master.Database.Management.DataLayer.DataAccess
{
  /// <summary>
  /// <para>A data access layer for the <see cref="FixType"/> model.</para>
  /// <para>Provides an interface that allows for an asynchronous 'Get' (with the option of pagination).</para>
  /// </summary>
  public interface IMdmFixTypeDal
  {
    /// <summary>
    /// Finds the first <see cref="FixType"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of the 
    /// <see cref="FixType"/> model: <see cref="FixCategoryDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<FixTypeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the all <see cref="FixType"/> that matches the specified params.
    /// </summary>
    /// <param name="fixTypeName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="FixTypeDto"/>s, if found. 
    /// Otherwise returns an empty <see cref="IEnumerable{T}"/>.</returns>
    public Task<IEnumerable<FixTypeDto>> GetManyAsync(string fixTypeName, CancellationToken cancellationToken, long? minTimestampUtc = null, long? maxTimestampUtc = null);

    /// <summary>
    /// <para>Finds the all <see cref="FixType"/> that matches the specified params.</para>
    /// <para>Then wraps the <see cref="List{T}"/> of <see cref="FixTypeDto"/>s to a <see cref="PagedModelCollectionDto{T}"/>.</para>
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="currentPage"></param>
    /// <param name="fixTypeName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="FixTypeDto"/>s, if found. 
    /// Otherwise returns the default value.</returns>
    public Task<PagedModelCollectionDto<FixTypeDto>> GetManyByPageAsync(int currentPage, string fixTypeName, CancellationToken cancellationToken, int? pageSize = null, long? minTimestampUtc = null, long? maxTimestampUtc = null);
  }
}

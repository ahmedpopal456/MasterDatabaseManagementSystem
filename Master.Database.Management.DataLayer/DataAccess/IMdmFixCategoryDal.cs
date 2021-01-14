using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.Fixes.Categories;
using Master.Database.Management.DataLayer.DataAccess.Models;

namespace Master.Database.Management.DataLayer.DataAccess
{
  /// <summary>
  /// <para>A data access layer for the <see cref="Master.Database.Management.DataLayer.Models.FixCategory"/> model.</para>
  /// <para>Provides an interface that allows for an asynchronous 'Get' (with the option of pagination).</para>
  /// </summary>
  public interface IMdmFixCategoryDal
  {
    /// <summary>
    /// Finds the first <see cref="Master.Database.Management.DataLayer.Models.FixCategory"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of the 
    /// <see cref="Master.Database.Management.DataLayer.Models.FixCategory"/> model: <see cref="FixCategoryDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<FixCategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds all the <see cref="Master.Database.Management.DataLayer.Models.FixCategory"/> that matches the specified params.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="categoryName"></param>
    /// <param name="startTimestampUtc"></param>
    /// <param name="endTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="FixCategoryDto"/>s, if found. 
    /// Otherwise returns an empty <see cref="IEnumerable{T}"/>.</returns>
    public Task<IEnumerable<FixCategoryDto>> GetManyAsync(CancellationToken cancellationToken, string categoryName = null, long? startTimestampUtc = null, long? endTimestampUtc = null);

    /// <summary>
    /// <para>Finds all the<see cref="Master.Database.Management.DataLayer.Models.FixCategory"/> that matches the specified params.</para>
    /// <para>Then wraps the <see cref="List{T}"/> of <see cref="FixCategoryDto"/>s to a <see cref="PagedModelCollectionDto{T}"/>.</para>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="pageSize"></param>
    /// <param name="currentPage"></param>
    /// <param name="categoryName"></param>
    /// <param name="startTimestampUtc"></param>
    /// <param name="endTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="FixCategoryDto"/>s, if found. 
    /// Otherwise returns the default value.</returns>
    public Task<PagedModelCollectionDto<FixCategoryDto>> GetManyByPageAsync(CancellationToken cancellationToken, int currentPage, int? pageSize = null, string categoryName = null, long? startTimestampUtc = null, long? endTimestampUtc = null);
  }
}

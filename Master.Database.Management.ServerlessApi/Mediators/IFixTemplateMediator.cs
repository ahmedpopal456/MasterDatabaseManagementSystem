using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Models;

namespace Master.Database.Management.ServerlessApi.Mediators
{
  /// <summary>
  /// Handles all communication with the <see cref="Master.Database.Management.DataLayer.DataAccess.IMdmFixTemplateDal"/>.
  /// </summary>
  public interface IFixTemplateMediator
  {
    /// <summary>
    /// <para>Creates a <see cref="Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate"/> Object.</para>
    /// </summary>
    /// <param name="fixTemplateCreateRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> CreateAsync(FixTemplateCreateRequestDto fixTemplateCreateRequestDto, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Finds the first <see cref="Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate"/> with the matching <see cref="Guid"/>.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Finds the all <see cref="Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate"/> that matches the specified params.</para>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="userId"></param>
    /// <param name="tags"></param>
    /// <param name="status"></param>
    /// <param name="typeName"></param>
    /// <param name="categoryName"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="FixTemplateDto"/>s, if found. 
    /// Otherwise returns an empty <see cref="IEnumerable{T}"/>.</returns>
    public Task<IEnumerable<FixTemplateDto>> GetManyAsync(CancellationToken cancellationToken, Guid? userId = null, string[] tags = null, FixTemplateStatus? status = null, string typeName = null, string categoryName = null, long? minTimestampUtc = null, long? maxTimestampUtc = null);

    /// <summary>
    /// <para>Finds the all <see cref="Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate"/> that matches the specified params.</para>
    /// </summary>
    /// <param name="currentPage"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageSize"></param>
    /// <param name="userId"></param>
    /// <param name="tags"></param>
    /// <param name="status"></param>
    /// <param name="typeName"></param>
    /// <param name="categoryName"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="FixTemplateDto"/>s, if found. 
    /// Otherwise returns a <see cref="PagedModelCollectionDto{T}"/> with default member values.</returns>
    public Task<PagedModelCollectionDto<FixTemplateDto>> GetManyByPageAsync(int currentPage, CancellationToken cancellationToken, int? pageSize = null, Guid? userId = null, string[] tags = null, FixTemplateStatus? status = null, string typeName = null, string categoryName = null, long? minTimestampUtc = null, long? maxTimestampUtc = null);

    /// <summary>
    /// <para>Updates an existing <see cref="FixTemplate"/> Object.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fixTemplateUpdateRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> UpdateAsync(Guid id, FixTemplateUpdateRequestDto fixTemplateUpdateRequestDto, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Updates an existing <see cref="FixTemplate"/> Object with the specified 'cost'.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cost"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> UpdateCostAsync(Guid id, double cost, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Updates an existing <see cref="FixTemplate"/> Object with the specified <see cref="FixTemplateStatus"/>.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fixTemplateStatus"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> UpdateStatusAsync(Guid id, FixTemplateStatus fixTemplateStatus, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Permanently removes an existing <see cref="Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate"/> Object.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="OperationStatus"/> Object.</returns>
    public Task<OperationStatus> DeleteAsync(Guid id, bool softDelete, CancellationToken cancellationToken);
  }
}

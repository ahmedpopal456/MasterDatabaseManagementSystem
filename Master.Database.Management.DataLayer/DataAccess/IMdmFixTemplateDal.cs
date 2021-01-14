using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models.FixTemplates;

namespace Master.Database.Management.DataLayer.DataAccess
{
  /// <summary>
  /// <para>A data access layer for the <see cref="FixTemplate"/> model.</para>
  /// <para>Provides an interface that allows for an asynchronous 'Create', 'Update', 'Delete' and 'Get' (with the option of pagination).</para>
  /// </summary>
  public interface IMdmFixTemplateDal
  {
    /// <summary>
    /// <para>Creates a <see cref="FixTemplate"/> Object 
    /// and adds it to the <see cref="Microsoft.EntityFrameworkCore.DbSet{FixTemplate}"/> of FixTemplates in the <see cref="MdmContext"/>.</para>
    /// <para>Calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the create request.</para>
    /// </summary>
    /// <param name="fixTemplateDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of 
    /// the <see cref="FixTemplate"/> model: <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> CreateAsync(FixTemplateDto fixTemplateDto, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the first <see cref="FixTemplate"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="fixTemplateDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of the 
    /// <see cref="FixTemplate"/> model: <see cref="FixTemplateDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<FixTemplateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the all <see cref="FixTemplate"/> that matches the specified params.
    /// </summary>
    /// <param name="cancellationToken"></param>
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
    /// <para>Finds the all <see cref="FixTemplate"/> that matches the specified params.</para>
    /// <para>Then wraps the <see cref="List{T}"/> of <see cref="FixTemplateDto"/>s to a <see cref="PagedModelCollectionDto{T}"/>.</para>
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="currentPage"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="tags"></param>
    /// <param name="status"></param>
    /// <param name="typeName"></param>
    /// <param name="categoryName"></param>
    /// <param name="minTimestampUtc"></param>
    /// <param name="maxTimestampUtc"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a <see cref="PagedModelCollectionDto{T}"/> of <see cref="FixTemplateDto"/>s, if found. 
    /// Otherwise returns the default value.</returns>
    public Task<PagedModelCollectionDto<FixTemplateDto>> GetManyByPageAsync(int currentPage, CancellationToken cancellationToken, int? pageSize = null, Guid? userId = null, string[] tags = null, FixTemplateStatus? status = null, string typeName = null, string categoryName = null, long? minTimestampUtc = null, long? maxTimestampUtc = null);


    /// <summary>
    /// <para>Updates an existing <see cref="FixTemplate"/> Object 
    /// amongst the <see cref="Microsoft.EntityFrameworkCore.DbSet{FixTemplate}"/> of FixTemplates in the <see cref="MdmContext"/>.</para>
    /// <para>Calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the update request.</para>
    /// </summary>
    /// <param name="fixTemplateDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of 
    /// the <see cref="FixTemplate"/> model: <see cref="FixTemplateDto"/>.</returns>
    public Task<FixTemplateDto> UpdateAsync(FixTemplateDto fixTemplateDto, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Permanently removes an existing <see cref="FixTemplate"/> Object 
    /// amongst the <see cref="Microsoft.EntityFrameworkCore.DbSet{FixTemplate}"/> of FixTemplates in the <see cref="MdmContext"/>, if 'isSoftDelete' is false.</para>
    /// <para>Otherwise, updates the model's <see cref="FixTemplate.IsDeleted"/> property.</para>
    /// <para>Calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the delete request.</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="isSoftDeleted"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an <see cref="OperationStatus"/> Object.</returns>
    public Task<OperationStatus> DeleteAsync(Guid id, CancellationToken cancellationToken, bool isSoftDeleted = true);

  }
}

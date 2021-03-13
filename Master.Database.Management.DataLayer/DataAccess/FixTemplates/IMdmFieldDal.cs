using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Fields;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;

namespace Master.Database.Management.DataLayer.DataAccess.FixTemplates
{
  /// <summary>
  /// <para>A data access layer for the <see cref="Field"/> model.</para>
  /// <para>Provides an interface that allows for an asynchronous 'Create' and 'Get'.</para>
  /// </summary>
  public interface IMdmFieldDal
  {
    /// <summary>
    /// <para>Creates a <see cref="Field"/> Object 
    /// and adds it to the <see cref="Microsoft.EntityFrameworkCore.DbSet{Field}"/> of Fields in the <see cref="MdmContext"/>.</para>
    /// <para>Calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the create request.</para>
    /// </summary>
    /// <param name="fieldCreateRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of 
    /// the <see cref="Field"/> model: <see cref="FieldDto"/>.</returns>
    public Task<FieldDto> CreateAsync(FieldCreateRequestDto fieldCreateRequestDto, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Finds all existing <see cref="FieldCreateRequestDto"/> in the <see cref="MdmContext"/> by a match of <see cref="Field.Name"/>.</para>
    /// <para>Then creates any unmatched <see cref="FieldCreateRequestDto"/>s.</para>
    /// <para>Finally, calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the request.</para>
    /// </summary>
    /// <param name="fieldCreateRequestDtos"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of 
    /// <see cref="MdmResponseDto{List{FieldDto}}"/> that wraps a <see cref="List{T}"/> of <see cref="FieldDto"/>.</returns>
    public Task<MdmResponseDto<List<FieldDto>>> GetOrCreateManyAsync(IEnumerable<FieldCreateRequestDto> fieldCreateRequestDtos, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the first <see cref="Field"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The Field Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of the 
    /// <see cref="Field"/> model: <see cref="FieldDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<FieldDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.Sections;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Master.Database.Management.DataLayer.DataAccess.Models;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;

namespace Master.Database.Management.DataLayer.DataAccess
{
  /// <summary>
  /// <para>A data access layer for the <see cref="Section"/> model.</para>
  /// <para>Provides an interface that allows for an asynchronous 'Create' and 'Get'.</para>
  /// </summary>
  public interface IMdmSectionDal
  {
    /// <summary>
    /// <para>Creates a <see cref="Section"/> Object 
    /// and adds it to the <see cref="Microsoft.EntityFrameworkCore.DbSet{Field}"/> of Fields in the <see cref="MdmContext"/>.</para>
    /// <para>Calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the create request.</para>
    /// </summary>
    /// <param name="sectionCreateRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of 
    /// the <see cref="Section"/> model: <see cref="SectionDto"/>.</returns>
    public Task<SectionDto> CreateAsync(SectionCreateRequestDto sectionCreateRequestDto, CancellationToken cancellationToken);

    /// <summary>
    /// <para>Finds all existing <see cref="SectionCreateRequestDto"/> in the <see cref="MdmContext"/> by a match of <see cref="Section.Name"/>.</para>
    /// <para>Then creates any unmatched <see cref="SectionCreateRequestDto"/>s.</para>
    /// <para>Finally, calls the <see cref="MdmContext.SaveChangesAsync(bool, CancellationToken)"/> method after attempting to finalize the request.</para>
    /// </summary>
    /// <param name="sectionCreateRequestDtos"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of 
    /// <see cref="MdmResponseDto{T}"/> that wraps a <see cref="List{T}"/> of <see cref="SectionDto"/>.</returns>
    public Task<MdmResponseDto<List<SectionDto>>> GetOrCreateManyAsync(IEnumerable<SectionCreateRequestDto> sectionCreateRequestDtos, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the first <see cref="Section"/> with the matching <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains an equivalent model representation of the 
    /// <see cref="Section"/> model: <see cref="SectionDto"/>, if found. Otherwise returns the default value.</returns>
    public Task<SectionDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  }
}

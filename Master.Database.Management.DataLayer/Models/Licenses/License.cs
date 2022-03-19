using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Models.Extensions;
using Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities;

namespace Master.Database.Management.DataLayer.Models.FixTemplates
{
  public class License : IAuditable, ITimeTraceable, ISoftDeletable
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public float ReferenceId  { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; }

    [Required]
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    public virtual ICollection<LicenseTag> Tags { get; set; }

    #region ITimeTraceable

    public long CreatedTimestampUtc { get; set; }

    public long UpdatedTimestampUtc { get; set; }

    public long LastAccessedTimestampUtc { get; set; }

    #endregion

    #region ISoftDeletable

    public bool IsDeleted { get; set; }

    public long DeletedTimestampUtc { get; set; }

    #endregion
  }
}

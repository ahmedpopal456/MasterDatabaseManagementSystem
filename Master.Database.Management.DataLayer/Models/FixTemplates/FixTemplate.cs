using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fixit.Core.DataContracts.FixTemplates;
using Master.Database.Management.DataLayer.Models.Classifications;
using Master.Database.Management.DataLayer.Models.Extensions;
using Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities;

namespace Master.Database.Management.DataLayer.Models.FixTemplates
{
  public class FixTemplate : IAuditable, ITimeTraceable, ISoftDeletable
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public FixTemplateStatus Status { get; set; }

    [Required]
    [MaxLength(32)]
    public string Name { get; set; }

    [Required]
    [ForeignKey("WorkType")]
    public Guid WorkTypeId { get; set; }

    public virtual WorkType WorkType { get; set; }

    [Required]
    [ForeignKey("WorkCategory")]
    public Guid WorkCategoryId { get; set; }

    public virtual WorkCategory WorkCategory { get; set; }

    [Required]
    [ForeignKey("FixUnit")]
    public Guid FixUnitId { get; set; }

    public virtual FixUnit FixUnit { get; set; }

    [Required]
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    public double SystemCostEstimate { get; set; }

    public Guid CreatedByUserId { get; set; }

    public Guid UpdatedByUserId { get; set; }

    public virtual ICollection<FixTemplateTag> Tags { get; set; }

    public virtual ICollection<FixTemplateSection> FixTemplateSections { get; set; }

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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Models.Classifications.WeakEntities;
using Master.Database.Management.DataLayer.Models.Extensions;

namespace Master.Database.Management.DataLayer.Models.Classifications
{
  public class WorkCategory : ITimeTraceable, ISoftDeletable
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(32)]
    public string Name { get; set; }

    public virtual ICollection<WorkCategorySkills> WorkCategorySkills { get; set; }

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
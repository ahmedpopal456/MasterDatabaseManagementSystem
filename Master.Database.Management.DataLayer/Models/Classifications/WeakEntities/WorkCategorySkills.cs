using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.DataLayer.Models.Classifications.WeakEntities
{
  public class WorkCategorySkills
  {
    [ForeignKey("WorkCategory")]
    public Guid WorkCategoryId { get; set; }

    public virtual WorkCategory WorkCategory { get; set; }

    [ForeignKey("Skill")]
    public Guid SkillId { get; set; }

    public virtual Skill Skill { get; set; }
  }
}

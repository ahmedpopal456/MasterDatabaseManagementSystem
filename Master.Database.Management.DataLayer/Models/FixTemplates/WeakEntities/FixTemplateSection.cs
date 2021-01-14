using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities
{
  public class FixTemplateSection
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("Section")]
    public Guid SectionId { get; set; }

    public virtual Section Section { get; set; }

    [ForeignKey("FixTemplate")]
    public Guid FixTemplateId { get; set; }

    public virtual FixTemplate FixTemplate { get; set; }

    public virtual ICollection<FixTemplateSectionField> FixTemplateSectionFields { get; set; }
  }
}

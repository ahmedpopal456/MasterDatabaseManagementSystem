using System;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities
{
  public class FixTemplateSectionField
  {
    [ForeignKey("FixTemplateSection")]
    public Guid FixTemplateSectionId { get; set; }

    public virtual FixTemplateSection FixTemplateSection { get; set; }

    [ForeignKey("Field")]
    public Guid FieldId { get; set; }

    public virtual Field Field { get; set; }

    public string Value { get; set; }
  }
}

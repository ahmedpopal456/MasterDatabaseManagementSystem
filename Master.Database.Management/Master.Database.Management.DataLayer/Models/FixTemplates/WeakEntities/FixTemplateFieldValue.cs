using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities
{
  public class FixTemplateFieldValue
  {
    [ForeignKey("Field")]
    public Guid FieldId { get; set; }

    public virtual Field Field { get; set; }

    [Required]
    [MaxLength(32)]
    public string Value { get; set; }
  }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities
{
  public class FixTemplateLicense
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("License")]
    public Guid LicenseId { get; set; }

    public virtual License License { get; set; }

    [ForeignKey("FixTemplate")]
    public Guid FixTemplateId { get; set; }

    public virtual FixTemplate FixTemplate { get; set; }
  }
}

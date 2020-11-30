using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.Data.Enums;
using Master.Database.Management.Data.Audits;
using Master.Database.Management.Data.Models.FixTemplates.Sections;

namespace Master.Database.Management.Data.Models
{
	public class FixTemplate : SoftDeletable
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required]
		public FixTemplateStatus Status { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }

		[Required, ForeignKey("FixCategoryId")]
		public Guid FixCategoryId { get; set; }

		public virtual FixCategory FixCategory { get; set; }

		[Required, ForeignKey("FixTypeId")]
		public Guid FixTypeId { get; set; }

		public virtual FixType FixType { get; set; }

		[Required, StringLength(int.MaxValue)]
		public string Description { get; set; }

		public double SystemCostEstimate { get; set; }

		public virtual ICollection<FixTemplateTag> FixTemplateTags { get; set; }

		public virtual ICollection<FixTemplateSectionField> FixTemplateSectionFields { get; set; }
	}
}

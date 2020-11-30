using Master.Database.Management.Data.Models.FixTemplates.Segments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Database.Management.Data.Models.FixTemplates.Sections
{
	public class FixTemplateSectionField
	{
		[ForeignKey("FixTemplate")]
		public Guid FixTemplateId { get; set; }

		public virtual FixTemplate FixTemplate { get; set; }

		[ForeignKey("Section")]
		public Guid SectionId { get; set; }

		public virtual Section Section { get; set; }

		[ForeignKey("Field")]
		public Guid FieldId { get; set; }

		public virtual ICollection<Field> Fields { get; set; }
	}
}

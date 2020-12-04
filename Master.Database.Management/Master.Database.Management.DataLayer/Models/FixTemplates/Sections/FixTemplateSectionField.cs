using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Master.Database.Management.DataLayer.Interfaces;
using Master.Database.Management.DataLayer.Models.FixTemplates.Segments;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.Sections
{
	public class FixTemplateSectionField : ISoftDeletable
	{
		#region MainProperties

		[ForeignKey("FixTemplate")]
		public Guid FixTemplateId { get; set; }

		public virtual FixTemplate FixTemplate { get; set; }

		[ForeignKey("Section")]
		public Guid SectionId { get; set; }

		public virtual Section Section { get; set; }

		[ForeignKey("Field")]
		public Guid FieldId { get; set; }

		public virtual ICollection<Field> Fields { get; set; }

		#endregion

		#region ISoftDeletable
		public bool IsDeleted { get; set; }

		public long DeletedTimestampUtc { get; set; }

		#endregion
	}
}

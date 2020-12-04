using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Master.Database.Management.DataLayer.Models.FixTemplates.Segments;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.Sections
{
	public class FixTemplateFieldValue
	{
		[ForeignKey("Field")]
		public Guid FieldId { get; set; }

		public virtual Field Field { get; set; }

		[Required, MaxLength(32)]
		public string Value { get; set; }
	}
}

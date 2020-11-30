using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.Data.Models.FixTemplates.Segments;
using System.ComponentModel.DataAnnotations;

namespace Master.Database.Management.Data.Models.FixTemplates.Sections
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

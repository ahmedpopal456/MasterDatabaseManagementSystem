using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Master.Database.Management.Data.Models.FixTemplates.Sections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.Data.Models.FixTemplates.Segments
{
	public class Section
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }

		public virtual FixTemplateSectionField FixTemplateSectionField { get; set; }
	}
}

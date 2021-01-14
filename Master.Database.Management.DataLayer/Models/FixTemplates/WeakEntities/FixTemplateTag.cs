using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities
{
	public class FixTemplateTag
	{
		[ForeignKey("FixTemplate")]
		public Guid FixTemplateId { get; set; }

		public virtual FixTemplate FixTemplate { get; set; }

		[Required]
		[MaxLength(32)]
		public string Name { get; set; }
	}
}

using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.Data.Models
{
	public class FixTemplateTag
	{
		[ForeignKey("FixTemplate")]
		public Guid FixTemplateId { get; set; }

		[Required, MaxLength(32)]
		public string TagName { get; set; }

		public virtual FixTemplate FixTemplate { get; set; }
	}
}

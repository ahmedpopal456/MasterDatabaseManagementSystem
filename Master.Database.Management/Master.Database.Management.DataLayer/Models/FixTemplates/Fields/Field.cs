using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.Fields
{
	public class Field
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }

		public virtual ICollection<FixTemplateFieldValue> FixTemplateFieldValues { get; set; }
	}
}

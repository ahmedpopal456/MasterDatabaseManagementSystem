using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Models.FixTemplates;

namespace Master.Database.Management.DataLayer.Models
{
	public class FixCategory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }

		public virtual ICollection<FixTemplate> FixTemplates { get; set; }
	}
}

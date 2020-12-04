using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.Sections
{
	public class Section
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }
	}
}

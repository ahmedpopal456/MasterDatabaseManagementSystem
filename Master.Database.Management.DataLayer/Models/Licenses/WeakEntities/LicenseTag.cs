using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities
{
	public class LicenseTag
  {
		[ForeignKey("License")]
		public Guid LicenseId { get; set; }

		public virtual License License { get; set; }

		[Required]
		[MaxLength(32)]
		public string Name { get; set; }
	}
}

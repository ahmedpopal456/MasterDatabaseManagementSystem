using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Interfaces;

namespace Master.Database.Management.DataLayer.Models.FixTemplates
{
	public class FixTemplateTag : ISoftDeletable
	{
		#region MainProperties

		[ForeignKey("FixTemplate")]
		public Guid FixTemplateId { get; set; }

		public virtual FixTemplate FixTemplate { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }

		#endregion

		#region ISoftDeletable
		public bool IsDeleted { get; set; }

		public long DeletedTimestampUtc { get; set; }

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Master.Database.Management.DataLayer.Interfaces;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;

namespace Master.Database.Management.DataLayer.Models.FixTemplates
{
	public class FixTemplate : ISoftDeletable, IAuditable
	{
		#region MainProperties

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required]
		public FixTemplateStatus Status { get; set; }

		[Required, MaxLength(32)]
		public string Name { get; set; }

		[Required, ForeignKey("FixCategoryId")]
		public Guid CategoryId { get; set; }

		public virtual FixCategory Category { get; set; }

		[Required, ForeignKey("FixTypeId")]
		public Guid TypeId { get; set; }

		public virtual FixType Type { get; set; }

		[Required, StringLength(int.MaxValue)]
		public string Description { get; set; }

		public double SystemCostEstimate { get; set; }

		public Guid CreatedByUserId { get; set; }

		public Guid UpdatedByUserId { get; set; }

		public virtual ICollection<FixTemplateTag> Tags { get; set; }

		public virtual ICollection<FixTemplateSectionField> SectionFields { get; set; }

		#endregion

		#region ISoftDeletable
		public bool IsDeleted { get; set; }

		public long DeletedTimestampUtc { get; set; }
		#endregion

		#region IAuditable

		public long CreatedTimestampUtc { get; set; }

		public long UpdatedTimestampUtc { get; set; }

		public long LastAccessedTimestampUtc { get; set; }

		#endregion
	}
}

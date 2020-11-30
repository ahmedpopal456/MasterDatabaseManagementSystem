using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Database.Management.Data.Audits
{
	public abstract class SoftDeletable : Auditable
	{
		public virtual bool? IsDeleted { get; set; }

		public virtual Guid? DeletedByUser { get; set; }

		public virtual byte[] DeletedTimestampUtc { get; set; }

	}
}

using System;
using System.Text;
using System.Collections.Generic;

namespace Master.Database.Management.Data.Audits
{
	public abstract class Auditable
	{
		public virtual Guid? CreatedByUser { get; set; }

		public virtual Guid? UpdatedByUser { get; set; }

		public virtual byte[] CreatedTimestampUtc { get; set; }

		public virtual byte[] UpdatedTimestampUtc { get; set; }

		public virtual byte[] LastAccessedTimestampUtc { get; set; }

		public virtual int? Frequency { get; set; }
	}
}

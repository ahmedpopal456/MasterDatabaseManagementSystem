using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Master.Database.Management.DataLayer.Interfaces;

namespace Master.Database.Management.DataLayer
{
	public class MdmContext : MdmBaseContext
	{
		private const string _configurationFileName = "appsettings.json";

		public MdmContext() : base() { }

		public MdmContext(DbContextOptions<MdmBaseContext> options) : base(options) { }

		#region SoftDeletable
		public void EnsureSoftDelete()
		{
			ChangeTracker.DetectChanges();

			var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
			long unixTimeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			foreach (var item in markedAsDeleted)
			{
				if (item.Entity is ISoftDeletable entity)
				{
					item.State = EntityState.Unchanged;
					entity.IsDeleted = true;
					entity.DeletedTimestampUtc = unixTimeNow;
				}
			}
		}
		#endregion

		#region Auditable
		public void EnsureAuditTimestamp()
		{
			ChangeTracker.DetectChanges();

			var markedAsAuditable = ChangeTracker.Entries().Where(x => x.Entity is IAuditable);
			long unixTimeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			foreach (var item in markedAsAuditable)
			{
				var entity = item.Entity as IAuditable;

				switch (item.State)
				{
					case EntityState.Unchanged:
					{
						entity.LastAccessedTimestampUtc = unixTimeNow;
						break;
					}
					case EntityState.Added:
					{
						entity.CreatedTimestampUtc = unixTimeNow;
						break;
					}
					case EntityState.Modified:
					{
						entity.UpdatedTimestampUtc = unixTimeNow;
						break;
					}
				}
			}
		}
		#endregion

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(_configurationFileName)
				.Build();

				var connectionString = configuration["FIXIT-MDM-SA-CS"];
				optionsBuilder.UseSqlServer(connectionString);
			}
		}

		public override int SaveChanges()
		{
			EnsureSoftDelete();
			EnsureAuditTimestamp();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
		{
			EnsureSoftDelete();
			EnsureAuditTimestamp();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}

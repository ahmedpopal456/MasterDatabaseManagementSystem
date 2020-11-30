using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Master.Database.Management.Data.Audits;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Master.Database.Management.Data
{
	public class MdmContext : MdmBaseContext
	{
		private const string _configurationFileName = "appsettings.json";

		public MdmContext() : base() { }

		public MdmContext(DbContextOptions<MdmBaseContext> options) : base(options) { }

		#region SoftDeletable
		public void EnsureSoftDeletable(Guid id)
		{
			ChangeTracker.DetectChanges();

			var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

			foreach (var item in markedAsDeleted)
			{
				if (item.Entity is SoftDeletable entity)
				{
					item.State = EntityState.Unchanged;
					entity.IsDeleted = true;
					entity.DeletedTimestampUtc = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
					entity.DeletedByUser = id;
				}
			}
		}
		#endregion

		#region Autidable
		public void EnsureAudit(Guid id)
		{
			ChangeTracker.DetectChanges();

			var markedAsRead = ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged);

			foreach (var item in markedAsRead)
			{
				if (item.Entity is Auditable entity)
				{
					entity.LastAccessedTimestampUtc = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
					entity.Frequency += 1;
				}
			}

			var markedAsCreated = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

			foreach (var item in markedAsCreated)
			{
				if (item.Entity is Auditable entity)
				{
					entity.CreatedTimestampUtc = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
					entity.CreatedByUser = id;
					entity.Frequency = 1;
				}
			}

			var markedAsUpdated = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

			foreach (var item in markedAsUpdated)
			{
				if (item.Entity is Auditable entity)
				{
					entity.UpdatedTimestampUtc = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
					entity.CreatedByUser = id;
					entity.Frequency += 1;
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
			//var user = GetCurrentUserInfo();
			EnsureAudit(Guid.Empty);
			EnsureSoftDeletable(Guid.Empty);
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
		{
			//var user = GetCurrentUserInfo();
			EnsureAudit(Guid.Empty);
			EnsureSoftDeletable(Guid.Empty);
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		#region Helpers
		/*private async Task GetCurrentUserInfo()
		{
			//TODO: call user management system
			return;
		}*/
		#endregion
	}
}

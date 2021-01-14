using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Master.Database.Management.DataLayer.Models.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Master.Database.Management.DataLayer
{
  public class MdmContext : MdmBaseContext
  {
    private const string _configurationFileName = "appsettings.json";

    public MdmContext() { }

    public MdmContext(DbContextOptions<MdmContext> options) : base(options) { }

    #region SoftDeletable

    public void EnsureSoftDelete()
    {
      ChangeTracker.DetectChanges();

      IEnumerable<EntityEntry> markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
      long unixTimeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

      foreach (EntityEntry item in markedAsDeleted)
        if (item.Entity is ISoftDeletable entity)
        {
          item.State = entity.IsDeleted ? EntityState.Unchanged : EntityState.Deleted;
          entity.DeletedTimestampUtc = unixTimeNow;
        }
    }

    #endregion

    #region Auditable

    public void EnsureAuditTimestamp()
    {
      ChangeTracker.DetectChanges();

      IEnumerable<EntityEntry> markedAsAuditable = ChangeTracker.Entries().Where(x => x.Entity is ITimeTraceable);
      long unixTimeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

      foreach (var item in markedAsAuditable)
      {
        if (item.Entity is ITimeTraceable entity)
        {
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
    }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (optionsBuilder.IsConfigured) return;
      IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(_configurationFileName)
        .Build();

      string connectionString = configuration["FIXIT-MDM-DB-CS"];
      optionsBuilder.UseSqlServer(connectionString);
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

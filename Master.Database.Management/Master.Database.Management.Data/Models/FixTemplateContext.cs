using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Master.Database.Management.Data.Models.FixTemplates;
using System.Threading.Tasks;
using System.Threading;

namespace Master.Database.Management.Data.Models
{
    public class FixTemplateContext : DbContext
    {
        public FixTemplateContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Type>().ToTable("Type");
            builder.Entity<Section>().ToTable("Section");
            builder.Entity<Field>().ToTable("Field");
            builder.Entity<Value>().ToTable("Value");

            builder.Entity<FixTemplate>().Property<bool>("IsDeleted");
            builder.Entity<FixTemplate>().HasQueryFilter(m => EF.Property<bool>(m, "isDeleted") == false);
            builder.Entity<FixTemplate>().ToTable("FixTemplate");

            builder.Entity<FixTemplateTag>().HasKey(ftt => new { ftt.FixTemplateId, ftt.TagName });
            builder.Entity<FixTemplateTag>().ToTable("FixTemplateTag");

            builder.Entity<SectionField>().HasKey(sf => new { sf.FixTemplateId, sf.SectionId, sf.FieldId });
            builder.Entity<SectionField>().ToTable("SectionField");

            builder.Entity<FieldValue>().HasKey(fv => new { fv.FieldId, fv.ValueId });
            builder.Entity<FieldValue>().ToTable("FieldValue");
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}

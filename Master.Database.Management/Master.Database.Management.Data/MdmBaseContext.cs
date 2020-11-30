using Master.Database.Management.Data.Models;
using Master.Database.Management.Data.Models.FixTemplates.Sections;
using Master.Database.Management.Data.Models.FixTemplates.Segments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Master.Database.Management.Data
{
	public class MdmBaseContext : DbContext
  {
    public MdmBaseContext() : base() { }

    public MdmBaseContext(DbContextOptions options) : base(options) { }

    #region Dbsets
		public DbSet<FixCategory> FixCategories { get; set; }

    public DbSet<FixType> FixTypes { get; set; }

    public DbSet<FixTemplate> FixTemplates { get; set; }

    public DbSet<FixTemplateTag> FixTemplateTags { get; set; }

    public DbSet<Section> Sections { get; set; }

    public DbSet<Field> Fields { get; set; }

    public DbSet<FixTemplateSectionField> FixTemplateSectionFields { get; set; }

    public DbSet<FixTemplateFieldValue> FixTemplateFieldValues { get; set; }
    #endregion

		protected override void OnModelCreating(ModelBuilder builder)
    {
      /* Map entity to table */
      builder.Entity<Section>().ToTable("FixTemplateSection");
      builder.Entity<Field>().ToTable("FixTemplateField");

      builder.Entity<FixTemplate>().HasQueryFilter(p => p.IsDeleted == false);

      /* Composite Keys */
      builder.Entity<FixTemplateTag>().HasKey(ftt => new { ftt.FixTemplateId, ftt.TagName });
      builder.Entity<FixTemplateSectionField>().HasKey(sf => new { sf.FixTemplateId, sf.SectionId, sf.FieldId });
      builder.Entity<FixTemplateFieldValue>().HasKey(fv => new { fv.FieldId, fv.Value });

      /* Entity Mapping */
      builder.Entity<FixTemplateTag>()
        .HasOne(ftt => ftt.FixTemplate)
        .WithMany(ft => ft.FixTemplateTags)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplate>()
        .HasOne(t => t.FixType);

      builder.Entity<FixTemplate>()
        .HasOne(c => c.FixCategory);

      builder.Entity<FixTemplateSectionField>()
        .HasOne(sf => sf.FixTemplate)
        .WithMany(temp => temp.FixTemplateSectionFields)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplateSectionField>()
        .HasOne(sf => sf.Section)
        .WithOne(s => s.FixTemplateSectionField)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplateSectionField>()
        .HasMany(sf => sf.Fields);

      builder.Entity<FixTemplateFieldValue>()
        .HasOne(fv => fv.Field)
        .WithMany(f => f.FixTemplateFieldValues)
        .OnDelete(DeleteBehavior.Cascade);
    }

    public override int SaveChanges()
    {
      return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
  }
}

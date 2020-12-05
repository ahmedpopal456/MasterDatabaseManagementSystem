using System.Threading;
using System.Threading.Tasks;
using Master.Database.Management.DataLayer.Models;
using Master.Database.Management.DataLayer.Models.FixTemplates;
using Master.Database.Management.DataLayer.Models.FixTemplates.Fields;
using Master.Database.Management.DataLayer.Models.FixTemplates.Sections;
using Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities;
using Microsoft.EntityFrameworkCore;

namespace Master.Database.Management.DataLayer
{
  public class MdmBaseContext : DbContext
  {
    public MdmBaseContext()
    {
    }

    public MdmBaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      /* Map entity to table */
      builder.Entity<Section>().ToTable("FixTemplateSection");
      builder.Entity<Field>().ToTable("FixTemplateField");

      builder.Entity<FixTemplate>().HasQueryFilter(p => p.IsDeleted == false);

      /* Composite Keys */
      builder.Entity<FixTemplateTag>().HasKey(ftt => new { ftt.FixTemplateId, ftt.Name });
      builder.Entity<FixTemplateSectionField>().HasKey(sf => new { sf.FixTemplateId, sf.SectionId, sf.FieldId });
      builder.Entity<FixTemplateFieldValue>().HasKey(fv => new { fv.FieldId, fv.Value });

      /* Entity Mapping */
      builder.Entity<FixTemplate>()
        .HasMany(principal => principal.Tags)
        .WithOne(dependent => dependent.FixTemplate)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixType>()
        .HasMany(principle => principle.FixTemplates)
        .WithOne(dependent => dependent.Type)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<FixCategory>()
        .HasMany(principle => principle.FixTemplates)
        .WithOne(dependent => dependent.Category)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<FixTemplate>()
        .HasMany(principle => principle.SectionFields)
        .WithOne(dependent => dependent.FixTemplate)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplateSectionField>()
        .HasOne(dependent => dependent.Section)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplateSectionField>()
        .HasMany(dependent => dependent.Fields)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<Field>()
        .HasMany(principal => principal.FixTemplateFieldValues)
        .WithOne(dependent => dependent.Field)
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
  }
}

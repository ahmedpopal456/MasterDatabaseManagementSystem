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
      #region FixTemplate
      builder.Entity<FixTemplate>().HasQueryFilter(p => p.IsDeleted == false);

      builder.Entity<FixTemplate>()
        .HasMany(principal => principal.Tags)
        .WithOne(dependent => dependent.FixTemplate)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplate>()
        .HasMany(principal => principal.FixTemplateSections)
        .WithOne(dependent => dependent.FixTemplate)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplate>()
        .HasOne(principal => principal.Type)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<FixTemplate>()
        .HasOne(principal => principal.Category)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);
      #endregion

      #region FixTemplateTags
      builder.Entity<FixTemplateTag>().HasKey(ftt => new { ftt.FixTemplateId, ftt.Name });
      #endregion

      #region FixTemplateSection
      builder.Entity<FixTemplateSection>()
        .HasOne(principal => principal.Section)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<FixTemplateSection>()
        .HasOne(principal => principal.FixTemplate)
        .WithMany(dependent => dependent.FixTemplateSections)
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<FixTemplateSection>()
        .HasMany(principal => principal.FixTemplateSectionFields)
        .WithOne(dependent => dependent.FixTemplateSection)
        .OnDelete(DeleteBehavior.Cascade);
      #endregion

      #region FixTemplateSectionField
      builder.Entity<FixTemplateSectionField>().HasKey(ftsf => new { ftsf.FixTemplateSectionId, ftsf.FieldId });

      builder.Entity<FixTemplateSectionField>()
        .HasOne(principal => principal.Field)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);
      #endregion
    }

    #region Dbsets

    public DbSet<FixCategory> FixCategories { get; set; }

    public DbSet<FixType> FixTypes { get; set; }

    public DbSet<FixTemplate> FixTemplates { get; set; }

    public DbSet<FixTemplateTag> FixTemplateTags { get; set; }

    public DbSet<Section> Sections { get; set; }

    public DbSet<Field> Fields { get; set; }

    public DbSet<FixTemplateSection> FixTemplateSections { get; set; }

    public DbSet<FixTemplateSectionField> FixTemplateSectionFields { get; set; }

    #endregion

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

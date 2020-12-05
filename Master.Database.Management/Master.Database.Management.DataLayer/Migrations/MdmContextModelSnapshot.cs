﻿// <auto-generated />
using System;
using Master.Database.Management.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Master.Database.Management.DataLayer.Migrations
{
  [DbContext(typeof(MdmContext))]
  partial class MdmContextModelSnapshot : ModelSnapshot
  {
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .UseIdentityColumns()
          .HasAnnotation("Relational:MaxIdentifierLength", 128)
          .HasAnnotation("ProductVersion", "5.0.0");

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixCategory", b =>
          {
            b.Property<Guid>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("uniqueidentifier");

            b.Property<string>("Name")
                      .IsRequired()
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.HasKey("Id");

            b.ToTable("FixCategories");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.Field", b =>
          {
            b.Property<Guid>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid?>("FixTemplateSectionFieldFieldId")
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid?>("FixTemplateSectionFieldFixTemplateId")
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid?>("FixTemplateSectionFieldSectionId")
                      .HasColumnType("uniqueidentifier");

            b.Property<string>("Name")
                      .IsRequired()
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.HasKey("Id");

            b.HasIndex("FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId");

            b.ToTable("FixTemplateField");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.FixTemplateFieldValue", b =>
          {
            b.Property<Guid>("FieldId")
                      .HasColumnType("uniqueidentifier");

            b.Property<string>("Value")
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.HasKey("FieldId", "Value");

            b.ToTable("FixTemplateFieldValues");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", b =>
          {
            b.Property<Guid>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid>("CategoryId")
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid>("CreatedByUserId")
                      .HasColumnType("uniqueidentifier");

            b.Property<long>("CreatedTimestampUtc")
                      .HasColumnType("bigint");

            b.Property<long>("DeletedTimestampUtc")
                      .HasColumnType("bigint");

            b.Property<string>("Description")
                      .IsRequired()
                      .HasMaxLength(2147483647)
                      .HasColumnType("nvarchar(max)");

            b.Property<bool>("IsDeleted")
                      .HasColumnType("bit");

            b.Property<long>("LastAccessedTimestampUtc")
                      .HasColumnType("bigint");

            b.Property<string>("Name")
                      .IsRequired()
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.Property<int>("Status")
                      .HasColumnType("int");

            b.Property<double>("SystemCostEstimate")
                      .HasColumnType("float");

            b.Property<Guid>("TypeId")
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid>("UpdatedByUserId")
                      .HasColumnType("uniqueidentifier");

            b.Property<long>("UpdatedTimestampUtc")
                      .HasColumnType("bigint");

            b.HasKey("Id");

            b.HasIndex("CategoryId");

            b.HasIndex("TypeId");

            b.ToTable("FixTemplates");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplateTag", b =>
          {
            b.Property<Guid>("FixTemplateId")
                      .HasColumnType("uniqueidentifier");

            b.Property<string>("Name")
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.Property<long>("DeletedTimestampUtc")
                      .HasColumnType("bigint");

            b.Property<bool>("IsDeleted")
                      .HasColumnType("bit");

            b.HasKey("FixTemplateId", "Name");

            b.ToTable("FixTemplateTags");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.FixTemplateSectionField", b =>
          {
            b.Property<Guid>("FixTemplateId")
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid>("SectionId")
                      .HasColumnType("uniqueidentifier");

            b.Property<Guid>("FieldId")
                      .HasColumnType("uniqueidentifier");

            b.Property<long>("DeletedTimestampUtc")
                      .HasColumnType("bigint");

            b.Property<bool>("IsDeleted")
                      .HasColumnType("bit");

            b.HasKey("FixTemplateId", "SectionId", "FieldId");

            b.HasIndex("SectionId")
                      .IsUnique();

            b.ToTable("FixTemplateSectionFields");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.Section", b =>
          {
            b.Property<Guid>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("uniqueidentifier");

            b.Property<string>("Name")
                      .IsRequired()
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.HasKey("Id");

            b.ToTable("FixTemplateSection");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixType", b =>
          {
            b.Property<Guid>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("uniqueidentifier");

            b.Property<string>("Name")
                      .IsRequired()
                      .HasMaxLength(32)
                      .HasColumnType("nvarchar(32)");

            b.HasKey("Id");

            b.ToTable("FixTypes");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.Field", b =>
          {
            b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.FixTemplateSectionField", null)
                      .WithMany("Fields")
                      .HasForeignKey("FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId")
                      .OnDelete(DeleteBehavior.Cascade);
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.FixTemplateFieldValue", b =>
          {
            b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.Field", "Field")
                      .WithMany("FixTemplateFieldValues")
                      .HasForeignKey("FieldId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

            b.Navigation("Field");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", b =>
          {
            b.HasOne("Master.Database.Management.DataLayer.Models.FixCategory", "Category")
                      .WithMany("FixTemplates")
                      .HasForeignKey("CategoryId")
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();

            b.HasOne("Master.Database.Management.DataLayer.Models.FixType", "Type")
                      .WithMany("FixTemplates")
                      .HasForeignKey("TypeId")
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();

            b.Navigation("Category");

            b.Navigation("Type");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplateTag", b =>
          {
            b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", "FixTemplate")
                      .WithMany("Tags")
                      .HasForeignKey("FixTemplateId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

            b.Navigation("FixTemplate");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.FixTemplateSectionField", b =>
          {
            b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", "FixTemplate")
                      .WithMany("SectionFields")
                      .HasForeignKey("FixTemplateId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

            b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.Section", "Section")
                      .WithOne()
                      .HasForeignKey("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.FixTemplateSectionField", "SectionId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

            b.Navigation("FixTemplate");

            b.Navigation("Section");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixCategory", b =>
          {
            b.Navigation("FixTemplates");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.Field", b =>
          {
            b.Navigation("FixTemplateFieldValues");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", b =>
          {
            b.Navigation("SectionFields");

            b.Navigation("Tags");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.FixTemplateSectionField", b =>
          {
            b.Navigation("Fields");
          });

      modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixType", b =>
          {
            b.Navigation("FixTemplates");
          });
#pragma warning restore 612, 618
    }
  }
}

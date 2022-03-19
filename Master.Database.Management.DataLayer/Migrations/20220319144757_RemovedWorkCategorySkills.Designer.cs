﻿// <auto-generated />
using System;
using Master.Database.Management.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Master.Database.Management.DataLayer.Migrations
{
    [DbContext(typeof(MdmContext))]
    [Migration("20220319144757_RemovedWorkCategorySkills")]
    partial class RemovedWorkCategorySkills
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.Classifications.FixUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("FixUnits");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.Classifications.WorkCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("WorkCategories");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.Classifications.WorkType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("WorkTypes");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.Field", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(2147483647);

                    b.Property<Guid>("FixUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<double>("SystemCostEstimate")
                        .HasColumnType("float");

                    b.Property<Guid>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<Guid>("WorkCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WorkTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FixUnitId");

                    b.HasIndex("WorkCategoryId");

                    b.HasIndex("WorkTypeId");

                    b.ToTable("FixTemplates");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.License", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(2147483647);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<float>("ReferenceId")
                        .HasColumnType("real");

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("License");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CreatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<long>("DeletedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastAccessedTimestampUtc")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<long>("UpdatedTimestampUtc")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateLicense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FixTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LicenseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FixTemplateId");

                    b.HasIndex("LicenseId");

                    b.ToTable("FixTemplateLicense");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateSection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FixTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FixTemplateId");

                    b.HasIndex("SectionId");

                    b.ToTable("FixTemplateSections");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateSectionField", b =>
                {
                    b.Property<Guid>("FixTemplateSectionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FieldId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FixTemplateSectionId", "FieldId");

                    b.HasIndex("FieldId");

                    b.ToTable("FixTemplateSectionFields");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateTag", b =>
                {
                    b.Property<Guid>("FixTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("FixTemplateId", "Name");

                    b.ToTable("FixTemplateTags");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.LicenseTag", b =>
                {
                    b.Property<Guid>("LicenseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("LicenseId", "Name");

                    b.ToTable("LicenseTag");
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", b =>
                {
                    b.HasOne("Master.Database.Management.DataLayer.Models.Classifications.FixUnit", "FixUnit")
                        .WithMany()
                        .HasForeignKey("FixUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Master.Database.Management.DataLayer.Models.Classifications.WorkCategory", "WorkCategory")
                        .WithMany()
                        .HasForeignKey("WorkCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Master.Database.Management.DataLayer.Models.Classifications.WorkType", "WorkType")
                        .WithMany()
                        .HasForeignKey("WorkTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateLicense", b =>
                {
                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", "FixTemplate")
                        .WithMany("FixTemplateLicenses")
                        .HasForeignKey("FixTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.License", "License")
                        .WithMany()
                        .HasForeignKey("LicenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateSection", b =>
                {
                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", "FixTemplate")
                        .WithMany("FixTemplateSections")
                        .HasForeignKey("FixTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.Sections.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateSectionField", b =>
                {
                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.Fields.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateSection", "FixTemplateSection")
                        .WithMany("FixTemplateSectionFields")
                        .HasForeignKey("FixTemplateSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.FixTemplateTag", b =>
                {
                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.FixTemplate", "FixTemplate")
                        .WithMany("Tags")
                        .HasForeignKey("FixTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Master.Database.Management.DataLayer.Models.FixTemplates.WeakEntities.LicenseTag", b =>
                {
                    b.HasOne("Master.Database.Management.DataLayer.Models.FixTemplates.License", "License")
                        .WithMany("Tags")
                        .HasForeignKey("LicenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

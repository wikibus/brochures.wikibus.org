﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wikibus.Sources.EF;

namespace Wikibus.Sources.EF.Migrations
{
    [DbContext(typeof(SourceContext))]
    partial class SourceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:Sources.ImagesSequenceHiLo", "'ImagesSequenceHiLo', 'Sources', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:Sources.SourcesSequenceHiLo", "'SourcesSequenceHiLo', 'Sources', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Wikibus.Sources.EF.FileCabinet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("FileCabinet","Priv");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.ImageData", b =>
                {
                    b.Property<int>("Id");

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnName("Image");

                    b.HasKey("Id");

                    b.ToTable("Source","Sources");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.ImageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "ImagesSequenceHiLo")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "Sources")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("ExternalId");

                    b.Property<string>("OriginalUrl");

                    b.Property<int?>("SourceId");

                    b.Property<string>("ThumbnailUrl");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Images","Sources");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.MagazineEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("SubName");

                    b.HasKey("Id");

                    b.ToTable("Magazine","Sources");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.SourceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "SourcesSequenceHiLo")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "Sources")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<byte?>("Day");

                    b.Property<string>("Language");

                    b.Property<string>("Language2");

                    b.Property<byte?>("Month");

                    b.Property<int?>("Pages");

                    b.Property<string>("SourceType")
                        .IsRequired();

                    b.Property<short?>("Year");

                    b.HasKey("Id");

                    b.ToTable("Source","Sources");

                    b.HasDiscriminator<string>("SourceType").HasValue("SourceEntity");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.BookEntity", b =>
                {
                    b.HasBaseType("Wikibus.Sources.EF.SourceEntity");

                    b.Property<string>("BookAuthor");

                    b.Property<string>("BookISBN");

                    b.Property<string>("BookTitle");

                    b.ToTable("Source","Sources");

                    b.HasDiscriminator().HasValue("book");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.BrochureEntity", b =>
                {
                    b.HasBaseType("Wikibus.Sources.EF.SourceEntity");

                    b.Property<int?>("FileCabinet");

                    b.Property<int?>("FileOffset");

                    b.Property<string>("FolderCode");

                    b.Property<string>("FolderName");

                    b.Property<string>("Notes");

                    b.ToTable("Source","Sources");

                    b.HasDiscriminator().HasValue("folder");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.MagazineIssueEntity", b =>
                {
                    b.HasBaseType("Wikibus.Sources.EF.SourceEntity");

                    b.Property<int>("MagIssueMagazine");

                    b.Property<int?>("MagIssueNumber");

                    b.HasIndex("MagIssueMagazine");

                    b.ToTable("Source","Sources");

                    b.HasDiscriminator().HasValue("magissue");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.ImageData", b =>
                {
                    b.HasOne("Wikibus.Sources.EF.SourceEntity")
                        .WithOne("Image")
                        .HasForeignKey("Wikibus.Sources.EF.ImageData", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wikibus.Sources.EF.ImageEntity", b =>
                {
                    b.HasOne("Wikibus.Sources.EF.SourceEntity")
                        .WithMany("Images")
                        .HasForeignKey("SourceId");
                });

            modelBuilder.Entity("Wikibus.Sources.EF.MagazineIssueEntity", b =>
                {
                    b.HasOne("Wikibus.Sources.EF.MagazineEntity", "Magazine")
                        .WithMany("Issues")
                        .HasForeignKey("MagIssueMagazine")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

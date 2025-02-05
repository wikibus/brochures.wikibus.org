﻿using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Wikibus.Sources.EF
{
    public class SourceContext : DbContext, ISourceContext
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly string connectionString;

        public SourceContext(ISourcesDatabaseSettings configuration, ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.connectionString = configuration.ConnectionString;
        }

        public SourceContext([NotNull] DbContextOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            this.loggerFactory = loggerFactory;
        }

        public DbSet<SourceEntity> Sources { get; set; }

        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<BookEntity> Books { get; set; }

        public DbSet<BrochureEntity> Brochures { get; set; }

        public DbSet<MagazineEntity> Magazines { get; set; }

        public DbSet<FileCabinet> FileCabinets { get; set; }

        public DbSet<WishlistItemEntity> WishlistItems { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceEntity>()
                .HasDiscriminator<string>("SourceType")
                .HasValue<BookEntity>("book")
                .HasValue<BrochureEntity>("folder")
                .HasValue<MagazineIssueEntity>("magissue");

            modelBuilder.Entity<SourceEntity>()
                .HasMany(s => s.Images).WithOne().HasForeignKey("SourceId");

            modelBuilder.Entity<ImageEntity>()
                .ToTable("Images", "Sources");

            modelBuilder.Entity<ImageEntity>()
                .Property(p => p.Id)
                .ForSqlServerUseSequenceHiLo("ImagesSequenceHiLo", "Sources");

            modelBuilder.Entity<SourceEntity>()
                .Property(p => p.Id)
                .ForSqlServerUseSequenceHiLo("SourcesSequenceHiLo", "Sources");

            modelBuilder.Entity<MagazineEntity>()
                .HasMany(t => t.Issues).WithOne(issue => issue.Magazine).IsRequired()
                .HasForeignKey(issue => issue.MagIssueMagazine);

            modelBuilder.Entity<SourceEntity>()
                .HasOne(e => e.Image).WithOne().HasForeignKey<ImageData>(e => e.Id);

            modelBuilder.Entity<ImageData>()
                .Property(img => img.Bytes).HasColumnName("Image").IsRequired();

            modelBuilder.Entity<ImageData>()
                .ToTable("Source", "Sources");

            modelBuilder.Entity<WishlistItemEntity>()
                .ToTable("Wishlist", "Sources")
                .HasOne(e => e.Brochure).WithMany().HasForeignKey("SourceId");

            modelBuilder.Entity<FileCabinet>()
                .ToTable("FileCabinet", "Priv");

            modelBuilder.Entity<UserEntity>()
                .ToTable("Users", "Sources");

            modelBuilder.Entity<SourceEntity>()
                .Property(s => s.Updated)
                .ValueGeneratedOnAddOrUpdate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(this.loggerFactory);

            if (string.IsNullOrWhiteSpace(this.connectionString) == false)
            {
                optionsBuilder.UseSqlServer(
                this.connectionString);
            }
        }
    }
}

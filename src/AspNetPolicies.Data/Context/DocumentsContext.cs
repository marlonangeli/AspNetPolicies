using AspNetPolicies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetPolicies.Data.Context
{
    public partial class DocumentsContext : DbContext
    {
        private readonly IConfiguration _configuration;
        
        public DocumentsContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DocumentsContext(DbContextOptions<DocumentsContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<DocumentCategory> DocumentCategories { get; set; } = null!;
        public virtual DbSet<DocumentRevision> DocumentRevisions { get; set; } = null!;
        public virtual DbSet<DocumentTag> DocumentTags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("documents");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");

                entity.HasOne(d => d.OwnerUser)
                    .WithMany(p => p.DocumentsNavigation)
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("documents_owner_user_id_fkey");

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Documents)
                    .UsingEntity<Dictionary<string, object>>(
                        "AuthorizedUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("authorized_users_user_id_fkey"),
                        r => r.HasOne<Document>().WithMany().HasForeignKey("DocumentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("authorized_users_document_id_fkey"),
                        j =>
                        {
                            j.HasKey("DocumentId", "UserId").HasName("authorized_users_pkey");

                            j.ToTable("authorized_users");

                            j.IndexerProperty<int>("DocumentId").HasColumnName("document_id");

                            j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        });
            });

            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.Category })
                    .HasName("document_categories_pkey");

                entity.ToTable("document_categories");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .HasColumnName("category");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentCategories)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_categories_document_id_fkey");
            });

            modelBuilder.Entity<DocumentRevision>(entity =>
            {
                entity.ToTable("document_revisions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.RevisionDate).HasColumnName("revision_date");

                entity.Property(e => e.RevisionNumber).HasColumnName("revision_number");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentRevisions)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_revisions_document_id_fkey");
            });

            modelBuilder.Entity<DocumentTag>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.Tag })
                    .HasName("document_tags_pkey");

                entity.ToTable("document_tags");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.Tag)
                    .HasMaxLength(50)
                    .HasColumnName("tag");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentTags)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_tags_document_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Function)
                    .HasMaxLength(50)
                    .HasColumnName("function");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

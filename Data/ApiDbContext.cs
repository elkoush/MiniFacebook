using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MiniFacebook.Data
{
    public partial class ApiDbContext : DbContext
    {
        public ApiDbContext()
        {
        }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlockedIp> BlockedIps { get; set; } = null!;
        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Otp> Otps { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostText> PostTexts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ELKOUSH-PC;Database=MiniFacebook;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockedIp>(entity =>
            {
                entity.ToTable("BlockedIp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FailedAttempts).HasColumnName("failedAttempts");

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Friends_UserId");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.FriendNavigation)
                    .WithMany(p => p.FriendFriendNavigations)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friends_Users");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FriendUsers)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImageData).HasColumnType("image");
            });

            modelBuilder.Entity<Otp>(entity =>
            {
                entity.ToTable("OTP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Mobile).HasMaxLength(15);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Posts_Images");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Posts_Posts");

                entity.HasOne(d => d.PostText)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostTextId)
                    .HasConstraintName("FK_Posts_PostText");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<PostText>(entity =>
            {
                entity.ToTable("PostText");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Text).HasMaxLength(500);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ip).HasColumnName("IP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

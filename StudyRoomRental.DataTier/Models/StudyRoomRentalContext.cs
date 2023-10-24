using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StudyRoomRental.DataTier.Models
{
    public partial class StudyRoomRentalContext : DbContext
    {
        public StudyRoomRentalContext()
        {
        }

        public StudyRoomRentalContext(DbContextOptions<StudyRoomRentalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomActivity> RoomActivities { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=StudyRoomRental;Persist Security Info=True;User ID=sa;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.DateOfBirth).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.Rating)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__OrderI__59063A47");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__UserId__59FA5E80");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__UserId__5AEE82B9");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");

                entity.Property(e => e.EndedTime).HasColumnType("datetime");

                entity.Property(e => e.StartedTime).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__5BE2A6F2");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__RoomI__5CD6CB2B");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.PaymentType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__OrderId__5DCAEF64");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Room__TypeId__5EBF139D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Room__UserId__5FB337D6");
            });

            modelBuilder.Entity<RoomActivity>(entity =>
            {
                entity.ToTable("RoomActivity");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.EndedTime).HasColumnType("datetime");

                entity.Property(e => e.StartedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomActivities)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoomActiv__RoomI__60A75C0F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoomActivities)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoomActiv__UserI__619B8048");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacti__Payme__628FA481");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

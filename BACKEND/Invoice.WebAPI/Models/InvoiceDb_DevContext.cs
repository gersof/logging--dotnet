using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Invoice.WebAPI.Models
{
    public partial class InvoiceDb_DevContext : DbContext
    {
        public InvoiceDb_DevContext()
        {
        }

        public InvoiceDb_DevContext(DbContextOptions<InvoiceDb_DevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }

    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.FirstLastName)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.SecondLastName).HasMaxLength(80);

                entity.Property(e => e.SecondName).HasMaxLength(80);

                entity.Property(e => e.Telephone).HasMaxLength(50);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Invoice)
                    .HasForeignKey<Invoice>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Employee");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetail)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetail_Invoice");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InvoiceDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetail_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

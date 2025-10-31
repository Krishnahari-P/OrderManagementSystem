using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> CustomerSet { get; set; }
        public DbSet<Order> OrderSet { get; set; }
        public DbSet<Product> ProductSet { get; set; }
        public DbSet<Category> CategorySet { get; set; }
        public DbSet<OrderItem> OrderItemSet { get; set; }
        public DbSet<Payment> PaymentSet { get; set; }
        public DbSet<Supplier> SupplierSet { get; set; }
        public DbSet<Purchase> PurchaseSet { get; set; }
        public DbSet<PurchaseItem> PurchaseItemSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                
            modelBuilder.Entity<Customer>(entity=>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne<Category>(c => c.CategorySet)
                .WithMany(p => p.Products)
                .HasForeignKey(k => k.CategoryId);

                entity.Property(e => e.StockQuantity).HasDefaultValue(0);
                entity.Property(e => e.UnitPrice).HasColumnType("DECIMAL(10,2)");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne<Customer>(c => c.CustomerSet)
                .WithMany(o=>o.Orders)
                .HasForeignKey(k => k.CustomerId);
                entity.Property(e => e.OrderDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Status).HasDefaultValue("Pending");
                entity.Property(e => e.TotalAmount).HasColumnType("DECIMAL(10,2)");
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne<Product>(c => c.ProductSet)
                .WithMany(o=>o.OrderItems)
                .HasForeignKey(k => k.ProductId);
                entity.HasOne<Order>(c => c.OrderSet)
                .WithMany(o=>o.OrderItems)
                .HasForeignKey(k => k.OrderId);
                entity.Property(e => e.UnitPrice).HasColumnType("DECIMAL(10,2)");
            });
            modelBuilder.Entity<Supplier>(entity =>
            {               
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasOne<Supplier>(c => c.SupplierSet)
                .WithMany(o => o.Purchases)
                .HasForeignKey(k => k.SupplierId);
                entity.Property(e => e.PurchaseDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.TotalAmount).HasColumnType("DECIMAL(10,2)");
                entity.Property(e => e.Status).HasDefaultValue("Pending");

            });
            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasOne<Purchase>(c => c.PurchaseSet)
                .WithMany(o => o.PurchaseItems)
                .HasForeignKey(k => k.PurchaseId);

                entity.HasOne<Product>(c => c.ProductSet)
                .WithMany(o => o.PurchaseItems)
                .HasForeignKey(k => k.ProductId);

                entity.Property(e => e.UnitCost).HasColumnType("DECIMAL(10,2)");

            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne<Order>(p => p.OrderSet)
               .WithOne(o => o.PaymentSet)
               .HasForeignKey<Payment>(p => p.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Purchase>(p => p.PurchaseSet)
               .WithOne(o => o.PaymentSet)
               .HasForeignKey<Payment>(p => p.PurchaseId)
               .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.PaymentDate)
               .HasDefaultValueSql("GETDATE()");

                entity.Property(p => p.AmountPaid)
               .HasColumnType("DECIMAL(10,2)");

            });
            base.OnModelCreating(modelBuilder); 
        }
    }
}

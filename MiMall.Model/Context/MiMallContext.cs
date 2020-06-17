using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MiMall.Model.Models;

namespace MiMall.Model.Context
{
    public partial class MiMallContext : DbContext
    {
        public MiMallContext()
        {
        }

        public MiMallContext(DbContextOptions<MiMallContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Carousel> Carousel { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Collect> Collect { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPicture> ProductPicture { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=111.230.171.47;Initial Catalog=MiMall;User ID=lcm;Password=ruanjian3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carousel>(entity =>
            {
                entity.ToTable("carousel");

                entity.Property(e => e.CarouselId).HasColumnName("carousel_id");

                entity.Property(e => e.Describes)
                    .IsRequired()
                    .HasColumnName("describes")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ImgPath)
                    .IsRequired()
                    .HasColumnName("imgPath")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carousel)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__carousel__userId__2B3F6F97");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Collect>(entity =>
            {
                entity.ToTable("collect");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CollectTime).HasColumnName("collect_time");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Collect)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__collect__product__3E52440B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Collect)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__collect__userId__3D5E1FD2");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.OrderTime).HasColumnName("order_time");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProductNum).HasColumnName("product_num");

                entity.Property(e => e.ProductPrice)
                    .HasColumnName("product_price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__product___3A81B327");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__userId__398D8EEE");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.ProductIntro)
                    .IsRequired()
                    .HasColumnName("product_intro")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ProductNum).HasColumnName("product_num");

                entity.Property(e => e.ProductPrice)
                    .HasColumnName("product_price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductSales).HasColumnName("product_sales");

                entity.Property(e => e.ProductSellingPrice)
                    .HasColumnName("product_selling_price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductTitle)
                    .IsRequired()
                    .HasColumnName("product_title")
                    .HasMaxLength(30);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__product__categor__300424B4");
            });

            modelBuilder.Entity<ProductPicture>(entity =>
            {
                entity.ToTable("product_picture");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Intro)
                    .HasColumnName("intro")
                    .HasMaxLength(1000);

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProductPicture1)
                    .HasColumnName("product_picture")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPicture)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__product_p__produ__32E0915F");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("shoppingCart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Num).HasColumnName("num");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCart)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__shoppingC__produ__36B12243");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ShoppingCart)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__shoppingC__userI__35BCFE0A");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__userRole__CD98462AF3198A55");

                entity.ToTable("userRole");

                entity.HasIndex(e => e.RoleName)
                    .HasName("UQ__userRole__B1947861D48E19C3")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("roleName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__users__CB9A1CFFCB41D8CD");

                entity.ToTable("users");

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__users__66DCF95C44B17256")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.NickName)
                    .HasColumnName("nickName")
                    .HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("userEmail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(40);

                entity.Property(e => e.UserPhoneNumber)
                    .HasColumnName("userPhoneNumber")
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Userpwd)
                    .IsRequired()
                    .HasColumnName("userpwd")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__users__roleId__286302EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

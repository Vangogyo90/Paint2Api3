using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Paint2API.Models
{
    public partial class PaintContext : DbContext
    {
        public PaintContext()
        {
        }

        public PaintContext(DbContextOptions<PaintContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<ColorDelivery> ColorDeliveries { get; set; } = null!;
        public virtual DbSet<Delivery> Deliveries { get; set; } = null!;
        public virtual DbSet<Discount> Discounts { get; set; } = null!;
        public virtual DbSet<FeedBack> FeedBacks { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<PhotoNews> PhotoNews { get; set; } = null!;
        public virtual DbSet<QuantityColor> QuantityColors { get; set; } = null!;
        public virtual DbSet<RalCatalog> RalCatalogs { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Shine> Shines { get; set; } = null!;
        public virtual DbSet<StatusDelivery> StatusDeliveries { get; set; } = null!;
        public virtual DbSet<TempPulverization> TempPulverizations { get; set; } = null!;
        public virtual DbSet<TypeApplication> TypeApplications { get; set; } = null!;
        public virtual DbSet<TypeSurface> TypeSurfaces { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WareHouse> WareHouses { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<SoldColor> SoldColors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-H63RQBM\\MYSERVERBD;Initial Catalog=Paint;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.IdCity);

                entity.ToTable("City");

                entity.HasIndex(e => e.NameCity, "UQ__City__431EEB911FC8FE71")
                    .IsUnique();

                entity.Property(e => e.IdCity).HasColumnName("ID_City");

                entity.Property(e => e.NameCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_City");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.IdColor);

                entity.ToTable("Color");

                entity.Property(e => e.IdColor).HasColumnName("ID_Color");

                entity.Property(e => e.RalCatalogId).HasColumnName("Ral_catalog_ID");

                entity.Property(e => e.ShineId).HasColumnName("Shine_ID");

                entity.Property(e => e.TempPulverizationId).HasColumnName("Temp_Pulverization_ID");

                entity.Property(e => e.TypeApplicationId).HasColumnName("Type_Application_ID");

                entity.Property(e => e.TypeSurfaceId).HasColumnName("Type_Surface_ID");
            });

            modelBuilder.Entity<ColorDelivery>(entity =>
            {
                entity.HasKey(e => e.IdColorDelivery);

                entity.ToTable("Color_Delivery");

                entity.Property(e => e.IdColorDelivery).HasColumnName("ID_Color_Delivery");

                entity.Property(e => e.ColorId).HasColumnName("Color_ID");

                entity.Property(e => e.DeliveryId).HasColumnName("Delivery_ID");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(e => e.IdDelivery);

                entity.ToTable("Delivery");

                entity.Property(e => e.IdDelivery).HasColumnName("ID_Delivery");

                entity.Property(e => e.Adress).IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("City_ID");

                entity.Property(e => e.Salt).IsUnicode(false);

                entity.Property(e => e.StatusOrderId).HasColumnName("Status_Order_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(e => e.IdDiscount);

                entity.ToTable("Discount");

                entity.HasIndex(e => e.ColorId, "UQ__Discount__795F1D75D99EA8CA")
                    .IsUnique();

                entity.Property(e => e.IdDiscount).HasColumnName("ID_Discount");

                entity.Property(e => e.ColorId).HasColumnName("Color_ID");

                entity.Property(e => e.SizeDicsount).HasColumnName("Size_Dicsount");
            });

            modelBuilder.Entity<FeedBack>(entity =>
            {
                entity.HasKey(e => e.IdFeedBack);

                entity.ToTable("FeedBack");

                entity.Property(e => e.IdFeedBack)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_FeedBack");

                entity.Property(e => e.NameUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_User");

                entity.Property(e => e.Number_Or_E_mail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Number_Or_E_mail");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.IdNews);

                entity.Property(e => e.IdNews).HasColumnName("ID_News");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HeadingNews)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Heading_News");

                entity.Property(e => e.TextNews).HasColumnName("Text_News");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<PhotoNews>(entity =>
            {
                entity.HasKey(e => e.IdPhotoNews);

                entity.ToTable("Photo_News");

                entity.Property(e => e.IdPhotoNews)
                    .HasColumnName("ID_Photo_News");

                entity.Property(e => e.NewsId).HasColumnName("News_ID");
            });

            modelBuilder.Entity<QuantityColor>(entity =>
            {
                entity.HasKey(e => e.IdQuantityColors);

                entity.ToTable("Quantity_Colors");

                entity.Property(e => e.IdQuantityColors).HasColumnName("ID_Quantity_Colors");

                entity.Property(e => e.ColorId).HasColumnName("Color_ID");

                entity.Property(e => e.WareHouseId).HasColumnName("WareHouse_ID");

                entity.Property(e => e.Price_For_KG).HasColumnName("Price_For_KG");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Shelf_Life)
                    .HasColumnType("int")
                    .HasDefaultValueSql("(1000)");
            });

            modelBuilder.Entity<RalCatalog>(entity =>
            {
                entity.HasKey(e => e.IdRalCatalog)
                    .HasName("PK_Ral_Catalog");

                entity.ToTable("Ral_catalog");

                entity.HasIndex(e => e.ColorRal, "UQ__Ral_cata__62BA0546EA2AF6BD")
                    .IsUnique();

                entity.HasIndex(e => e.ColorHtml, "UQ__Ral_cata__707B009012679A6E")
                    .IsUnique();

                entity.HasIndex(e => e.NameRal, "UQ__Ral_cata__FC7F1DA82D7908A7")
                    .IsUnique();

                entity.Property(e => e.IdRalCatalog).HasColumnName("ID_Ral_Catalog");

                entity.Property(e => e.ColorHtml)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("Color_HTML");

                entity.Property(e => e.ColorRal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Color_Ral");

                entity.Property(e => e.NameRal)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Name_Ral");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.ToTable("Role");

                entity.HasIndex(e => e.NameRole, "UQ__Role__32E244D41F254F5F")
                    .IsUnique();

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Role");
            });

            modelBuilder.Entity<Shine>(entity =>
            {
                entity.HasKey(e => e.IdShine);

                entity.ToTable("Shine");

                entity.HasIndex(e => e.NameShine, "UQ__Shine__85632E656D0F014D")
                    .IsUnique();

                entity.Property(e => e.IdShine).HasColumnName("ID_Shine");

                entity.Property(e => e.EndProcent).HasColumnName("End_Procent");

                entity.Property(e => e.FirstProcent).HasColumnName("First_Procent");

                entity.Property(e => e.NameShine)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Shine");
            });

            modelBuilder.Entity<StatusDelivery>(entity =>
            {
                entity.HasKey(e => e.IdStatusOrder)
                    .HasName("PK_Status_Order");

                entity.ToTable("Status_Delivery");

                entity.HasIndex(e => e.NameStatusOrder, "UQ__Status_D__5E55D10E3F7D9484")
                    .IsUnique();

                entity.Property(e => e.IdStatusOrder).HasColumnName("ID_Status_Order");

                entity.Property(e => e.NameStatusOrder)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Status_Order");
            });

            modelBuilder.Entity<TempPulverization>(entity =>
            {
                entity.HasKey(e => e.IdTempPulverization);

                entity.ToTable("Temp_Pulverization");

                entity.Property(e => e.IdTempPulverization).HasColumnName("ID_Temp_Pulverization");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.IdToken);

                entity.ToTable("Token");

                entity.Property(e => e.IdToken).HasColumnName("ID_Token");

                entity.Property(e => e.Token1)
                    .IsUnicode(false)
                    .HasColumnName("Token");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<TypeApplication>(entity =>
            {
                entity.HasKey(e => e.IdTypeApplication);

                entity.ToTable("Type_Application");

                entity.HasIndex(e => e.NameTypeApplication, "UQ__Type_App__812EFC4BFA43FB25")
                    .IsUnique();

                entity.Property(e => e.IdTypeApplication).HasColumnName("ID_Type_Application");

                entity.Property(e => e.NameTypeApplication)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Name_Type_Application");
            });

            modelBuilder.Entity<TypeSurface>(entity =>
            {
                entity.HasKey(e => e.IdTypeSurface);

                entity.ToTable("Type_Surface");

                entity.HasIndex(e => e.NameTypeSurface, "UQ__Type_Sur__5051066D0442CC47")
                    .IsUnique();

                entity.Property(e => e.IdTypeSurface).HasColumnName("ID_Type_Surface");

                entity.Property(e => e.NameTypeSurface)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Type_Surface");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.HasIndex(e => e.EMail, "UQ__User__31660442310EBE85")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "UQ__User__5E55825B9E37C23E")
                    .IsUnique();

                entity.HasIndex(e => e.NumberTelephone, "UQ__User__B94E574760CA49F9")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.EMail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("E_mail");

                entity.Property(e => e.Login)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberTelephone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Number_Telephone");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Patromymic)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Salt).IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WareHouse>(entity =>
            {
                entity.HasKey(e => e.IdWareHouse);

                entity.ToTable("WareHouse");

                entity.Property(e => e.IdWareHouse).HasColumnName("ID_WareHouse");

                entity.Property(e => e.Adress).IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("City_ID");
            });

            modelBuilder.Entity<SoldColor>(entity =>
            {
                entity.HasKey(e => e.ID_Sold_Color);

                entity.ToTable("Sold_Color");

                entity.Property(e => e.Delivery_ID).HasColumnName("Delivery_ID");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

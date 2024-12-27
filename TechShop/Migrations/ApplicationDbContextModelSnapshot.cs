﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechShop.Data;

#nullable disable

namespace TechShop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechShop.Models.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandId"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BrandId");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            BrandId = 1,
                            BrandName = "Lenovo"
                        },
                        new
                        {
                            BrandId = 2,
                            BrandName = "Samsung"
                        },
                        new
                        {
                            BrandId = 3,
                            BrandName = "MSI"
                        },
                        new
                        {
                            BrandId = 4,
                            BrandName = "Apple"
                        },
                        new
                        {
                            BrandId = 5,
                            BrandName = "Intel"
                        },
                        new
                        {
                            BrandId = 6,
                            BrandName = "Dell"
                        });
                });

            modelBuilder.Entity("TechShop.Models.CPU", b =>
                {
                    b.Property<int>("CPUId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CPUId"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TenCPU")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CPUId");

                    b.ToTable("CPUs");
                });

            modelBuilder.Entity("TechShop.Models.CardDoHoa", b =>
                {
                    b.Property<int>("CardDoHoaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardDoHoaId"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TenCardDoHoa")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardDoHoaId");

                    b.ToTable("CardDoHoa");
                });

            modelBuilder.Entity("TechShop.Models.CartDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<int>("specId")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("specId");

                    b.ToTable("CartDetails");
                });

            modelBuilder.Entity("TechShop.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Laptop văn phòng",
                            Description = "Là sản phẩm bán chạy nhất của chúng tôi"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Laptop Gaming",
                            Description = "Máy có cấu hình mạnh đáp ứng nhu cầu chơi game"
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "Laptop đồ họa",
                            Description = "Máy có cấu hình mạnh đáp ứng nhu cầu thiết kế đồ họa"
                        },
                        new
                        {
                            CategoryId = 4,
                            CategoryName = "Laptop cao cấp",
                            Description = "Máy có cấu thiết kế mỏng nhẹ"
                        },
                        new
                        {
                            CategoryId = 5,
                            CategoryName = "Surface",
                            Description = "Surface phụ vụ cho công việc, học tập"
                        });
                });

            modelBuilder.Entity("TechShop.Models.CauHinh", b =>
                {
                    b.Property<int>("MaCauHinh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaCauHinh"));

                    b.Property<int?>("CPUId")
                        .HasColumnType("int");

                    b.Property<int?>("CardDoHoaId")
                        .HasColumnType("int");

                    b.Property<int?>("ManHinhId")
                        .HasColumnType("int");

                    b.Property<int?>("ODiaId")
                        .HasColumnType("int");

                    b.Property<int?>("PinId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("RAMId")
                        .HasColumnType("int");

                    b.Property<int?>("TrongLuongId")
                        .HasColumnType("int");

                    b.HasKey("MaCauHinh");

                    b.HasIndex("CPUId");

                    b.HasIndex("CardDoHoaId");

                    b.HasIndex("ManHinhId");

                    b.HasIndex("ODiaId");

                    b.HasIndex("PinId");

                    b.HasIndex("ProductId");

                    b.HasIndex("RAMId");

                    b.HasIndex("TrongLuongId");

                    b.ToTable("cauHinhs");
                });

            modelBuilder.Entity("TechShop.Models.ManHinh", b =>
                {
                    b.Property<int>("ManHinhId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ManHinhId"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("KichThuocManHinh")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ManHinhId");

                    b.ToTable("ManHinhs");
                });

            modelBuilder.Entity("TechShop.Models.ODia", b =>
                {
                    b.Property<int>("ODiaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ODiaId"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TenODia")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ODiaId");

                    b.ToTable("ODias");
                });

            modelBuilder.Entity("TechShop.Models.OTP", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OtpCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OTPs");
                });

            modelBuilder.Entity("TechShop.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<string>("StatusPayment")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StatusShipping")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("district")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAccept")
                        .HasMaxLength(100)
                        .HasColumnType("bit");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("provice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ward")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TechShop.Models.OrderDetail", b =>
                {
                    b.Property<int>("specId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("specId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("DetailsOrders");
                });

            modelBuilder.Entity("TechShop.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"));

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");

                    b.HasData(
                        new
                        {
                            PaymentMethodId = 1,
                            MethodName = "Thanh toán tại cửa hàng"
                        },
                        new
                        {
                            PaymentMethodId = 2,
                            MethodName = "Thanh toán khi nhận hàng"
                        },
                        new
                        {
                            PaymentMethodId = 3,
                            MethodName = "Thanh toán bằng chuyển khoản ngân hàng"
                        },
                        new
                        {
                            PaymentMethodId = 4,
                            MethodName = "Thanh toán trả góp"
                        });
                });

            modelBuilder.Entity("TechShop.Models.Pin", b =>
                {
                    b.Property<int>("PinId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PinId"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DungLuongPin")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PinId");

                    b.ToTable("Pins");
                });

            modelBuilder.Entity("TechShop.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PriceBase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<int>("warranty")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            BrandId = 1,
                            CategoryId = 1,
                            Description = "Legion Y7000P 2024 là một chiếc Laptop hứa hẹn là một trong những sự lựa chọn tuyệt vời bởi thiết kế độc đáo, cá tính cùng với hiệu năng và những thông số kĩ thuật ấn tượng. Vậy nên đừng ngần ngại lựa chọn mua Legion Y7000P 2024 tại hệ thống của hàng của LaptopAZ.vn để được trải nghiệm sự tuyệt vời của chiếc Laptop này đem lại.",
                            Img = "https://laptopaz.vn/media/product/3174_",
                            PriceBase = 21490000m,
                            ProductName = "Lenovo Thinkbook 16 G6+ 2024",
                            StockQuantity = 100,
                            warranty = 0
                        });
                });

            modelBuilder.Entity("TechShop.Models.RAM", b =>
                {
                    b.Property<int>("RAMId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RAMId"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DungLuong")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RAMId");

                    b.ToTable("RAMs");
                });

            modelBuilder.Entity("TechShop.Models.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<byte?>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("TechShop.Models.Role", b =>
                {
                    b.Property<int>("roleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("roleId"));

                    b.Property<string>("roleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("roleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TechShop.Models.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("TechShop.Models.Specs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfigId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.HasIndex("ProductId");

                    b.ToTable("specs");
                });

            modelBuilder.Entity("TechShop.Models.TrongLuong", b =>
                {
                    b.Property<int>("TrongLuongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrongLuongId"));

                    b.Property<string>("SoKg")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrongLuongId");

                    b.ToTable("TrongLuongs");
                });

            modelBuilder.Entity("TechShop.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("RandomKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TechShop.Models.CartDetail", b =>
                {
                    b.HasOne("TechShop.Models.ShoppingCart", "cart")
                        .WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechShop.Models.Specs", "Specs")
                        .WithMany()
                        .HasForeignKey("specId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specs");

                    b.Navigation("cart");
                });

            modelBuilder.Entity("TechShop.Models.CauHinh", b =>
                {
                    b.HasOne("TechShop.Models.CPU", "CPU")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("CPUId");

                    b.HasOne("TechShop.Models.CardDoHoa", "CardDoHoa")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("CardDoHoaId");

                    b.HasOne("TechShop.Models.ManHinh", "ManHinh")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("ManHinhId");

                    b.HasOne("TechShop.Models.ODia", "ODia")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("ODiaId");

                    b.HasOne("TechShop.Models.Pin", "Pin")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("PinId");

                    b.HasOne("TechShop.Models.Product", null)
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("ProductId");

                    b.HasOne("TechShop.Models.RAM", "RAM")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("RAMId");

                    b.HasOne("TechShop.Models.TrongLuong", "TrongLuong")
                        .WithMany("CauHinhProducts")
                        .HasForeignKey("TrongLuongId");

                    b.Navigation("CPU");

                    b.Navigation("CardDoHoa");

                    b.Navigation("ManHinh");

                    b.Navigation("ODia");

                    b.Navigation("Pin");

                    b.Navigation("RAM");

                    b.Navigation("TrongLuong");
                });

            modelBuilder.Entity("TechShop.Models.OTP", b =>
                {
                    b.HasOne("TechShop.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TechShop.Models.Order", b =>
                {
                    b.HasOne("TechShop.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechShop.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentMethod");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TechShop.Models.OrderDetail", b =>
                {
                    b.HasOne("TechShop.Models.Order", "order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechShop.Models.Specs", "Specs")
                        .WithMany()
                        .HasForeignKey("specId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specs");

                    b.Navigation("order");
                });

            modelBuilder.Entity("TechShop.Models.Product", b =>
                {
                    b.HasOne("TechShop.Models.Brand", "BrandOfProducts")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechShop.Models.Category", "CategoryOfProducts")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrandOfProducts");

                    b.Navigation("CategoryOfProducts");
                });

            modelBuilder.Entity("TechShop.Models.Review", b =>
                {
                    b.HasOne("TechShop.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechShop.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TechShop.Models.ShoppingCart", b =>
                {
                    b.HasOne("TechShop.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TechShop.Models.Specs", b =>
                {
                    b.HasOne("TechShop.Models.CauHinh", "Config")
                        .WithMany()
                        .HasForeignKey("ConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechShop.Models.Product", "Product")
                        .WithMany("Specifications")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Config");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("TechShop.Models.User", b =>
                {
                    b.HasOne("TechShop.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TechShop.Models.CPU", b =>
                {
                    b.Navigation("CauHinhProducts");
                });

            modelBuilder.Entity("TechShop.Models.CardDoHoa", b =>
                {
                    b.Navigation("CauHinhProducts");
                });

            modelBuilder.Entity("TechShop.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("TechShop.Models.ManHinh", b =>
                {
                    b.Navigation("CauHinhProducts");
                });

            modelBuilder.Entity("TechShop.Models.ODia", b =>
                {
                    b.Navigation("CauHinhProducts");
                });

            modelBuilder.Entity("TechShop.Models.Pin", b =>
                {
                    b.Navigation("CauHinhProducts");
                });

            modelBuilder.Entity("TechShop.Models.Product", b =>
                {
                    b.Navigation("CauHinhProducts");

                    b.Navigation("Specifications");
                });

            modelBuilder.Entity("TechShop.Models.RAM", b =>
                {
                    b.Navigation("CauHinhProducts");
                });

            modelBuilder.Entity("TechShop.Models.TrongLuong", b =>
                {
                    b.Navigation("CauHinhProducts");
                });
#pragma warning restore 612, 618
        }
    }
}

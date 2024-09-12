﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using template_csharp_dotnet.Data;

#nullable disable

namespace template_csharp_dotnet.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240912185438_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("template_csharp_dotnet.Models.Entrada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodigoQR")
                        .IsRequired()
                        .HasMaxLength(7089)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<bool>("Verificada")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("Entradas", (string)null);
                });

            modelBuilder.Entity("template_csharp_dotnet.Models.Reventa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompradorId")
                        .HasColumnType("int");

                    b.Property<int>("EntradaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaReventa")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Precio")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VendedorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EntradaId")
                        .IsUnique();

                    b.HasIndex("VendedorId")
                        .IsUnique();

                    b.ToTable("Reventas", (string)null);
                });

            modelBuilder.Entity("template_csharp_dotnet.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("varchar");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar");

                    b.Property<string>("NumeroTelefono")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar");

                    b.Property<bool>("Verificado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("template_csharp_dotnet.Models.Entrada", b =>
                {
                    b.HasOne("template_csharp_dotnet.Models.Usuario", "Usuario")
                        .WithOne("Entrada")
                        .HasForeignKey("template_csharp_dotnet.Models.Entrada", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("template_csharp_dotnet.Models.Reventa", b =>
                {
                    b.HasOne("template_csharp_dotnet.Models.Entrada", "Entrada")
                        .WithOne("Reventa")
                        .HasForeignKey("template_csharp_dotnet.Models.Reventa", "EntradaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("template_csharp_dotnet.Models.Usuario", "Usuario")
                        .WithOne("Reventa")
                        .HasForeignKey("template_csharp_dotnet.Models.Reventa", "VendedorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Entrada");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("template_csharp_dotnet.Models.Entrada", b =>
                {
                    b.Navigation("Reventa")
                        .IsRequired();
                });

            modelBuilder.Entity("template_csharp_dotnet.Models.Usuario", b =>
                {
                    b.Navigation("Entrada")
                        .IsRequired();

                    b.Navigation("Reventa")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

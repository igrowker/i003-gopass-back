﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace template_csharp_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {

                    // Puntos a tener en cuenta al momento de crear un usuario los valores son reducidos 
                    // De tal manera que una persona de X pais debe cumplir con estos criterios sin embargo
                    // Donde un DNI exceda los 8 caracteres, o el Telf sea mayor a 10 dara un error 404 en DB.
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    DNI = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    NumeroTelefono = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Verificado = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoQR = table.Column<string>(type: "nvarchar(max)", maxLength: 7089, nullable: false),
                    Verificada = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entradas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntradaId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    CompradorId = table.Column<int>(type: "int", nullable: false),
                    FechaReventa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reventas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reventas_Entradas_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entradas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reventas_Usuarios_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_UsuarioId",
                table: "Entradas",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reventas_EntradaId",
                table: "Reventas",
                column: "EntradaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reventas_VendedorId",
                table: "Reventas",
                column: "VendedorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reventas");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

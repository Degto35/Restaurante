using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCategoriasYPlatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id_Pedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id_Pedido);
                });

            migrationBuilder.CreateTable(
                name: "Platos",
                columns: table => new
                {
                    Id_Plato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platos", x => x.Id_Plato);
                    table.ForeignKey(
                        name: "FK_Platos_Categorias_Id_Categoria",
                        column: x => x.Id_Categoria,
                        principalTable: "Categorias",
                        principalColumn: "Id_Categoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedido",
                columns: table => new
                {
                    Id_DetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Pedido = table.Column<int>(type: "int", nullable: false),
                    Id_Plato = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedido", x => x.Id_DetallePedido);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Pedidos_Id_Pedido",
                        column: x => x.Id_Pedido,
                        principalTable: "Pedidos",
                        principalColumn: "Id_Pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Platos_Id_Plato",
                        column: x => x.Id_Plato,
                        principalTable: "Platos",
                        principalColumn: "Id_Plato",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_Id_Pedido",
                table: "DetallesPedido",
                column: "Id_Pedido");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_Id_Plato",
                table: "DetallesPedido",
                column: "Id_Plato");

            migrationBuilder.CreateIndex(
                name: "IX_Platos_Id_Categoria",
                table: "Platos",
                column: "Id_Categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesPedido");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Platos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}

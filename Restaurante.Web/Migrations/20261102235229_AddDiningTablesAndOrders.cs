using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDiningTablesAndOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    IdTable = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.IdTable);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IdOrder = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdTable = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableIdTable = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdEmployee = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeIdEmployee = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeIdEmployee",
                        column: x => x.EmployeeIdEmployee,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Tables_TableIdTable",
                        column: x => x.TableIdTable,
                        principalTable: "Tables",
                        principalColumn: "IdTable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    IdOrderDetail = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdOrder = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderIdOrder = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDish = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishIdDish = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.IdOrderDetail);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Dishes_DishIdDish",
                        column: x => x.DishIdDish,
                        principalTable: "Dishes",
                        principalColumn: "IdDish",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderIdOrder",
                        column: x => x.OrderIdOrder,
                        principalTable: "Orders",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_DishIdDish",
                table: "OrderDetails",
                column: "DishIdDish");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderIdOrder",
                table: "OrderDetails",
                column: "OrderIdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeIdEmployee",
                table: "Orders",
                column: "EmployeeIdEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TableIdTable",
                table: "Orders",
                column: "TableIdTable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}

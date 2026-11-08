using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Categories_IdCategories",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_IdRole",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "NameRole",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "IdRole",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdRole",
                table: "Employees",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "IdEmployee",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_IdRole",
                table: "Employees",
                newName: "IX_Employees_RoleId");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Dishes",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IdCategories",
                table: "Dishes",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "IdDish",
                table: "Dishes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Dishes_IdCategories",
                table: "Dishes",
                newName: "IX_Dishes_CategoryId");

            migrationBuilder.RenameColumn(
                name: "IdTable",
                table: "DiningTables",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Categories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IdCategories",
                table: "Categories",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "NameRole");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "IdRole");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Employees",
                newName: "IdRole");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "IdEmployee");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                newName: "IX_Employees_IdRole");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Dishes",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Dishes",
                newName: "IdCategories");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Dishes",
                newName: "IdDish");

            migrationBuilder.RenameIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishes",
                newName: "IX_Dishes_IdCategories");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DiningTables",
                newName: "IdTable");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Categories",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "IdCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Categories_IdCategories",
                table: "Dishes",
                column: "IdCategories",
                principalTable: "Categories",
                principalColumn: "IdCategories",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_IdRole",
                table: "Employees",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

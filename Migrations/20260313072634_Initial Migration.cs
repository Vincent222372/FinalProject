using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Carts_tb_Customers_CustomerId",
                table: "tb_Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Order_tb_Customers_CustomerID",
                table: "tb_Order");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_ProductCategoryCateID",
                table: "tb_Product");

            migrationBuilder.DropTable(
                name: "tb_Customers");

            migrationBuilder.DropIndex(
                name: "IX_tb_Product_ProductCategoryCateID",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "ProductCategoryCateID",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "tb_Product");

            migrationBuilder.RenameColumn(
                name: "Statuss",
                table: "tb_Product",
                newName: "Status");

            migrationBuilder.AlterColumn<decimal>(
                name: "PromotionPrice",
                table: "tb_Product",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tb_Product",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<string>(
                name: "MetaKeywords",
                table: "tb_Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "tb_Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "tb_Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ShopID",
                table: "tb_Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tb_Brand",
                columns: table => new
                {
                    BrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Brand", x => x.BrandID);
                });

            migrationBuilder.CreateTable(
                name: "tb_User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    PhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_tb_User_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_BrandID",
                table: "tb_Product",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_CateID",
                table: "tb_Product",
                column: "CateID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_ShopID",
                table: "tb_Product",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_User_RoleId",
                table: "tb_User",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Carts_tb_User_CustomerId",
                table: "tb_Carts",
                column: "CustomerId",
                principalTable: "tb_User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Order_tb_User_CustomerID",
                table: "tb_Order",
                column: "CustomerID",
                principalTable: "tb_User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Product_tb_Brand_BrandID",
                table: "tb_Product",
                column: "BrandID",
                principalTable: "tb_Brand",
                principalColumn: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_CateID",
                table: "tb_Product",
                column: "CateID",
                principalTable: "tb_ProductCategory",
                principalColumn: "CateID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Product_tb_Shop_ShopID",
                table: "tb_Product",
                column: "ShopID",
                principalTable: "tb_Shop",
                principalColumn: "ShopID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Carts_tb_User_CustomerId",
                table: "tb_Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Order_tb_User_CustomerID",
                table: "tb_Order");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Product_tb_Brand_BrandID",
                table: "tb_Product");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_CateID",
                table: "tb_Product");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Product_tb_Shop_ShopID",
                table: "tb_Product");

            migrationBuilder.DropTable(
                name: "tb_Brand");

            migrationBuilder.DropTable(
                name: "tb_User");

            migrationBuilder.DropIndex(
                name: "IX_tb_Product_BrandID",
                table: "tb_Product");

            migrationBuilder.DropIndex(
                name: "IX_tb_Product_CateID",
                table: "tb_Product");

            migrationBuilder.DropIndex(
                name: "IX_tb_Product_ShopID",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "ShopID",
                table: "tb_Product");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tb_Product",
                newName: "Statuss");

            migrationBuilder.AlterColumn<decimal>(
                name: "PromotionPrice",
                table: "tb_Product",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tb_Product",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "MetaKeywords",
                table: "tb_Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "tb_Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "tb_Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryCateID",
                table: "tb_Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "tb_Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_tb_Customers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_ProductCategoryCateID",
                table: "tb_Product",
                column: "ProductCategoryCateID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Customers_RoleId",
                table: "tb_Customers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Carts_tb_Customers_CustomerId",
                table: "tb_Carts",
                column: "CustomerId",
                principalTable: "tb_Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Order_tb_Customers_CustomerID",
                table: "tb_Order",
                column: "CustomerID",
                principalTable: "tb_Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Product_tb_ProductCategory_ProductCategoryCateID",
                table: "tb_Product",
                column: "ProductCategoryCateID",
                principalTable: "tb_ProductCategory",
                principalColumn: "CateID");
        }
    }
}

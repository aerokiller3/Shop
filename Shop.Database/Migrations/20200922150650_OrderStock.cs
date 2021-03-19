﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Database.Migrations
{
    public partial class OrderStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderProducts");

            migrationBuilder.RenameTable(
                name: "OrderProducts",
                newName: "OrderStocks");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderStocks",
                newName: "IX_OrderStocks_OrderId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeReference",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "OrderStocks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "OrderStocks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStocks",
                table: "OrderStocks",
                columns: new[] { "StockId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStocks_Orders_OrderId",
                table: "OrderStocks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStocks_Stock_StockId",
                table: "OrderStocks",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStocks_Orders_OrderId",
                table: "OrderStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStocks_Stock_StockId",
                table: "OrderStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStocks",
                table: "OrderStocks");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StripeReference",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "OrderStocks");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "OrderStocks");

            migrationBuilder.RenameTable(
                name: "OrderStocks",
                newName: "OrderProducts");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStocks_OrderId",
                table: "OrderProducts",
                newName: "IX_OrderProducts_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts",
                columns: new[] { "ProductId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

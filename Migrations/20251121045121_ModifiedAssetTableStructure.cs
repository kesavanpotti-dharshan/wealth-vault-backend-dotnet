using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WealthVaultApi.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedAssetTableStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "PurchaseValue",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "YearlyYield",
                table: "Assets",
                newName: "AssetYield");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Assets",
                newName: "AssetTotalValue");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Assets",
                newName: "AssetType");

            migrationBuilder.RenameColumn(
                name: "Ticker",
                table: "Assets",
                newName: "AssetCurrency");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Assets",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Assets",
                newName: "AssetName");

            migrationBuilder.AddColumn<int>(
                name: "AssetCategory",
                table: "Assets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetCategory",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Assets",
                newName: "PurchaseDate");

            migrationBuilder.RenameColumn(
                name: "AssetYield",
                table: "Assets",
                newName: "YearlyYield");

            migrationBuilder.RenameColumn(
                name: "AssetType",
                table: "Assets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "AssetTotalValue",
                table: "Assets",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "AssetName",
                table: "Assets",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AssetCurrency",
                table: "Assets",
                newName: "Ticker");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseValue",
                table: "Assets",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Qty",
                table: "Assets",
                type: "numeric",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "Name", "PurchaseDate", "PurchaseValue", "Qty", "Ticker", "Type", "Value", "YearlyYield" },
                values: new object[,]
                {
                    { 1, "Chase Savings", null, null, null, null, "Bank", 75000m, 1200m },
                    { 2, "Bitcoin", new DateOnly(2025, 1, 20), 12000m, 1m, "bitcoin", "Crypto", null, null }
                });
        }
    }
}

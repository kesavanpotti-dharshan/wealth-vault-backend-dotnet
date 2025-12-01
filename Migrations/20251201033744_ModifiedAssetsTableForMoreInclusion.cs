using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthVaultApi.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedAssetsTableForMoreInclusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "YieldPercentage");

            migrationBuilder.RenameColumn(
                name: "AssetType",
                table: "Assets",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "AssetTotalValue",
                table: "Assets",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "AssetCurrency",
                table: "Assets",
                newName: "Ticker");

            migrationBuilder.AddColumn<decimal>(
                name: "AnnualIncome",
                table: "Assets",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostBasis",
                table: "Assets",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Assets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentValue",
                table: "Assets",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeFrequency",
                table: "Assets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Assets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastIncomeDate",
                table: "Assets",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Assets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "NextIncomeDate",
                table: "Assets",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Assets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePricePerUnit",
                table: "Assets",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryCurrency",
                table: "Assets",
                type: "text",
                nullable: true);

            // 1. Add AssetTypeId column as NULLABLE first
            migrationBuilder.AddColumn<int>(
                name: "AssetTypeId",
                table: "Assets",
                type: "integer",
                nullable: true);  // ← MUST be nullable first

            // 2. Update existing records to set AssetTypeId based on existing data
            migrationBuilder.Sql(@"
            UPDATE ""Assets""
            SET ""AssetTypeId"" = COALESCE(""AssetTypeId"", 1)
            WHERE ""AssetTypeId"" = 0 OR ""AssetTypeId"" IS NULL");

            
            migrationBuilder.CreateIndex(
         name: "IX_Assets_AssetTypeId",
         table: "Assets",
         column: "AssetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AssetTypes_AssetTypeId",
                table: "Assets",
                column: "AssetTypeId",
                principalTable: "AssetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AssetTypes_AssetTypeId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetTypeId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AnnualIncome",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AssetTypeId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CostBasis",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "IncomeFrequency",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "LastIncomeDate",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "NextIncomeDate",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PurchasePricePerUnit",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "SecondaryCurrency",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "YieldPercentage",
                table: "Assets",
                newName: "AssetYield");

            migrationBuilder.RenameColumn(
                name: "Ticker",
                table: "Assets",
                newName: "AssetCurrency");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Assets",
                newName: "AssetTotalValue");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Assets",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Assets",
                newName: "AssetType");

            migrationBuilder.AddColumn<string>(
                name: "AssetCategory",
                table: "Assets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

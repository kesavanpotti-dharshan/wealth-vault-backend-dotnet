using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WealthVaultApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedLiabilitiesAndObligationsModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiabilityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LiabilityTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IconName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiabilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObligationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ObligationTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IconName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObligationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Liabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LiabilityTypeId = table.Column<int>(type: "integer", nullable: false),
                    LiabilityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    OriginalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyPayment = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Creditor = table.Column<string>(type: "text", nullable: false),
                    IsSecured = table.Column<bool>(type: "boolean", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Liabilities_LiabilityTypes_LiabilityTypeId",
                        column: x => x.LiabilityTypeId,
                        principalTable: "LiabilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obligations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ObligationTypeId = table.Column<int>(type: "integer", nullable: false),
                    ObligationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MonthlyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    AnnualAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Beneficiary = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obligations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obligations_ObligationTypes_ObligationTypeId",
                        column: x => x.ObligationTypeId,
                        principalTable: "ObligationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LiabilityTypes",
                columns: new[] { "Id", "IconName", "IsActive", "LiabilityTypeName" },
                values: new object[,]
                {
                    { 1, "home-modern", true, "Mortgage" },
                    { 2, "academic-cap", true, "Student Loan" },
                    { 3, "credit-card", true, "Credit Card" },
                    { 4, "car", true, "Car Loan" },
                    { 5, "users", true, "Personal Loan" }
                });

            migrationBuilder.InsertData(
                table: "ObligationTypes",
                columns: new[] { "Id", "IconName", "IsActive", "ObligationTypeName" },
                values: new object[,]
                {
                    { 1, "heart", true, "Parents Support" },
                    { 2, "book-open", true, "Kids Education" },
                    { 3, "hand-raised", true, "Charity Pledge" },
                    { 4, "plus-circle", true, "Family Medical" },
                    { 5, "plus-circle", true, "Studies" },
                    { 6, "ellipsis-h", true, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Liabilities_LiabilityTypeId",
                table: "Liabilities",
                column: "LiabilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Obligations_ObligationTypeId",
                table: "Obligations",
                column: "ObligationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Liabilities");

            migrationBuilder.DropTable(
                name: "Obligations");

            migrationBuilder.DropTable(
                name: "LiabilityTypes");

            migrationBuilder.DropTable(
                name: "ObligationTypes");
        }
    }
}

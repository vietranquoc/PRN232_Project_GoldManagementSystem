using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddKaratAndPriceTypeToGoldType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Karat",
                table: "GoldType",
                type: "int",
                precision: 2,
                scale: 0,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceType",
                table: "GoldType",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Password" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$HztrffNhajQaClulYm7N5OwsePdtufa7t7arD6IVzoAxnjmBohDyu" });

            migrationBuilder.CreateIndex(
                name: "IX_GoldType_Name_Karat_PriceType",
                table: "GoldType",
                columns: new[] { "Name", "Karat", "PriceType" },
                unique: true,
                filter: "[Karat] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoldType_Name_Karat_PriceType",
                table: "GoldType");

            migrationBuilder.DropColumn(
                name: "Karat",
                table: "GoldType");

            migrationBuilder.DropColumn(
                name: "PriceType",
                table: "GoldType");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Password" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBAQHxQZxqKQHy" });
        }
    }
}

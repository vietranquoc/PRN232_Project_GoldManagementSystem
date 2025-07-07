using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDataOrPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedBy", "CreatedDate", "Email", "FullName", "IsActive", "Password", "PhoneNumber", "RoleId", "UpdatedBy", "UpdatedDate", "Username" },
                values: new object[] { 1, null, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Admin User", true, "$2a$11$HztrffNhajQaClulYm7N5OwsePdtufa7t7arD6IVzoAxnjmBohDyu", null, 3, null, null, "admin" });
        }
    }
}

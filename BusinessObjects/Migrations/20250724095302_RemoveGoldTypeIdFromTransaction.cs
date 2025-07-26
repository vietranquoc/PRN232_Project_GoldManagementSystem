using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGoldTypeIdFromTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_GoldType_GoldTypeId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "GoldTypeId",
                table: "Transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_GoldType_GoldTypeId",
                table: "Transaction",
                column: "GoldTypeId",
                principalTable: "GoldType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_GoldType_GoldTypeId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "GoldTypeId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_GoldType_GoldTypeId",
                table: "Transaction",
                column: "GoldTypeId",
                principalTable: "GoldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

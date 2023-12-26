using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixQuizIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizes_Name",
                table: "Quizes");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Quizes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Quizes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Quizes_UserName_Name",
                table: "Quizes",
                columns: new[] { "UserName", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizes_UserName_Name",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Quizes");

            migrationBuilder.CreateIndex(
                name: "IX_Quizes_Name",
                table: "Quizes",
                column: "Name",
                unique: true);
        }
    }
}

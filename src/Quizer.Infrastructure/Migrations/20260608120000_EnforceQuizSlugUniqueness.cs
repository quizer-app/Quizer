using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizer.Infrastructure.Migrations
{
    public partial class EnforceQuizSlugUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizes_UserName_Name",
                table: "Quizes");

            migrationBuilder.CreateIndex(
                name: "IX_Quizes_UserName_Slug",
                table: "Quizes",
                columns: new[] { "UserName", "Slug" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizes_UserName_Slug",
                table: "Quizes");

            migrationBuilder.CreateIndex(
                name: "IX_Quizes_UserName_Name",
                table: "Quizes",
                columns: new[] { "UserName", "Name" },
                unique: true);
        }
    }
}

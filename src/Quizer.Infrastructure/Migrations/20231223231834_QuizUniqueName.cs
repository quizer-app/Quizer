using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuizUniqueName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Quizes_Name",
                table: "Quizes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizes_Name",
                table: "Quizes");
        }
    }
}

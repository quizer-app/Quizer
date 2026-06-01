using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizer.Infrastructure.Migrations
{
    public partial class AddTimedQuizSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoSubmitWhenTimeExpires",
                table: "Quizes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxAttemptsPerUser",
                table: "Quizes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimitInSeconds",
                table: "Quizes",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoSubmitWhenTimeExpires",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "MaxAttemptsPerUser",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "TimeLimitInSeconds",
                table: "Quizes");
        }
    }
}

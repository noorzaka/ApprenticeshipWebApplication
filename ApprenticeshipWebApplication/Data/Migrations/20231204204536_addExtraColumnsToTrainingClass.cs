using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprenticeshipWebApplication.Data.Migrations
{
    public partial class addExtraColumnsToTrainingClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "estimatedNumberOfTasks",
                table: "trainings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "trainingName",
                table: "trainings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimatedNumberOfTasks",
                table: "trainings");

            migrationBuilder.DropColumn(
                name: "trainingName",
                table: "trainings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ApprenticeshipWebApplication.Data.Migrations
{
    public partial class AddDateToAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "endDate",
                table: "assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "timeRemaining",
                table: "assignments",
                type: "time", // Adjust the type as needed
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "endDate",
                table: "assignments");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "assignments");

            migrationBuilder.DropColumn(
                name: "timeRemaining",
                table: "assignments");
        }
    }
}

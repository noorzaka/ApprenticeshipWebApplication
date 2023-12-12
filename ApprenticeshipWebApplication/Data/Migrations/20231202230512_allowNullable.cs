using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprenticeshipWebApplication.Data.Migrations
{
    public partial class allowNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_assignments_assignmentId",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "FK_documents_reports_reportId",
                table: "documents");

            migrationBuilder.AlterColumn<int>(
                name: "reportId",
                table: "documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "assignmentId",
                table: "documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_assignments_assignmentId",
                table: "documents",
                column: "assignmentId",
                principalTable: "assignments",
                principalColumn: "assignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_reports_reportId",
                table: "documents",
                column: "reportId",
                principalTable: "reports",
                principalColumn: "reportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_assignments_assignmentId",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "FK_documents_reports_reportId",
                table: "documents");

            migrationBuilder.AlterColumn<int>(
                name: "reportId",
                table: "documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "assignmentId",
                table: "documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_documents_assignments_assignmentId",
                table: "documents",
                column: "assignmentId",
                principalTable: "assignments",
                principalColumn: "assignmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_documents_reports_reportId",
                table: "documents",
                column: "reportId",
                principalTable: "reports",
                principalColumn: "reportId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

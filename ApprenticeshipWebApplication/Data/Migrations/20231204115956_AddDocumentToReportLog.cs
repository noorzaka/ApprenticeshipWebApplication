using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprenticeshipWebApplication.Data.Migrations
{
    public partial class AddDocumentToReportLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "reportLogId",
                table: "documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_documents_reportLogId",
                table: "documents",
                column: "reportLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_reportLogs_reportLogId",
                table: "documents",
                column: "reportLogId",
                principalTable: "reportLogs",
                principalColumn: "reportLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_reportLogs_reportLogId",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "IX_documents_reportLogId",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "reportLogId",
                table: "documents");
        }
    }
}

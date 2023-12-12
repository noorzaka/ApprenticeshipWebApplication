using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprenticeshipWebApplication.Data.Migrations
{
    public partial class businessLayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Student_schoolId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "companyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "schoolId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    companyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    companyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.companyId);
                });

            migrationBuilder.CreateTable(
                name: "objectives",
                columns: table => new
                {
                    objectiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    objectiveName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objectives", x => x.objectiveId);
                });

            migrationBuilder.CreateTable(
                name: "reportStatuses",
                columns: table => new
                {
                    reportStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportStatuses", x => x.reportStatusId);
                });

            migrationBuilder.CreateTable(
                name: "schools",
                columns: table => new
                {
                    schoolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    schoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    schoolAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schools", x => x.schoolId);
                });

            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    skillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skillName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skills", x => x.skillId);
                });

            migrationBuilder.CreateTable(
                name: "trainings",
                columns: table => new
                {
                    trainingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    teamLeaderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    schoolSupervisorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trainings", x => x.trainingId);
                    table.ForeignKey(
                        name: "FK_trainings_AspNetUsers_schoolSupervisorId",
                        column: x => x.schoolSupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trainings_AspNetUsers_studentId",
                        column: x => x.studentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trainings_AspNetUsers_teamLeaderId",
                        column: x => x.teamLeaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "objectiveSkills",
                columns: table => new
                {
                    objectiveId = table.Column<int>(type: "int", nullable: false),
                    skillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objectiveSkills", x => new { x.objectiveId, x.skillId });
                    table.ForeignKey(
                        name: "FK_objectiveSkills_objectives_objectiveId",
                        column: x => x.objectiveId,
                        principalTable: "objectives",
                        principalColumn: "objectiveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_objectiveSkills_skills_skillId",
                        column: x => x.skillId,
                        principalTable: "skills",
                        principalColumn: "skillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    assignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assignmentTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assignmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assignmentNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trainingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => x.assignmentId);
                    table.ForeignKey(
                        name: "FK_assignments_trainings_trainingId",
                        column: x => x.trainingId,
                        principalTable: "trainings",
                        principalColumn: "trainingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trainingObjectives",
                columns: table => new
                {
                    trainingId = table.Column<int>(type: "int", nullable: false),
                    objectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trainingObjectives", x => new { x.objectiveId, x.trainingId });
                    table.ForeignKey(
                        name: "FK_trainingObjectives_objectives_objectiveId",
                        column: x => x.objectiveId,
                        principalTable: "objectives",
                        principalColumn: "objectiveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trainingObjectives_trainings_trainingId",
                        column: x => x.trainingId,
                        principalTable: "trainings",
                        principalColumn: "trainingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignmentObjectives",
                columns: table => new
                {
                    assignmentId = table.Column<int>(type: "int", nullable: false),
                    objectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignmentObjectives", x => new { x.objectiveId, x.assignmentId });
                    table.ForeignKey(
                        name: "FK_assignmentObjectives_assignments_assignmentId",
                        column: x => x.assignmentId,
                        principalTable: "assignments",
                        principalColumn: "assignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assignmentObjectives_objectives_objectiveId",
                        column: x => x.objectiveId,
                        principalTable: "objectives",
                        principalColumn: "objectiveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    reportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assignmentId = table.Column<int>(type: "int", nullable: false),
                    reportStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.reportId);
                    table.ForeignKey(
                        name: "FK_reports_assignments_assignmentId",
                        column: x => x.assignmentId,
                        principalTable: "assignments",
                        principalColumn: "assignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reports_reportStatuses_reportStatusId",
                        column: x => x.reportStatusId,
                        principalTable: "reportStatuses",
                        principalColumn: "reportStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_companyId",
                table: "AspNetUsers",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_schoolId",
                table: "AspNetUsers",
                column: "schoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Student_schoolId",
                table: "AspNetUsers",
                column: "Student_schoolId");

            migrationBuilder.CreateIndex(
                name: "IX_assignmentObjectives_assignmentId",
                table: "assignmentObjectives",
                column: "assignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_trainingId",
                table: "assignments",
                column: "trainingId");

            migrationBuilder.CreateIndex(
                name: "IX_objectiveSkills_skillId",
                table: "objectiveSkills",
                column: "skillId");

            migrationBuilder.CreateIndex(
                name: "IX_reports_assignmentId",
                table: "reports",
                column: "assignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_reports_reportStatusId",
                table: "reports",
                column: "reportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_trainingObjectives_trainingId",
                table: "trainingObjectives",
                column: "trainingId");

            migrationBuilder.CreateIndex(
                name: "IX_trainings_schoolSupervisorId",
                table: "trainings",
                column: "schoolSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_trainings_studentId",
                table: "trainings",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_trainings_teamLeaderId",
                table: "trainings",
                column: "teamLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_companies_companyId",
                table: "AspNetUsers",
                column: "companyId",
                principalTable: "companies",
                principalColumn: "companyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_schools_schoolId",
                table: "AspNetUsers",
                column: "schoolId",
                principalTable: "schools",
                principalColumn: "schoolId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_schools_Student_schoolId",
                table: "AspNetUsers",
                column: "Student_schoolId",
                principalTable: "schools",
                principalColumn: "schoolId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_companies_companyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_schools_schoolId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_schools_Student_schoolId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "assignmentObjectives");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "objectiveSkills");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "schools");

            migrationBuilder.DropTable(
                name: "trainingObjectives");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "reportStatuses");

            migrationBuilder.DropTable(
                name: "objectives");

            migrationBuilder.DropTable(
                name: "trainings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_companyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_schoolId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Student_schoolId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Student_schoolId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "companyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "schoolId",
                table: "AspNetUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfilePicture = table.Column<string>(nullable: true),
                    StudentId = table.Column<string>(maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(nullable: true),
                    AcquiredCredits = table.Column<int>(nullable: true),
                    CurrentSemestar = table.Column<int>(nullable: true),
                    EducationLevel = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Degree = table.Column<string>(maxLength: 50, nullable: true),
                    AcademicRank = table.Column<string>(maxLength: 25, nullable: true),
                    OfficeNumber = table.Column<string>(maxLength: 10, nullable: true),
                    HireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Credits = table.Column<int>(nullable: false),
                    Semester = table.Column<int>(nullable: false),
                    Programme = table.Column<string>(maxLength: 100, nullable: true),
                    EducationLevel = table.Column<string>(maxLength: 25, nullable: true),
                    FirstTeacherID = table.Column<int>(nullable: true),
                    SecondTeacherID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Teacher_FirstTeacherID",
                        column: x => x.FirstTeacherID,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Course_Teacher_SecondTeacherID",
                        column: x => x.SecondTeacherID,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    Semester = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Grade = table.Column<int>(nullable: true),
                    SeminarUrl = table.Column<string>(nullable: true),
                    ProjectUrl = table.Column<string>(nullable: true),
                    ExamPoints = table.Column<int>(nullable: true),
                    SeminarPoints = table.Column<int>(nullable: true),
                    ProjectPoints = table.Column<int>(nullable: true),
                    AdditionalPoints = table.Column<int>(nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_FirstTeacherID",
                table: "Course",
                column: "FirstTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SecondTeacherID",
                table: "Course",
                column: "SecondTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Migrations
{
    public partial class updateinstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "People");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "Courses",
                nullable: false,
                defaultValue: "");
        }
    }
}

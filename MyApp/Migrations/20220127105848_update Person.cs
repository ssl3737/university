using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Migrations
{
    public partial class updatePerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_People_InstructorPersonId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "People");

            migrationBuilder.RenameTable(
                name: "People",
                newName: "Instructors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructors_InstructorPersonId",
                table: "Courses",
                column: "InstructorPersonId",
                principalTable: "Instructors",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructors_InstructorPersonId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors");

            migrationBuilder.RenameTable(
                name: "Instructors",
                newName: "People");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "People",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_People_InstructorPersonId",
                table: "Courses",
                column: "InstructorPersonId",
                principalTable: "People",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

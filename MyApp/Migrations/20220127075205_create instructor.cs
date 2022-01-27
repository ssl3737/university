using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Migrations
{
    public partial class createinstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorPersonId",
                table: "Courses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    HireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorPersonId",
                table: "Courses",
                column: "InstructorPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_People_InstructorPersonId",
                table: "Courses",
                column: "InstructorPersonId",
                principalTable: "People",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_People_InstructorPersonId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropIndex(
                name: "IX_Courses_InstructorPersonId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "InstructorPersonId",
                table: "Courses");
        }
    }
}

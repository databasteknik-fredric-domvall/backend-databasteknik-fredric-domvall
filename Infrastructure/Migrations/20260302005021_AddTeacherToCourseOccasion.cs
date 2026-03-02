using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherToCourseOccasion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "CourseOccasions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseOccasions_TeacherId",
                table: "CourseOccasions",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOccasions_Teachers_TeacherId",
                table: "CourseOccasions",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOccasions_Teachers_TeacherId",
                table: "CourseOccasions");

            migrationBuilder.DropIndex(
                name: "IX_CourseOccasions_TeacherId",
                table: "CourseOccasions");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "CourseOccasions");
        }
    }
}

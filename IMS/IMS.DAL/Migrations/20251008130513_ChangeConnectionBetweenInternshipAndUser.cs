using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConnectionBetweenInternshipAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Internships_InternId",
                table: "Internships");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Internships",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Internships_InternId",
                table: "Internships",
                column: "InternId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_UserId",
                table: "Internships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Internships_Users_UserId",
                table: "Internships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Internships_Users_UserId",
                table: "Internships");

            migrationBuilder.DropIndex(
                name: "IX_Internships_InternId",
                table: "Internships");

            migrationBuilder.DropIndex(
                name: "IX_Internships_UserId",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Internships");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_InternId",
                table: "Internships",
                column: "InternId",
                unique: true);
        }
    }
}

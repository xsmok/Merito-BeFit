using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeFit.Migrations
{
    /// <inheritdoc />
    public partial class JoinedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ExerciseType",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ExerciseSession",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Exercise",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseType_CreatedById",
                table: "ExerciseType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSession_CreatedById",
                table: "ExerciseSession",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_CreatedById",
                table: "Exercise",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_AspNetUsers_CreatedById",
                table: "Exercise",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSession_AspNetUsers_CreatedById",
                table: "ExerciseSession",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseType_AspNetUsers_CreatedById",
                table: "ExerciseType",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_AspNetUsers_CreatedById",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSession_AspNetUsers_CreatedById",
                table: "ExerciseSession");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseType_AspNetUsers_CreatedById",
                table: "ExerciseType");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseType_CreatedById",
                table: "ExerciseType");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseSession_CreatedById",
                table: "ExerciseSession");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_CreatedById",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ExerciseType");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ExerciseSession");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Exercise");
        }
    }
}

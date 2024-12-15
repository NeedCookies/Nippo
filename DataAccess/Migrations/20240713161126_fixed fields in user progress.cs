using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixedfieldsinuserprogress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_UserProgress",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Quizzes_ElementId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_ElementId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "ElementId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "ElementType",
                table: "UserProgresses");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "UserProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "UserProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LessonId",
                table: "UserProgresses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_QuizId",
                table: "UserProgresses",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Lessons_LessonId",
                table: "UserProgresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Quizzes_QuizId",
                table: "UserProgresses",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Lessons_LessonId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Quizzes_QuizId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_LessonId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_QuizId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "UserProgresses");

            migrationBuilder.AddColumn<int>(
                name: "ElementId",
                table: "UserProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ElementType",
                table: "UserProgresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_ElementId",
                table: "UserProgresses",
                column: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_UserProgress",
                table: "UserProgresses",
                column: "ElementId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Quizzes_ElementId",
                table: "UserProgresses",
                column: "ElementId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFServer.Migrations
{
    /// <inheritdoc />
    public partial class isLikedExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExercisePerson",
                columns: table => new
                {
                    ExercisesId = table.Column<int>(type: "int", nullable: false),
                    PersonsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisePerson", x => new { x.ExercisesId, x.PersonsId });
                    table.ForeignKey(
                        name: "FK_ExercisePerson_AspNetUsers_PersonsId",
                        column: x => x.PersonsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExercisePerson_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercisePerson_PersonsId",
                table: "ExercisePerson",
                column: "PersonsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercisePerson");
        }
    }
}

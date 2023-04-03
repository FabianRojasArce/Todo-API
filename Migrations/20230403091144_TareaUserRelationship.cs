using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class TareaUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tareas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_UserId",
                table: "Tareas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_AspNetUsers_UserId",
                table: "Tareas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_AspNetUsers_UserId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_UserId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tareas");
        }
    }
}

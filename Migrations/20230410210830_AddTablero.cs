using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTablero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableroId",
                table: "Tareas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tableros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tableros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tableros_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_TableroId",
                table: "Tareas",
                column: "TableroId");

            migrationBuilder.CreateIndex(
                name: "IX_Tableros_UserId",
                table: "Tableros",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Tableros_TableroId",
                table: "Tareas",
                column: "TableroId",
                principalTable: "Tableros",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Tableros_TableroId",
                table: "Tareas");

            migrationBuilder.DropTable(
                name: "Tableros");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_TableroId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "TableroId",
                table: "Tareas");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddListado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListadoId",
                table: "Tareas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Listados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listados_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ListadoId",
                table: "Tareas",
                column: "ListadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Listados_UserId",
                table: "Listados",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Listados_ListadoId",
                table: "Tareas",
                column: "ListadoId",
                principalTable: "Listados",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Listados_ListadoId",
                table: "Tareas");

            migrationBuilder.DropTable(
                name: "Listados");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_ListadoId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ListadoId",
                table: "Tareas");
        }
    }
}

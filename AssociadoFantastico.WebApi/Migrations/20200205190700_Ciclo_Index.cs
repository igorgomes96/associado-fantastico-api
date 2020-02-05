using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class Ciclo_Index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ciclos_Ano_Semestre_EmpresaId",
                table: "Ciclos",
                columns: new[] { "Ano", "Semestre", "EmpresaId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ciclos_Ano_Semestre_EmpresaId",
                table: "Ciclos");
        }
    }
}

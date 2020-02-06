using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class GruposPorCiclo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Grupos");

            migrationBuilder.AddColumn<Guid>(
                name: "CicloId",
                table: "Grupos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_CicloId",
                table: "Grupos",
                column: "CicloId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Nome_CicloId",
                table: "Grupos",
                columns: new[] { "Nome", "CicloId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_Ciclos_CicloId",
                table: "Grupos",
                column: "CicloId",
                principalTable: "Ciclos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_Ciclos_CicloId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Grupos_CicloId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Grupos_Nome_CicloId",
                table: "Grupos");

            migrationBuilder.DropColumn(
                name: "CicloId",
                table: "Grupos");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Grupos",
                nullable: false,
                defaultValue: false);
        }
    }
}

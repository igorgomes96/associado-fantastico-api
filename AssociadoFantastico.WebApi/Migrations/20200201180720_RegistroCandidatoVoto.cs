using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class RegistroCandidatoVoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Associados_AssociadoId",
                table: "Votos");

            migrationBuilder.RenameColumn(
                name: "AssociadoId",
                table: "Votos",
                newName: "EleitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Votos_AssociadoId",
                table: "Votos",
                newName: "IX_Votos_EleitorId");

            migrationBuilder.AddColumn<Guid>(
                name: "CandidatoId",
                table: "Votos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Votos_CandidatoId",
                table: "Votos",
                column: "CandidatoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Elegiveis_CandidatoId",
                table: "Votos",
                column: "CandidatoId",
                principalTable: "Elegiveis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Associados_EleitorId",
                table: "Votos",
                column: "EleitorId",
                principalTable: "Associados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Elegiveis_CandidatoId",
                table: "Votos");

            migrationBuilder.DropForeignKey(
                name: "FK_Votos_Associados_EleitorId",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Votos_CandidatoId",
                table: "Votos");

            migrationBuilder.DropColumn(
                name: "CandidatoId",
                table: "Votos");

            migrationBuilder.RenameColumn(
                name: "EleitorId",
                table: "Votos",
                newName: "AssociadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Votos_EleitorId",
                table: "Votos",
                newName: "IX_Votos_AssociadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votos_Associados_AssociadoId",
                table: "Votos",
                column: "AssociadoId",
                principalTable: "Associados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

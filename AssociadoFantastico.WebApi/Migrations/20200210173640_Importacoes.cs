using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class Importacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Importacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    VotacaoId = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Importacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Importacoes_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inconsistencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Coluna = table.Column<string>(maxLength: 100, nullable: true),
                    Linha = table.Column<int>(nullable: false),
                    Mensagem = table.Column<string>(maxLength: 255, nullable: true),
                    ImportacaoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inconsistencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inconsistencias_Importacoes_ImportacaoId",
                        column: x => x.ImportacaoId,
                        principalTable: "Importacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Importacoes_VotacaoId",
                table: "Importacoes",
                column: "VotacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inconsistencias_ImportacaoId",
                table: "Inconsistencias",
                column: "ImportacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inconsistencias");

            migrationBuilder.DropTable(
                name: "Importacoes");
        }
    }
}

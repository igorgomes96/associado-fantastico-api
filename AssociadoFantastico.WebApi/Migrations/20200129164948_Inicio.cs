using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class Inicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(maxLength: 150, nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciclos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Ano = table.Column<int>(nullable: false),
                    Semestre = table.Column<int>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 255, nullable: false),
                    EmpresaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciclos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciclos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cpf = table.Column<string>(maxLength: 11, nullable: false),
                    Matricula = table.Column<string>(maxLength: 20, nullable: false),
                    Nome = table.Column<string>(maxLength: 255, nullable: false),
                    Cargo = table.Column<string>(maxLength: 255, nullable: true),
                    Area = table.Column<string>(maxLength: 255, nullable: true),
                    Perfil = table.Column<int>(nullable: false),
                    EmpresaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PeriodoPrevisto_DataInicio = table.Column<DateTime>(nullable: false),
                    PeriodoPrevisto_DataFim = table.Column<DateTime>(nullable: false),
                    PeriodoRealizado_DataInicio = table.Column<DateTime>(nullable: true),
                    PeriodoRealizado_DataFim = table.Column<DateTime>(nullable: true),
                    CicloId = table.Column<Guid>(nullable: false),
                    TipoVotacao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votacoes_Ciclos_CicloId",
                        column: x => x.CicloId,
                        principalTable: "Ciclos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Associados",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioId = table.Column<Guid>(nullable: false),
                    GrupoId = table.Column<Guid>(nullable: false),
                    CicloId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Associados_Ciclos_CicloId",
                        column: x => x.CicloId,
                        principalTable: "Ciclos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Associados_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Associados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Elegiveis",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Aplausogramas = table.Column<int>(nullable: false),
                    Foto = table.Column<string>(maxLength: 255, nullable: true),
                    AssociadoId = table.Column<Guid>(nullable: false),
                    VotacaoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elegiveis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Elegiveis_Associados_AssociadoId",
                        column: x => x.AssociadoId,
                        principalTable: "Associados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elegiveis_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VotacaoId = table.Column<Guid>(nullable: false),
                    AssociadoId = table.Column<Guid>(nullable: false),
                    Horario = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votos_Associados_AssociadoId",
                        column: x => x.AssociadoId,
                        principalTable: "Associados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votos_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Associados_CicloId",
                table: "Associados",
                column: "CicloId");

            migrationBuilder.CreateIndex(
                name: "IX_Associados_GrupoId",
                table: "Associados",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Associados_UsuarioId",
                table: "Associados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciclos_EmpresaId",
                table: "Ciclos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Elegiveis_AssociadoId",
                table: "Elegiveis",
                column: "AssociadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Elegiveis_VotacaoId_AssociadoId",
                table: "Elegiveis",
                columns: new[] { "VotacaoId", "AssociadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Cpf",
                table: "Usuarios",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Votacoes_CicloId",
                table: "Votacoes",
                column: "CicloId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_AssociadoId",
                table: "Votos",
                column: "AssociadoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votos_VotacaoId",
                table: "Votos",
                column: "VotacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elegiveis");

            migrationBuilder.DropTable(
                name: "Votos");

            migrationBuilder.DropTable(
                name: "Associados");

            migrationBuilder.DropTable(
                name: "Votacoes");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ciclos");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}

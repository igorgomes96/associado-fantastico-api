using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class FinalizacaoCiclo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataFinalizacao",
                table: "Ciclos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CentroCusto",
                table: "Associados",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFinalizacao",
                table: "Ciclos");

            migrationBuilder.DropColumn(
                name: "CentroCusto",
                table: "Associados");
        }
    }
}

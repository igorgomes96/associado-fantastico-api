using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociadoFantastico.WebApi.Migrations
{
    public partial class Dimensionamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Aplausogramas",
                table: "Elegiveis",
                newName: "Votos");

            migrationBuilder.AddColumn<int>(
                name: "Dimensionamento_Acrescimo",
                table: "Votacoes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dimensionamento_Intervalo",
                table: "Votacoes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Apuracao",
                table: "Elegiveis",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Aplausogramas",
                table: "Associados",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensionamento_Acrescimo",
                table: "Votacoes");

            migrationBuilder.DropColumn(
                name: "Dimensionamento_Intervalo",
                table: "Votacoes");

            migrationBuilder.DropColumn(
                name: "Apuracao",
                table: "Elegiveis");

            migrationBuilder.DropColumn(
                name: "Aplausogramas",
                table: "Associados");

            migrationBuilder.RenameColumn(
                name: "Votos",
                table: "Elegiveis",
                newName: "Aplausogramas");
        }
    }
}

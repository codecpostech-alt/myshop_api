using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrixToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PrixVente",
                table: "Produits",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<int>(
                name: "PrixAchat",
                table: "Produits",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(10,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PrixVente",
                table: "Produits",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "PrixAchat",
                table: "Produits",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}

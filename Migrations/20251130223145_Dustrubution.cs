using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyShop.Migrations
{
    /// <inheritdoc />
    public partial class Dustrubution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chauffeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prenom = table.Column<string>(type: "text", nullable: false),
                    Telephone = table.Column<string>(type: "text", nullable: false),
                    NumeroPermis = table.Column<string>(type: "text", nullable: false),
                    ExpirationPermis = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Camion = table.Column<string>(type: "text", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chauffeurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreVendeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prenom = table.Column<string>(type: "text", nullable: false),
                    Telephone = table.Column<string>(type: "text", nullable: false),
                    Zone = table.Column<string>(type: "text", nullable: false),
                    ObjectifMensuel = table.Column<decimal>(type: "numeric", nullable: false),
                    RealisationMensuelle = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreVendeurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChauffeurId = table.Column<int>(type: "integer", nullable: false),
                    PreVendeurId = table.Column<int>(type: "integer", nullable: false),
                    Zone = table.Column<string>(type: "text", nullable: false),
                    DateTournee = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournees_Chauffeurs_ChauffeurId",
                        column: x => x.ChauffeurId,
                        principalTable: "Chauffeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournees_PreVendeurs_PreVendeurId",
                        column: x => x.PreVendeurId,
                        principalTable: "PreVendeurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livraisons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TourneeId = table.Column<int>(type: "integer", nullable: false),
                    ClientNom = table.Column<string>(type: "text", nullable: false),
                    DateLivraison = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstLivre = table.Column<bool>(type: "boolean", nullable: false),
                    SignatureClient = table.Column<string>(type: "text", nullable: false),
                    MontantPaye = table.Column<decimal>(type: "numeric", nullable: false),
                    ModePaiement = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livraisons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livraisons_Tournees_TourneeId",
                        column: x => x.TourneeId,
                        principalTable: "Tournees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetourProduits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TourneeId = table.Column<int>(type: "integer", nullable: false),
                    Produit = table.Column<string>(type: "text", nullable: false),
                    Quantite = table.Column<int>(type: "integer", nullable: false),
                    TypeRetour = table.Column<string>(type: "text", nullable: false),
                    DateRetour = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetourProduits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetourProduits_Tournees_TourneeId",
                        column: x => x.TourneeId,
                        principalTable: "Tournees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourneeClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TourneeId = table.Column<int>(type: "integer", nullable: false),
                    ClientNom = table.Column<string>(type: "text", nullable: false),
                    Adresse = table.Column<string>(type: "text", nullable: false),
                    Ordre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourneeClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourneeClients_Tournees_TourneeId",
                        column: x => x.TourneeId,
                        principalTable: "Tournees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivraisonProduits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LivraisonId = table.Column<int>(type: "integer", nullable: false),
                    Produit = table.Column<string>(type: "text", nullable: false),
                    QteDemandee = table.Column<int>(type: "integer", nullable: false),
                    QteLivree = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivraisonProduits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivraisonProduits_Livraisons_LivraisonId",
                        column: x => x.LivraisonId,
                        principalTable: "Livraisons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivraisonProduits_LivraisonId",
                table: "LivraisonProduits",
                column: "LivraisonId");

            migrationBuilder.CreateIndex(
                name: "IX_Livraisons_TourneeId",
                table: "Livraisons",
                column: "TourneeId");

            migrationBuilder.CreateIndex(
                name: "IX_RetourProduits_TourneeId",
                table: "RetourProduits",
                column: "TourneeId");

            migrationBuilder.CreateIndex(
                name: "IX_TourneeClients_TourneeId",
                table: "TourneeClients",
                column: "TourneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournees_ChauffeurId",
                table: "Tournees",
                column: "ChauffeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournees_PreVendeurId",
                table: "Tournees",
                column: "PreVendeurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivraisonProduits");

            migrationBuilder.DropTable(
                name: "RetourProduits");

            migrationBuilder.DropTable(
                name: "TourneeClients");

            migrationBuilder.DropTable(
                name: "Livraisons");

            migrationBuilder.DropTable(
                name: "Tournees");

            migrationBuilder.DropTable(
                name: "Chauffeurs");

            migrationBuilder.DropTable(
                name: "PreVendeurs");
        }
    }
}

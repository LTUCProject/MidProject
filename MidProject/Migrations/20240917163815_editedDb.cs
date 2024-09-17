using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MidProject.Migrations
{
    /// <inheritdoc />
    public partial class editedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.CreateTable(
                name: "ChargingStationFavorites",
                columns: table => new
                {
                    ChargingStationFavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargingStationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStationFavorites", x => x.ChargingStationFavoriteId);
                    table.ForeignKey(
                        name: "FK_ChargingStationFavorites_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "ChargingStationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChargingStationFavorites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInfoFavorites",
                columns: table => new
                {
                    ServiceInfoFavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInfoFavorites", x => x.ServiceInfoFavoriteId);
                    table.ForeignKey(
                        name: "FK_ServiceInfoFavorites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceInfoFavorites_ServiceInfos_ServiceInfoId",
                        column: x => x.ServiceInfoId,
                        principalTable: "ServiceInfos",
                        principalColumn: "ServiceInfoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin_role_id",
                column: "ConcurrencyStamp",
                value: "e006589a-cc71-4d5b-846c-7450aa5a495a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "bf0d2004-6ba8-443e-b24c-71de388ad88a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "85099c52-f178-4c3a-896c-46d00ef878ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "3c626d9d-2ec1-4b36-b853-9148a60cbf26");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationFavorites_ChargingStationId",
                table: "ChargingStationFavorites",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationFavorites_ClientId",
                table: "ChargingStationFavorites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInfoFavorites_ClientId",
                table: "ServiceInfoFavorites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInfoFavorites_ServiceInfoId",
                table: "ServiceInfoFavorites",
                column: "ServiceInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargingStationFavorites");

            migrationBuilder.DropTable(
                name: "ServiceInfoFavorites");

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargingStationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ServiceInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_Favorites_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "ChargingStationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favorites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favorites_ServiceInfos_ServiceInfoId",
                        column: x => x.ServiceInfoId,
                        principalTable: "ServiceInfos",
                        principalColumn: "ServiceInfoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin_role_id",
                column: "ConcurrencyStamp",
                value: "5568d46d-1622-45b1-a452-33237cb5ba23");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "e6e757f7-9942-48dc-932f-574b93c827ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "6adb9808-27b2-411e-8ec4-7d6be6418bb0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "1d2caef0-5fd4-4c00-979d-95e6d48361f0");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ChargingStationId",
                table: "Favorites",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ClientId",
                table: "Favorites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ServiceInfoId",
                table: "Favorites",
                column: "ServiceInfoId");
        }
    }
}

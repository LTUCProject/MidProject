using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MidProject.Migrations
{
    /// <inheritdoc />
    public partial class AddGetSessionsByChargingStationAsync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin_role_id",
                column: "ConcurrencyStamp",
                value: "29da9cd0-e54f-4ea3-9c1a-8587f88dc06f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "6070581c-874f-4b66-b9e5-4d7d7129f062");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "af0d6d16-2107-4458-a23e-d1e34a9442c7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "bcabc659-a7e2-4407-a446-a7bb358ac232");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ProviderId",
                table: "Sessions",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Providers_ProviderId",
                table: "Sessions",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "ProviderId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Providers_ProviderId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_ProviderId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Sessions");

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
        }
    }
}

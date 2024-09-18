using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MidProject.Migrations
{
    /// <inheritdoc />
    public partial class editTabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: "f15e9239-f9a5-4e5d-a2f3-52cd04c1b2ee");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "aecc4007-e5e5-4f36-ae92-98e53e128096");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "394865e0-3170-4c47-961a-0f3a89b229f1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "40aefd76-a499-46b3-96c6-d4c3a94e097f");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: "2da02577-8fd3-4d21-bb2d-5ac251b3b0b6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "f9fb523e-f1b5-42a9-beaf-bb289759dbf8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "4a585e46-376a-4a12-9fc2-bb2987760312");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "2692969a-097c-4b4d-b889-be359eb6fa05");

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
    }
}

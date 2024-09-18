using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MidProject.Migrations
{
    /// <inheritdoc />
    public partial class addCostToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin_role_id",
                column: "ConcurrencyStamp",
                value: "624a3aa1-02d5-4a85-8aea-ab6703ed0020");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "814fa491-8960-469f-80f7-04d3a62dd512");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "3308af07-d50d-41c5-b21c-4dd2c5eca54d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "a29e04bb-d94a-4c6d-98ee-f9e98f404eed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
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
    }
}

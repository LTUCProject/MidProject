using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MidProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostAndCommentForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Clients_ClientId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ClientId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ClientId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin_role_id",
                column: "ConcurrencyStamp",
                value: "47f5fc9c-ec1f-4918-82bc-4889edd77b1c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "client_role_id",
                column: "ConcurrencyStamp",
                value: "a9551ebb-e230-4183-8514-3d3f62462e19");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner_role_id",
                column: "ConcurrencyStamp",
                value: "44876518-54e8-4ea4-8eef-3bab7f016bf3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "servicer_role_id",
                column: "ConcurrencyStamp",
                value: "d1d66685-7248-43fb-9f03-c3665454e0c6");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AccountId",
                table: "Posts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AccountId",
                table: "Comments",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AccountId",
                table: "Comments",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AccountId",
                table: "Posts",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AccountId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AccountId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AccountId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AccountId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Comments",
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
                name: "IX_Posts_ClientId",
                table: "Posts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClientId",
                table: "Comments",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Clients_ClientId",
                table: "Posts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

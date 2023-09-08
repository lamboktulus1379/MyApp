using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enjoyer.Infrastructure.Migrations
{
    public partial class AddedCreatedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "339f575a-d7ac-46d5-a305-e90aa3c12f79");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "361e3d3d-8f44-430a-870d-c47f507b9d07");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "33540be1-0eae-47b5-aaed-7111c0f6e8a3", "63fa78da-f945-4b87-8758-68feda5bf6c2", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4da2cf64-b92e-4ae9-88d5-d48ddcb93093", "ba9bb584-c22e-42b4-ac20-5b22e4e15ca7", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33540be1-0eae-47b5-aaed-7111c0f6e8a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4da2cf64-b92e-4ae9-88d5-d48ddcb93093");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "339f575a-d7ac-46d5-a305-e90aa3c12f79", "df65d13e-8ae4-4512-9823-a4ab93ab08c1", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "361e3d3d-8f44-430a-870d-c47f507b9d07", "49d2a4d0-b3b4-4e8c-a45b-48588d16d867", "Administrator", "ADMINISTRATOR" });
        }
    }
}

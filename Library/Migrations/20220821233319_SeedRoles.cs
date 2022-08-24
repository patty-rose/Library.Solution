using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b4a79c4-18ad-42cf-a3a5-2be56051c085", "66a3f850-32e6-465a-be1d-d5b215c4ef3e", "Librarian", "LIBRARIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb63dde5-7f87-4e23-9468-e3da63a946dc", "a071cee6-8855-4959-ad98-9c0fb146b7d8", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b4a79c4-18ad-42cf-a3a5-2be56051c085");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb63dde5-7f87-4e23-9468-e3da63a946dc");
        }
    }
}

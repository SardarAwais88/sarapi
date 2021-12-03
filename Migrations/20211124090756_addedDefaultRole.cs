using Microsoft.EntityFrameworkCore.Migrations;

namespace sarapi.Migrations
{
    public partial class addedDefaultRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d6405f88-e5da-488c-a38f-4f8b2944d2e1", "0e4e33b8-f014-4fdf-b168-3a43dcac3006", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "242d2e39-eba4-48d4-bfa9-abf319b19579", "bc976598-b6f9-45de-ae29-44735cf81c1c", "Administrator", "Administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "242d2e39-eba4-48d4-bfa9-abf319b19579");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6405f88-e5da-488c-a38f-4f8b2944d2e1");
        }
    }
}

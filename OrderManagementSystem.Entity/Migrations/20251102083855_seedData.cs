using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementSystem.Entity.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fa4156b1-eee5-4e0d-92c1-cbb7cc09c7c5", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8202f7bc-1e9a-4db9-b837-025200fe4485", 0, "2813c5f7-7dee-4eca-b25a-6361fe2ea272", "admin@email.com", false, false, false, null, "ADMIN@EMAIL.COM", "ADMIN@EMAIL.COM", "AQAAAAIAAYagAAAAEHQavqiY2GKRKl63IlUbOqr9UV+ptJ6oQX3Fb5IujqNNs4Sd5Z45tuGMqyakTsAqtA==", null, false, "92d0db0b-0c84-47da-ba11-632be0d5a601", false, "admin@email.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fa4156b1-eee5-4e0d-92c1-cbb7cc09c7c5", "8202f7bc-1e9a-4db9-b837-025200fe4485" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fa4156b1-eee5-4e0d-92c1-cbb7cc09c7c5", "8202f7bc-1e9a-4db9-b837-025200fe4485" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa4156b1-eee5-4e0d-92c1-cbb7cc09c7c5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8202f7bc-1e9a-4db9-b837-025200fe4485");
        }
    }
}

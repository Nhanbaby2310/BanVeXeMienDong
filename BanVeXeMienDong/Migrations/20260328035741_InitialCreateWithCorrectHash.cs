using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BanVeXeMienDong.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithCorrectHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "NKN46HZxLgle0QWaTuROoqx8k4Md1PWOXpgIU6a3mZ4=", "Admin", "quanly" },
                    { 2, "N9XB5N9pqH5Vn/F0LrfWvJwFWYW+Xc2yXgNM7f2RJIc=", "User", "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

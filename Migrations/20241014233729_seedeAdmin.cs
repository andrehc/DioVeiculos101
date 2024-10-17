using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DioVeiculos101.Migrations
{
    /// <inheritdoc />
    public partial class seedeAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "Password", "Profile", "Username" },
                values: new object[,]
                {
                    { 1, "john@teste.com", "102030", "admin", "John Doe" },
                    { 2, "marc@teste.com", "102030", "n1", "Marc Goddart" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

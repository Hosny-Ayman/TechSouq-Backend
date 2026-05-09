using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSouq_DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addEmailToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Addresses",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Addresses");
        }
    }
}

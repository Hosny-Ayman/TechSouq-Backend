using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSouq_DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddFNandSNtoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "SecondName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "NVARCHAR(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SecondName",
                table: "Users",
                newName: "Name");
        }
    }
}

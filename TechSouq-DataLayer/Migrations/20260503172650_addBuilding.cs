using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSouq_DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addBuilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<string>(
                name: "building",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "OldPassword");
        }
    }
}

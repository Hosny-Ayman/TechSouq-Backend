using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSouq.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editDeliveryZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryZoneId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryZoneId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZones",
                principalColumn: "Id");
        }
    }
}

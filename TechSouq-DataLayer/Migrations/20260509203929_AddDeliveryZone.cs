using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSouq_DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryCost",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryZoneId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingGovernorate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryZone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZone", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryZone_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZone",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryZone_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryZone");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryCost",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryZoneId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingGovernorate",
                table: "Orders");
        }
    }
}

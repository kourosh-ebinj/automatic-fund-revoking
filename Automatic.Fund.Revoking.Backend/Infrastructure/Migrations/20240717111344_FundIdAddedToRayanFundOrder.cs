using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FundIdAddedToRayanFundOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RayanCustomers_Funds_FundId",
                table: "RayanCustomers");

            migrationBuilder.AddColumn<int>(
                name: "FundId",
                table: "RayanFundOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RayanFundOrders_FundId",
                table: "RayanFundOrders",
                column: "FundId");

            migrationBuilder.AddForeignKey(
                name: "FK_RayanCustomers_Funds_FundId",
                table: "RayanCustomers",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RayanFundOrders_Funds_FundId",
                table: "RayanFundOrders",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RayanCustomers_Funds_FundId",
                table: "RayanCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_RayanFundOrders_Funds_FundId",
                table: "RayanFundOrders");

            migrationBuilder.DropIndex(
                name: "IX_RayanFundOrders_FundId",
                table: "RayanFundOrders");

            migrationBuilder.DropColumn(
                name: "FundId",
                table: "RayanFundOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_RayanCustomers_Funds_FundId",
                table: "RayanCustomers",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

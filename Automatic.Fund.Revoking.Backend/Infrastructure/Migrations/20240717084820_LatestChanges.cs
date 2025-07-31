using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LatestChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RayanCustomers_CustomerId",
                table: "RayanCustomers");

            migrationBuilder.AddColumn<int>(
                name: "FundId",
                table: "RayanCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "BankId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "BankId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsEnabled",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_RayanCustomers_FundId_CustomerId",
                table: "RayanCustomers",
                columns: new[] { "FundId", "CustomerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RayanCustomers_Funds_FundId",
                table: "RayanCustomers",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RayanCustomers_Funds_FundId",
                table: "RayanCustomers");

            migrationBuilder.DropIndex(
                name: "IX_RayanCustomers_FundId_CustomerId",
                table: "RayanCustomers");

            migrationBuilder.DropColumn(
                name: "FundId",
                table: "RayanCustomers");

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "BankId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "BankId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsEnabled",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_RayanCustomers_CustomerId",
                table: "RayanCustomers",
                column: "CustomerId",
                unique: true);
        }
    }
}

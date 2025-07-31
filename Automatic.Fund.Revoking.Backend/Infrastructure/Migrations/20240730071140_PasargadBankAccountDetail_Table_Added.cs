using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasargadBankAccountDetail_Table_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasargadBankAccountDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    ScApiKeyAccountBalance = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ScApiKeyInternal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ScApiKeyPaya = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ScApiKeySatna = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ScApiKeyKYC = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasargadBankAccountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasargadBankAccountDetails_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnabled",
                value: false);

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountNumber", "Balance", "BankId", "CreatedById", "FundId", "IsDeleted", "IsEnabled", "ModifiedAt", "ModifiedById", "ShebaNumber" },
                values: new object[] { 3, "203.110.1419864.1", 0m, 16, 0L, 1, false, true, null, null, "IR190570077700101016288301" });

            migrationBuilder.InsertData(
                table: "PasargadBankAccountDetails",
                columns: new[] { "Id", "BankAccountId", "CreatedAt", "CreatedById", "ModifiedAt", "ModifiedById", "ScApiKeyAccountBalance", "ScApiKeyInternal", "ScApiKeyKYC", "ScApiKeyPaya", "ScApiKeySatna" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, null, "0f8d375954b64fc1a4a92b33e95c5c87.XzIwMjQ3", "edd7c1468e2949d1871f191c369084f3.XzIwMjQ1", "e36f3972a7654900bfab231e1225951e.XzIwMjQ3", "6e036d9932864ded945807f7132ae4da.XzIwMjQ3", "9414ea350a234ece99af089647841cf9.XzIwMjQ3" },
                    { 2, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, null, "0f8d375954b64fc1a4a92b33e95c5c87.XzIwMjQ3", "edd7c1468e2949d1871f191c369084f3.XzIwMjQ1", "e36f3972a7654900bfab231e1225951e.XzIwMjQ3", "6e036d9932864ded945807f7132ae4da.XzIwMjQ3", "9414ea350a234ece99af089647841cf9.XzIwMjQ3" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasargadBankAccountDetails_BankAccountId",
                table: "PasargadBankAccountDetails",
                column: "BankAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasargadBankAccountDetails");

            migrationBuilder.DeleteData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnabled",
                value: true);
        }
    }
}

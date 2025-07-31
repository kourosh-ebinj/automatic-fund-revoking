using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserFunds_SagaTransaction_CustomersIndexedAltered_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistories_Orders_OrderId",
                table: "OrderHistories");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BackOfficeId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_FundId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_MobileNumber",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_NationalCode",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TradingCode",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "FundId",
                table: "Limitations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FundId",
                table: "Banks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SagaTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SagaTransactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FundId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFunds_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "FundId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "FundId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BackOfficeBankId", "FundId" },
                values: new object[] { 8, 1 });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 6,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BackOfficeBankId", "FundId" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 9,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 10,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 11,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 12,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 13,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 14,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 15,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 16,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 17,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 18,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 19,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 20,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 21,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 22,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 23,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 24,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 25,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 26,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 27,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 28,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 29,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 30,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 33, 1, "دي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 34, 1, "قرض الحسنه باب الحوائج " });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 35, 1, "گردشگري" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 36, 1, "توسعه تعاون" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 37, 1, "ايران زمين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 38, 1, "موسسه اعتباري پيشگامان" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 40, 1, "مؤسسه مالي و اعتباري آتي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 42, 1, "موسسه مالي و اعتباري صالحين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 43, 1, "حكمت ايرانيان" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 44, 1, "موسسه مالي و اعتباري فردوسي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 45, 1, "خاورميانه" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 46, 1, "موسسه مالي و اعتباري رضوي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 47, 1, "قرض الحسنه مهر ايران" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 48, 1, "قرض الحسنه رسالت" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 49, 1, "مهر اقتصاد" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 50, 1, "موسسه مالي و اعتباري کوثر" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 51, 1, "مبنا" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 52, 1, "پرشين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 54, 1, "موسسه اعتباری افضل توس" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 55, 1, "موسسه اعتباری نور" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 56, 1, "بانک مشترک ايران و ونزوئلا" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 100, 1, "نامشخص" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 1, 2, "اقتصاد نوين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 46, 2, "موسسه مالي و اعتباري رضوي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "BackOfficeBankId", "FundId", "Name" },
                values: new object[] { 47, 2, "قرض الحسنه مهر ايران" });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "BackOfficeBankId", "Description", "FundId", "IsEnabled", "Name", "ProviderClassName" },
                values: new object[,]
                {
                    { 31, 32, null, 1, false, "تعاوني اعتبار صالحين", "" },
                    { 39, 41, null, 1, false, "موسسه اعتباري ثامن الحجج", "" },
                    { 54, 99, null, 1, false, "عابر بانك", "" },
                    { 57, 2, null, 2, false, "تجارت", "" },
                    { 58, 3, null, 2, false, "توسعه صادرات", "" },
                    { 59, 4, null, 2, false, "رفاه كارگران", "Infrastructure.Services.ThirdParties.Banks.RefahBankingProviderService, Infrastructure" },
                    { 60, 5, null, 2, false, "سامان", "" },
                    { 61, 6, null, 2, false, "سرمايه", "" },
                    { 62, 7, null, 2, false, "سپه", "" },
                    { 63, 8, null, 2, false, "صادرات", "" },
                    { 64, 9, null, 2, false, "صنعت و معدن", "" },
                    { 65, 10, null, 2, false, "مسكن", "" },
                    { 66, 11, null, 2, false, "ملت", "" },
                    { 67, 12, null, 2, false, "ملي", "" },
                    { 68, 13, null, 2, false, "موسسه اعتباري بنياد", "" },
                    { 69, 14, null, 2, false, "قوامين", "" },
                    { 70, 15, null, 2, false, "پارسيان", "" },
                    { 71, 16, null, 2, true, "پاسارگاد", "Infrastructure.Services.ThirdParties.Banks.PasargadBankingProviderService, Infrastructure" },
                    { 72, 17, null, 2, false, "كارآفرين", "" },
                    { 73, 18, null, 2, false, "كشاورزي", "" },
                    { 74, 19, null, 2, false, "شهر", "" },
                    { 75, 20, null, 2, false, "مركزي", "" },
                    { 76, 21, null, 2, false, "پست بانك", "" },
                    { 77, 22, null, 2, false, "موسسه اعتباري مهر", "" },
                    { 78, 23, null, 2, false, "انصار", "" },
                    { 79, 24, null, 2, false, "سينا", "" },
                    { 80, 25, null, 2, false, "صندوق تعاون", "" },
                    { 81, 26, null, 2, false, "موسسه اعتباري توسعه", "" },
                    { 82, 27, null, 2, false, "موسسه مالي مولي الموحدين", "" },
                    { 83, 28, null, 2, false, "موسسه اعتباری ملل", "" },
                    { 84, 29, null, 2, false, "موسسه اعتباري ثامن الائمه ", "" },
                    { 85, 30, null, 2, false, "آينده", "" },
                    { 86, 32, null, 2, false, "تعاوني اعتبار صالحين", "" },
                    { 87, 33, null, 2, false, "دي", "" },
                    { 88, 34, null, 2, false, "قرض الحسنه باب الحوائج ", "" },
                    { 89, 35, null, 2, false, "گردشگري", "" },
                    { 90, 36, null, 2, false, "توسعه تعاون", "" },
                    { 91, 37, null, 2, false, "ايران زمين", "" },
                    { 92, 38, null, 2, false, "موسسه اعتباري پيشگامان", "" },
                    { 93, 40, null, 2, false, "مؤسسه مالي و اعتباري آتي", "" },
                    { 94, 41, null, 2, false, "موسسه اعتباري ثامن الحجج", "" },
                    { 95, 42, null, 2, false, "موسسه مالي و اعتباري صالحين", "" },
                    { 96, 43, null, 2, false, "حكمت ايرانيان", "" },
                    { 97, 44, null, 2, false, "موسسه مالي و اعتباري فردوسي", "" },
                    { 98, 45, null, 2, false, "خاورميانه", "" },
                    { 101, 48, null, 2, false, "قرض الحسنه رسالت", "" },
                    { 102, 49, null, 2, false, "مهر اقتصاد", "" },
                    { 103, 50, null, 2, false, "موسسه مالي و اعتباري کوثر", "" },
                    { 104, 51, null, 2, false, "مبنا", "" },
                    { 105, 52, null, 2, false, "پرشين", "" },
                    { 106, 53, null, 2, false, "موسسه اعتباری افضل توس", "" },
                    { 107, 55, null, 2, false, "موسسه اعتباری نور", "" },
                    { 108, 56, null, 2, false, "بانک مشترک ايران و ونزوئلا", "" },
                    { 109, 99, null, 2, false, "عابر بانك", "" },
                    { 110, 100, null, 2, false, "نامشخص", "" }
                });

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DsCode", "Name" },
                values: new object[] { 11049, "صندوق سرمایه گذاری امین آشنا ایرانیان" });

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DsCode", "Name" },
                values: new object[] { 10784, "صندوق سرمایه گذاری حکمت آشنا ایرانیان" });

            migrationBuilder.InsertData(
                table: "Funds",
                columns: new[] { "Id", "DsCode", "IsDeleted", "IsEnabled", "Name" },
                values: new object[,]
                {
                    { 3, 11223, false, false, "صندوق سرمایه گذاری سهم آشنا" },
                    { 4, 11380, false, false, "صندوق سرمايه گذاری نيكوكاری جايزه علمی و فناوری پيامبر اعظم (ص)" },
                    { 5, 11915, false, false, "صندوق سرمایه گذاری نماد آشنا ایرانیان" },
                    { 6, 126, false, false, "صندوق سرمایه گذاری کامیاب آشنا" }
                });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Enabled", "Error", "LimitationComponentTypeId", "LimitationId", "Title", "Value" },
                values: new object[] { false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 1, "چک کردن حداکثر تعداد واحد", "1000" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, "چک کردن حداقل تعداد واحد", "1" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, "چک کردن حداقل مبلغ درخواست", "100000" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, "چک کردن حداکثر مبلغ درخواست", "1000000000" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Enabled", "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, "لیست کاربران مجاز", "[158,31130]" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, "لیست سامانه های مجاز", "[\"fund_site\"]" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Error", "LimitationComponentTypeId", "LimitationId", "Title", "Value" },
                values: new object[] { "موجودی حساب کافی نیست", (byte)1, 2, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" });

            migrationBuilder.UpdateData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 1,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 2,
                column: "FundId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Limitations",
                columns: new[] { "Id", "CreatedById", "FundId", "LimitationTypeId", "ModifiedAt", "ModifiedById", "Title" },
                values: new object[,]
                {
                    { 3, 0L, 2, (byte)1, null, null, "پیش از ثبت سفارش" },
                    { 4, 0L, 2, (byte)2, null, null, "پیش از پرداخت" }
                });

            migrationBuilder.InsertData(
                table: "UserFunds",
                columns: new[] { "Id", "FundId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 84282L },
                    { 2, 2, 84282L },
                    { 7, 1, 84283L },
                    { 8, 2, 84283L }
                });

            migrationBuilder.InsertData(
                table: "LimitationComponents",
                columns: new[] { "Id", "CreatedById", "Enabled", "Error", "LimitationComponentTypeId", "LimitationId", "ModifiedAt", "ModifiedById", "Title", "Value" },
                values: new object[,]
                {
                    { 8, 0L, false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 3, null, null, "چک کردن حداکثر تعداد واحد", "1000" },
                    { 9, 0L, false, "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, 3, null, null, "چک کردن حداقل تعداد واحد", "1" },
                    { 10, 0L, false, "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, 3, null, null, "چک کردن حداقل مبلغ درخواست", "100000" },
                    { 11, 0L, false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, 3, null, null, "چک کردن حداکثر مبلغ درخواست", "1000000000" },
                    { 12, 0L, true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, 3, null, null, "لیست کاربران مجاز", "[158,31130]" },
                    { 13, 0L, true, "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 3, null, null, "لیست سامانه های مجاز", "[\"fund_site\"]" },
                    { 14, 0L, true, "موجودی حساب کافی نیست", (byte)1, 4, null, null, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" }
                });

            migrationBuilder.InsertData(
                table: "Limitations",
                columns: new[] { "Id", "CreatedById", "FundId", "LimitationTypeId", "ModifiedAt", "ModifiedById", "Title" },
                values: new object[,]
                {
                    { 5, 0L, 3, (byte)1, null, null, "پیش از ثبت سفارش" },
                    { 6, 0L, 3, (byte)2, null, null, "پیش از پرداخت" },
                    { 7, 0L, 4, (byte)1, null, null, "پیش از ثبت سفارش" },
                    { 8, 0L, 4, (byte)2, null, null, "پیش از پرداخت" },
                    { 9, 0L, 5, (byte)1, null, null, "پیش از ثبت سفارش" },
                    { 10, 0L, 5, (byte)2, null, null, "پیش از پرداخت" },
                    { 11, 0L, 6, (byte)1, null, null, "پیش از ثبت سفارش" },
                    { 12, 0L, 6, (byte)2, null, null, "پیش از پرداخت" }
                });

            migrationBuilder.InsertData(
                table: "UserFunds",
                columns: new[] { "Id", "FundId", "UserId" },
                values: new object[,]
                {
                    { 3, 3, 84282L },
                    { 4, 4, 84282L },
                    { 5, 5, 84282L },
                    { 6, 6, 84282L }
                });

            migrationBuilder.InsertData(
                table: "LimitationComponents",
                columns: new[] { "Id", "CreatedById", "Enabled", "Error", "LimitationComponentTypeId", "LimitationId", "ModifiedAt", "ModifiedById", "Title", "Value" },
                values: new object[,]
                {
                    { 15, 0L, false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 5, null, null, "چک کردن حداکثر تعداد واحد", "1000" },
                    { 16, 0L, false, "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, 5, null, null, "چک کردن حداقل تعداد واحد", "1" },
                    { 17, 0L, false, "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, 5, null, null, "چک کردن حداقل مبلغ درخواست", "100000" },
                    { 18, 0L, false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, 5, null, null, "چک کردن حداکثر مبلغ درخواست", "1000000000" },
                    { 19, 0L, true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, 5, null, null, "لیست کاربران مجاز", "[158,31130]" },
                    { 20, 0L, true, "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 5, null, null, "لیست سامانه های مجاز", "[\"fund_site\"]" },
                    { 21, 0L, true, "موجودی حساب کافی نیست", (byte)1, 6, null, null, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" },
                    { 22, 0L, false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 7, null, null, "چک کردن حداکثر تعداد واحد", "1000" },
                    { 23, 0L, false, "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, 7, null, null, "چک کردن حداقل تعداد واحد", "1" },
                    { 24, 0L, false, "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, 7, null, null, "چک کردن حداقل مبلغ درخواست", "100000" },
                    { 25, 0L, false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, 7, null, null, "چک کردن حداکثر مبلغ درخواست", "1000000000" },
                    { 26, 0L, true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, 7, null, null, "لیست کاربران مجاز", "[158,31130]" },
                    { 27, 0L, true, "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 7, null, null, "لیست سامانه های مجاز", "[\"fund_site\"]" },
                    { 28, 0L, true, "موجودی حساب کافی نیست", (byte)1, 8, null, null, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" },
                    { 29, 0L, false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 9, null, null, "چک کردن حداکثر تعداد واحد", "1000" },
                    { 30, 0L, false, "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, 9, null, null, "چک کردن حداقل تعداد واحد", "1" },
                    { 31, 0L, false, "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, 9, null, null, "چک کردن حداقل مبلغ درخواست", "100000" },
                    { 32, 0L, false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, 9, null, null, "چک کردن حداکثر مبلغ درخواست", "1000000000" },
                    { 33, 0L, true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, 9, null, null, "لیست کاربران مجاز", "[158,31130]" },
                    { 34, 0L, true, "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 9, null, null, "لیست سامانه های مجاز", "[\"fund_site\"]" },
                    { 35, 0L, true, "موجودی حساب کافی نیست", (byte)1, 10, null, null, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" },
                    { 36, 0L, false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 11, null, null, "چک کردن حداکثر تعداد واحد", "1000" },
                    { 37, 0L, false, "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, 11, null, null, "چک کردن حداقل تعداد واحد", "1" },
                    { 38, 0L, false, "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, 11, null, null, "چک کردن حداقل مبلغ درخواست", "100000" },
                    { 39, 0L, false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, 11, null, null, "چک کردن حداکثر مبلغ درخواست", "1000000000" },
                    { 40, 0L, true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, 11, null, null, "لیست کاربران مجاز", "[158,31130]" },
                    { 41, 0L, true, "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 11, null, null, "لیست سامانه های مجاز", "[\"fund_site\"]" },
                    { 42, 0L, true, "موجودی حساب کافی نیست", (byte)1, 12, null, null, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Limitations_FundId",
                table: "Limitations",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FundId_BackOfficeId",
                table: "Customers",
                columns: new[] { "FundId", "BackOfficeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FundId_NationalCode",
                table: "Customers",
                columns: new[] { "FundId", "NationalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FundId_TradingCode",
                table: "Customers",
                columns: new[] { "FundId", "TradingCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Banks_FundId",
                table: "Banks",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_SagaTransactions_OrderId",
                table: "SagaTransactions",
                column: "OrderId",
                unique: true,
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_UserFunds_FundId",
                table: "UserFunds",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFunds_UserId",
                table: "UserFunds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFunds_UserId_FundId",
                table: "UserFunds",
                columns: new[] { "UserId", "FundId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_Funds_FundId",
                table: "Banks",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Limitations_Funds_FundId",
                table: "Limitations",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistories_Orders_OrderId",
                table: "OrderHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banks_Funds_FundId",
                table: "Banks");

            migrationBuilder.DropForeignKey(
                name: "FK_Limitations_Funds_FundId",
                table: "Limitations");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistories_Orders_OrderId",
                table: "OrderHistories");

            migrationBuilder.DropTable(
                name: "SagaTransactions");

            migrationBuilder.DropTable(
                name: "UserFunds");

            migrationBuilder.DropIndex(
                name: "IX_Limitations_FundId",
                table: "Limitations");

            migrationBuilder.DropIndex(
                name: "IX_Customers_FundId_BackOfficeId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_FundId_NationalCode",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_FundId_TradingCode",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Banks_FundId",
                table: "Banks");

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Limitations",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "FundId",
                table: "Limitations");

            migrationBuilder.DropColumn(
                name: "FundId",
                table: "Banks");

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "FundId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "BackOfficeBankId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 8,
                column: "BackOfficeBankId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 32, "تعاوني اعتبار صالحين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 33, "دي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 34, "قرض الحسنه باب الحوائج " });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 35, "گردشگري" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 36, "توسعه تعاون" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 37, "ايران زمين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 38, "موسسه اعتباري پيشگامان" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 40, "مؤسسه مالي و اعتباري آتي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 41, "موسسه اعتباري ثامن الحجج" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 42, "موسسه مالي و اعتباري صالحين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 43, "حكمت ايرانيان" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 44, "موسسه مالي و اعتباري فردوسي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 45, "خاورميانه" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 46, "موسسه مالي و اعتباري رضوي" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 47, "قرض الحسنه مهر ايران" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 48, "قرض الحسنه رسالت" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 49, "مهر اقتصاد" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 50, "موسسه مالي و اعتباري کوثر" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 51, "مبنا" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 52, "پرشين" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 53, "موسسه اعتباری افضل توس" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 55, "موسسه اعتباری نور" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 56, "بانک مشترک ايران و ونزوئلا" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 99, "عابر بانك" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "BackOfficeBankId", "Name" },
                values: new object[] { 100, "نامشخص" });

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DsCode", "Name" },
                values: new object[] { 10784, "صندوق حکمت" });

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DsCode", "Name" },
                values: new object[] { 11111, "..." });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Enabled", "Error", "LimitationComponentTypeId", "LimitationId", "Title", "Value" },
                values: new object[] { true, "موجودی حساب کافی نیست", (byte)1, 2, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, "چک کردن حداکثر تعداد واحد", "1000" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, "چک کردن حداقل تعداد واحد", "1" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, "چک کردن حداقل مبلغ درخواست", "100000" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Enabled", "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, "چک کردن حداکثر مبلغ درخواست", "1000000000" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Error", "LimitationComponentTypeId", "Title", "Value" },
                values: new object[] { "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, "لیست کاربران مجاز", "[158,31130]" });

            migrationBuilder.UpdateData(
                table: "LimitationComponents",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Error", "LimitationComponentTypeId", "LimitationId", "Title", "Value" },
                values: new object[] { "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 1, "لیست سامانه های مجاز", "[\"fund_site\"]" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BackOfficeId",
                table: "Customers",
                column: "BackOfficeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FundId",
                table: "Customers",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MobileNumber",
                table: "Customers",
                column: "MobileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_NationalCode",
                table: "Customers",
                column: "NationalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TradingCode",
                table: "Customers",
                column: "TradingCode");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistories_Orders_OrderId",
                table: "OrderHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

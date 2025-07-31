using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BackOfficeBankId = table.Column<int>(type: "int", nullable: false),
                    ProviderClassName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DsCode = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Limitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LimitationTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RayanCustomers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Personality = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CustomerFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Representative = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    BirthCertificationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalIdentifier = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    CustomerStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CellPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    BourseCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RayanCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RayanFundOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundOrderId = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: true),
                    OrderAmount = table.Column<long>(type: "bigint", nullable: true),
                    customerAccountNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NationalCode = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    FoStatusName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FundOrderNumber = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    FundUnit = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreationDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CreationTime = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    ModificationDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ModificationTime = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    LicenseDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    GuaranteeAmount = table.Column<long>(type: "bigint", nullable: false),
                    VatAmount = table.Column<long>(type: "bigint", nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AppuserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FundIssueTypeName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OrderPaymentTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FundIssueOriginName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FixWage = table.Column<long>(type: "bigint", nullable: false),
                    VariableWage = table.Column<long>(type: "bigint", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    BranchCode = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    DlNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    LicenseNumber = table.Column<long>(type: "bigint", unicode: false, maxLength: 10, nullable: false),
                    ReceiptComment = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    VoucherNumber = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    VoucherTempNumber = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RayanFundOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShebaNumber = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    FundId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackOfficeId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NationalCode = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    TradingCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    MobileNumber = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    FundId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LimitationComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LimitationId = table.Column<int>(type: "int", nullable: false),
                    LimitationComponentTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Error = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitationComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LimitationComponents_Limitations_LimitationId",
                        column: x => x.LimitationId,
                        principalTable: "Limitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShebaNumber = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerBankAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    TotalUnits = table.Column<int>(type: "int", nullable: false),
                    OrderStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    OrderStatusDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AppCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerNationalCode = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    CustomerAccountSheba = table.Column<string>(type: "varchar(27)", unicode: false, maxLength: 27, nullable: true),
                    CustomerAccountNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CustomerAccountBankId = table.Column<int>(type: "int", nullable: false),
                    RayanFundOrderId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Banks_CustomerAccountBankId",
                        column: x => x.CustomerAccountBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_RayanFundOrders_RayanFundOrderId",
                        column: x => x.RayanFundOrderId,
                        principalTable: "RayanFundOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankPayments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    SourceBankAccountId = table.Column<int>(type: "int", nullable: false),
                    DestinationShebaNumber = table.Column<string>(type: "varchar(27)", unicode: false, maxLength: 27, nullable: true),
                    DestinationBankId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    BankUniqueId = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankPaymentMethodId = table.Column<byte>(type: "tinyint", nullable: false),
                    TransactionStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankPayments_BankAccounts_SourceBankAccountId",
                        column: x => x.SourceBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankPayments_Banks_DestinationBankId",
                        column: x => x.DestinationBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankPayments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    TotalUnits = table.Column<int>(type: "int", nullable: false),
                    OrderStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    OrderStatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackOfficeOrderId = table.Column<long>(type: "bigint", nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerNationalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAccountSheba = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAccountBankId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "BackOfficeBankId", "Description", "IsEnabled", "Name", "ProviderClassName" },
                values: new object[,]
                {
                    { 1, 1, null, false, "اقتصاد نوين", "" },
                    { 2, 2, null, false, "تجارت", "" },
                    { 3, 3, null, false, "توسعه صادرات", "" },
                    { 4, 4, null, false, "رفاه كارگران", "Infrastructure.Services.ThirdParties.Banks.RefahBankingProviderService, Infrastructure" },
                    { 5, 5, null, false, "سامان", "" },
                    { 6, 6, null, false, "سرمايه", "" },
                    { 7, 7, null, false, "سپه", "" },
                    { 8, 8, null, false, "صادرات", "" },
                    { 9, 9, null, false, "صنعت و معدن", "" },
                    { 10, 10, null, false, "مسكن", "" },
                    { 11, 11, null, false, "ملت", "" },
                    { 12, 12, null, false, "ملي", "" },
                    { 13, 13, null, false, "موسسه اعتباري بنياد", "" },
                    { 14, 14, null, false, "قوامين", "" },
                    { 15, 15, null, false, "پارسيان", "" },
                    { 16, 16, null, true, "پاسارگاد", "Infrastructure.Services.ThirdParties.Banks.PasargadBankingProviderService, Infrastructure" },
                    { 17, 17, null, false, "كارآفرين", "" },
                    { 18, 18, null, false, "كشاورزي", "" },
                    { 19, 19, null, false, "شهر", "" },
                    { 20, 20, null, false, "مركزي", "" },
                    { 21, 21, null, false, "پست بانك", "" },
                    { 22, 22, null, false, "موسسه اعتباري مهر", "" },
                    { 23, 23, null, false, "انصار", "" },
                    { 24, 24, null, false, "سينا", "" },
                    { 25, 25, null, false, "صندوق تعاون", "" },
                    { 26, 26, null, false, "موسسه اعتباري توسعه", "" },
                    { 27, 27, null, false, "موسسه مالي مولي الموحدين", "" },
                    { 28, 28, null, false, "موسسه اعتباری ملل", "" },
                    { 29, 29, null, false, "موسسه اعتباري ثامن الائمه ", "" },
                    { 30, 30, null, false, "آينده", "" },
                    { 32, 32, null, false, "تعاوني اعتبار صالحين", "" },
                    { 33, 33, null, false, "دي", "" },
                    { 34, 34, null, false, "قرض الحسنه باب الحوائج ", "" },
                    { 35, 35, null, false, "گردشگري", "" },
                    { 36, 36, null, false, "توسعه تعاون", "" },
                    { 37, 37, null, false, "ايران زمين", "" },
                    { 38, 38, null, false, "موسسه اعتباري پيشگامان", "" },
                    { 40, 40, null, false, "مؤسسه مالي و اعتباري آتي", "" },
                    { 41, 41, null, false, "موسسه اعتباري ثامن الحجج", "" },
                    { 42, 42, null, false, "موسسه مالي و اعتباري صالحين", "" },
                    { 43, 43, null, false, "حكمت ايرانيان", "" },
                    { 44, 44, null, false, "موسسه مالي و اعتباري فردوسي", "" },
                    { 45, 45, null, false, "خاورميانه", "" },
                    { 46, 46, null, false, "موسسه مالي و اعتباري رضوي", "" },
                    { 47, 47, null, false, "قرض الحسنه مهر ايران", "" },
                    { 48, 48, null, false, "قرض الحسنه رسالت", "" },
                    { 49, 49, null, false, "مهر اقتصاد", "" },
                    { 50, 50, null, false, "موسسه مالي و اعتباري کوثر", "" },
                    { 51, 51, null, false, "مبنا", "" },
                    { 52, 52, null, false, "پرشين", "" },
                    { 53, 53, null, false, "موسسه اعتباری افضل توس", "" },
                    { 55, 55, null, false, "موسسه اعتباری نور", "" },
                    { 56, 56, null, false, "بانک مشترک ايران و ونزوئلا", "" },
                    { 99, 99, null, false, "عابر بانك", "" },
                    { 100, 100, null, false, "نامشخص", "" }
                });

            migrationBuilder.InsertData(
                table: "Funds",
                columns: new[] { "Id", "DsCode", "IsDeleted", "IsEnabled", "Name" },
                values: new object[,]
                {
                    { 1, 10784, false, true, "صندوق حکمت" },
                    { 2, 11111, false, false, "..." }
                });

            migrationBuilder.InsertData(
                table: "Limitations",
                columns: new[] { "Id", "CreatedById", "LimitationTypeId", "ModifiedAt", "ModifiedById", "Title" },
                values: new object[,]
                {
                    { 1, 0L, (byte)1, null, null, "پیش از ثبت سفارش" },
                    { 2, 0L, (byte)2, null, null, "پیش از پرداخت" }
                });

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountNumber", "Balance", "BankId", "CreatedById", "FundId", "IsDeleted", "IsEnabled", "ModifiedAt", "ModifiedById", "ShebaNumber" },
                values: new object[,]
                {
                    { 1, "415.8100.41581000.1", 0m, 16, 0L, 1, false, true, null, null, "IR510570041581041581000101" },
                    { 2, "203.110.1419864.1", 0m, 16, 0L, 1, false, true, null, null, "IR190570077700101016288301" }
                });

            migrationBuilder.InsertData(
                table: "LimitationComponents",
                columns: new[] { "Id", "CreatedById", "Enabled", "Error", "LimitationComponentTypeId", "LimitationId", "ModifiedAt", "ModifiedById", "Title", "Value" },
                values: new object[,]
                {
                    { 1, 0L, true, "موجودی حساب کافی نیست", (byte)1, 2, null, null, "چک کردن موجودی حساب", "{ \"MinBalance\": 100000000 }" },
                    { 2, 0L, false, "تعداد واحدها بیشتر از بازه تعیین شده است", (byte)2, 1, null, null, "چک کردن حداکثر تعداد واحد", "1000" },
                    { 3, 0L, false, "تعداد واحدها کمتر از بازه تعیین شده است", (byte)3, 1, null, null, "چک کردن حداقل تعداد واحد", "1" },
                    { 4, 0L, false, "مبلغ درخواست کمتر از بازه تعیین شده است", (byte)4, 1, null, null, "چک کردن حداقل مبلغ درخواست", "100000" },
                    { 5, 0L, false, "مبلغ درخواست بیشتر از بازه تعیین شده است", (byte)5, 1, null, null, "چک کردن حداکثر مبلغ درخواست", "1000000000" },
                    { 6, 0L, true, "ارایه سرویس به کاربر جاری ممکن نیست", (byte)6, 1, null, null, "لیست کاربران مجاز", "[158,31130]" },
                    { 7, 0L, true, "ارایه سرویس به سامانه جاری مجاز نیست", (byte)7, 1, null, null, "لیست سامانه های مجاز", "[\"fund_site\"]" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankId",
                table: "BankAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_FundId",
                table: "BankAccounts",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_BankPayments_DestinationBankId",
                table: "BankPayments",
                column: "DestinationBankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankPayments_OrderId",
                table: "BankPayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BankPayments_SourceBankAccountId",
                table: "BankPayments",
                column: "SourceBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBankAccounts_CustomerId",
                table: "CustomerBankAccounts",
                column: "CustomerId");

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

            migrationBuilder.CreateIndex(
                name: "IX_LimitationComponents_LimitationId",
                table: "LimitationComponents",
                column: "LimitationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_OrderId",
                table: "OrderHistories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerAccountBankId",
                table: "Orders",
                column: "CustomerAccountBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RayanFundOrderId",
                table: "Orders",
                column: "RayanFundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RayanCustomers_CellPhone",
                table: "RayanCustomers",
                column: "CellPhone");

            migrationBuilder.CreateIndex(
                name: "IX_RayanCustomers_CustomerId",
                table: "RayanCustomers",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RayanFundOrders_FundOrderId",
                table: "RayanFundOrders",
                column: "FundOrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankPayments");

            migrationBuilder.DropTable(
                name: "CustomerBankAccounts");

            migrationBuilder.DropTable(
                name: "LimitationComponents");

            migrationBuilder.DropTable(
                name: "OrderHistories");

            migrationBuilder.DropTable(
                name: "RayanCustomers");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Limitations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "RayanFundOrders");

            migrationBuilder.DropTable(
                name: "Funds");
        }
    }
}

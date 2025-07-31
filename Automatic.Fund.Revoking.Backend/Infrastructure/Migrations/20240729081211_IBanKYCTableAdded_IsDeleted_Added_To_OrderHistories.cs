using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IBanKYCTableAdded_IsDeleted_Added_To_OrderHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IBanKYCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IBan = table.Column<string>(type: "varchar(26)", unicode: false, maxLength: 26, nullable: true),
                    IsKYCCompliant = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IBanKYCs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IBanKYCs_IBan",
                table: "IBanKYCs",
                column: "IBan",
                unique: true,
                filter: "[IBan] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IBanKYCs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderHistories");
        }
    }
}

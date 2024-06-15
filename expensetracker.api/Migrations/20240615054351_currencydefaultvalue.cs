using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetracker.api.Migrations
{
    /// <inheritdoc />
    public partial class currencydefaultvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Expenses",
                type: "TEXT",
                nullable: false,
                defaultValue: "USD",
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Expenses",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "USD");
        }
    }
}

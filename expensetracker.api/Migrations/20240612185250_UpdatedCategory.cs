using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetracker.api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Catagory",
                table: "Expenses",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Expenses",
                newName: "Catagory");
        }
    }
}
